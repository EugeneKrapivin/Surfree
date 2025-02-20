using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terminal.Gui;

namespace Surfree.Host.Views;

internal class RequestFrame : FrameView
{
    public RequestFrame()
    {
        Title = "Request";
        BorderStyle = LineStyle.Rounded;
        
        Height = Dim.Fill();
        Width = Dim.Fill();

        var urlView = new RequestUrlFrame()
        {
            X = 1,
            Y = 0,
            Width = Dim.Fill(1),
            BorderStyle = LineStyle.None
        };

        var tabView = new TabView()
        {
            X = 1,
            Y = Pos.Bottom(urlView),
            Width = Dim.Fill(1),
            Height = Dim.Fill(),
            BorderStyle = LineStyle.None
        };
       
        var headersTab = new Tab() 
        {
            DisplayText = "Headers",
            X = 0,
            Y = 0,
            Height = Dim.Fill(), 
            Width = Dim.Fill(), 
            View = new HeadersFrame(true)
        };

        var bodyTab = new Tab() 
        { 
            DisplayText = "Body",
            X = 1,
            Y = 0,
            Height = Dim.Fill(),
            Width = Dim.Fill(1),
            View = new RequestBodyFrame()
        };
        var query = new Tab() 
        { 
            DisplayText = "Query",
            X = 1,
            Y = 0,
            Height = Dim.Fill(),
            Width = Dim.Fill(1),
            View = new RequestQueryFrame(true)
        };
        var auth = new Tab() { DisplayText = "Auth" };

        tabView.AddTab(headersTab, true);
        tabView.AddTab(bodyTab, false);
        tabView.AddTab(query, false);
        tabView.AddTab(auth, false);
        
        Add(urlView, tabView);
    }
}

internal sealed class RequestBodyFrame : FrameView
{
    public RequestBodyFrame()
    {
        Title = "Body";
        BorderStyle = LineStyle.None;
        CanFocus = false;
        Height = Dim.Fill();
        Width = Dim.Fill(1);
        X = 1;

        var contentTypeLabel = new Label()
        {
            X = 1,
            Y = 0,
            Width = Dim.Auto(),
            Height = 1,
            Text = "Content-Type:"
        };

        var contentTypeCombo = new ComboBox()
        {
            X = Pos.Right(contentTypeLabel) + 1,
            Y = 0,
            Width = 40,
            Height = 4,
            ReadOnly = true,
            CanFocus = false,
            HideDropdownListOnClick = true
        };
        var source = new ObservableCollection<string>
        {
            "application/json",
            "application/x-www-form-urlencoded",
            "application/xml"
        };
        contentTypeCombo.SetSource(source);

        var jsonTextView = new TextView
        {
            Visible = true,
            Text = """
            { 
            }
            """,
            Width = Dim.Fill(1),
            Height = Dim.Fill(1),
            X = 1,
            Y = Pos.Bottom(contentTypeCombo),
        };

        var xmlTextView = new TextView
        {
            Visible = false,
            Text = """
            <root> 
            </root>
            """,
            Width = Dim.Fill(1),
            Height = Dim.Fill(1),
            X = 1,
            Y = Pos.Bottom(contentTypeCombo),
        };

        var formTableView = new TableView
        {
            Visible = false,
            Width = Dim.Fill(1),
            Height = Dim.Fill(1),
            X = 1,
            Y = Pos.Bottom(contentTypeCombo),
            Table = new DataTableSource(new DataTable() { Columns = { "Name", "Value" } }),
        };
        
        formTableView.Style.ShowHorizontalBottomline = true;
        formTableView.Style.ShowHorizontalScrollIndicators = false;
        formTableView.BorderStyle = LineStyle.Rounded;

        Add(formTableView, jsonTextView, xmlTextView, contentTypeLabel, contentTypeCombo);

        contentTypeCombo.SelectedItemChanged += (sender, e) =>
        {
            if (contentTypeCombo.Text == "application/json")
            {
                jsonTextView.Visible = true;
                formTableView.Visible = false;
                xmlTextView.Visible = false;
            }
            else if (contentTypeCombo.Text == "application/x-www-form-urlencoded")
            {
                jsonTextView.Visible = false;
                formTableView.Visible = true;
                xmlTextView.Visible = false;
            }
            else
            {
                xmlTextView.Visible = true;
                jsonTextView.Visible = false;
                formTableView.Visible = false;
            }
        };
    }
}

internal sealed class RequestQueryFrame : FrameView
{
    private DataTable _dt;

    public RequestQueryFrame(bool allowEdit = false)
    {
        Title = "Query";
        BorderStyle = LineStyle.None;
        Height = Dim.Fill();
        Width = Dim.Fill(1);
        X = 1;

        _dt = CreateQueryTable();
        var queryTableView = new TableView
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill(2),
            Table = new DataTableSource(_dt),
            CellActivationKey = KeyCode.Enter,
        };
        queryTableView.Style.ShowHorizontalBottomline = true;
        queryTableView.Style.ShowHorizontalScrollIndicators = false;
        queryTableView.BorderStyle = LineStyle.None;
        
        Add(queryTableView);

        if (allowEdit)
        {
            var addQueryButton = new Button()
            {
                X = 0,
                Y = Pos.Bottom(queryTableView) + 1,
                Text = "Add Query"
            };

            addQueryButton.Accept += (sender, e) =>
            {
                var addDialog = new AddQueryDialog(_dt);
                Application.Run(addDialog);
            };

            Add(addQueryButton);

            queryTableView.CellActivated += (sender, e) =>
            {
                var editDialog = new EditQueryDialog(_dt, e.Row);
                Application.Run(editDialog);
            };
        }
    }

    private DataTable CreateQueryTable()
    {
        var tbl = new DataTable();
        var nameColumn = new DataColumn
        {
            DataType = typeof(string),
            ColumnName = "Name",
            Unique = true,
            Caption = "Name",
        };
        tbl.Columns.Add(nameColumn);
        tbl.Columns.Add(new DataColumn
        {
            DataType = typeof(string),
            ColumnName = "Value",
            Unique = false,
            Caption = "Value",
        });
        tbl.PrimaryKey = new[] { nameColumn };
        return tbl;
    }
}

public abstract class QueryDialog : Dialog
{
    protected readonly DataTable _dt;
    protected readonly TextField _nameField;
    protected readonly TextField _valueField;
    protected readonly TextView _textView;

    protected abstract string _title { get; }

    public QueryDialog(DataTable dt)
    {
        _dt = dt;
        Title = _title;
        Height = Dim.Auto();
        Width = 50;

        var nameLabel = new Label()
        {
            Text = "Name:",
            X = 1,
            Y = 1,
            Width = Dim.Percent(15)
        };

        _nameField = new TextField
        {
            X = Pos.Right(nameLabel) + 1,
            Y = Pos.Top(nameLabel),
            Width = Dim.Fill(1),
        };

        var valueLabel = new Label
        {
            Text = "Value:",
            X = 1,
            Y = Pos.Bottom(nameLabel) + 1,
            Width = Dim.Percent(15)
        };
        _valueField = new TextField
        {
            X = Pos.Right(valueLabel) + 1,
            Y = Pos.Top(valueLabel),
            Width = Dim.Fill(1)
        };

        _textView = new TextView()
        {
            Height = 3,
            X = Pos.Center(),
            Y = Pos.Bottom(_valueField) + 1,
            Visible = false
        };

        var buttons = new FrameView()
        {
            Height = Dim.Auto(),
            Width = Dim.Auto(),
            X = Pos.Center(),
            Y = Pos.Bottom(_textView) + 2,
            BorderStyle = LineStyle.None
        };
        var okButton = new Button()
        {
            Text = "OK",
            X = 0,
            Y = 0,
            CanFocus = true,
            IsDefault = true
        };

        var cancelButton = new Button()
        {
            Text = "Cancel",
            X = Pos.Right(okButton) + 2,
            Y = 0,
            CanFocus = true
        };
        buttons.Add(okButton, cancelButton);
        okButton.Accept += (sender, e) =>
        {
            OkAccept(sender, e);
            if (e.Handled)
                Application.RequestStop();
        };

        cancelButton.Accept += (sender, e) =>
        {
            Application.RequestStop();
        };

        Add(nameLabel, _nameField, valueLabel, _valueField, buttons, _textView);
    }

    protected abstract void OkAccept(object? sender, HandledEventArgs args);
}

public sealed class EditQueryDialog : QueryDialog
{
    private readonly int _rowIndex;
    private DataRow _row;

    public EditQueryDialog(DataTable dt, int rowIndex) : base(dt)
    {
        _row = dt.Rows[rowIndex];
        _nameField.Text = _row["Name"] as string;
        _valueField.Text = _row["Value"] as string;
        _rowIndex = rowIndex;
    }

    protected override string _title => "Edit Query";

    protected override void OkAccept(object? sender, HandledEventArgs args)
    {
        var newQueryName = _nameField.Text;
        var newQueryValue = _valueField.Text;

        if (string.IsNullOrEmpty(newQueryName))
        {
            _textView.Text = "Query name cannot be empty";
            _textView.Visible = true;
            _textView.Width = Dim.Fill(2);
            _textView.TextAlignment = Alignment.Center;
            _textView.VerticalTextAlignment = Alignment.Center;

            args.Handled = false;
            return;
        }

        if (_dt.Rows.Contains(newQueryName))
        {
            var suspectedRow = _dt.Rows.Find(newQueryName);
            var suspectedRowIndex = _dt.Rows.IndexOf(suspectedRow);
            if (suspectedRowIndex != _rowIndex)
            {
                _textView.Text = "Query already exists";
                _textView.Visible = true;
                _textView.Width = Dim.Fill(2);
                _textView.TextAlignment = Alignment.Center;
                _textView.VerticalTextAlignment = Alignment.Center;

                args.Handled = false;
                return;
            }
        }

        _row["Name"] = _nameField.Text;
        _row["Value"] = _valueField.Text;

        _row.AcceptChanges();

        args.Handled = true;
    }
}

internal sealed class AddQueryDialog : QueryDialog
{
    public AddQueryDialog(DataTable dt) : base(dt)
    {
    }

    protected override string _title => "Add Query";

    protected override void OkAccept(object? sender, HandledEventArgs args)
    {
        var addedQueryName = _nameField.Text;
        var addedValue = _valueField.Text;

        if (string.IsNullOrEmpty(addedQueryName))
        {
            _textView.Text = "Query name cannot be empty";
            _textView.Visible = true;
            _textView.Width = Dim.Fill(2);
            _textView.TextAlignment = Alignment.Center;
            _textView.VerticalTextAlignment = Alignment.Center;

            args.Handled = false;
            return;
        }

        if (_dt.Rows.Contains(addedQueryName))
        {
            _textView.Text = "Query already exists";
            _textView.Visible = true;
            _textView.Width = Dim.Fill(2);
            _textView.TextAlignment = Alignment.Center  ;
            _textView.VerticalTextAlignment = Alignment.Center;

            args.Handled = false;
            return;
        }
        var row = _dt.NewRow();
        row["Name"] = addedQueryName;
        row["Value"] = addedValue;
        _dt.Rows.Add(row);
        _dt.AcceptChanges();

        args.Handled = true;
    }
}