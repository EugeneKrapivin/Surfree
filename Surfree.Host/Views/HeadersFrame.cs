using Spectre.Console;

using System.ComponentModel;
using System.Data;

using Terminal.Gui;

namespace Surfree.Host.Views;

internal class HeadersFrame : FrameView
{
    private DataTable _dt;

    public HeadersFrame(bool allowEdit = false)
    {
        X = 0;
        Y = 0;
        Width = Dim.Fill();
        Height = Dim.Fill();
        BorderStyle = LineStyle.None;
        
        _dt = CreateHeadersTable();
        var headersTableView = new TableView
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill(2),
            Table = new DataTableSource(_dt),
        };
        headersTableView.Style.ShowHorizontalBottomline = true;
        headersTableView.BorderStyle = LineStyle.Rounded;

        Add(headersTableView);

        if (allowEdit)
        {
            var addHeaderButton = new Button()
            {
                X = 0,
                Y = Pos.Bottom(headersTableView) + 1,
                Text = "Add Header"
            };

            addHeaderButton.Accept += (sender, e) =>
            {
                var addDialog = new AddHeaderDialog(_dt);
                Application.Run(addDialog);
            };

            Add(addHeaderButton);

            headersTableView.CellActivated += (sender, e) =>
            {
                if (e.Col == 0)
                {
                    var row = _dt.Rows[e.Row];
                    row["Enabled"] = row["Enabled"] as string == "[x]" ? "[ ]" : "[x]";
                    headersTableView.Draw();
                    _dt.AcceptChanges();
                    return;
                }
                var editDialog = new EditHeaderDialog(_dt, e.Row);
                Application.Run(editDialog);
            };
        }
    }

    private DataTable CreateHeadersTable()
    {
        var tbl = new DataTable();
        var nameColumn = new DataColumn
        {
            DataType = typeof(string),
            ColumnName = "Name",
            Unique = true,
            Caption = "Name",
        };
        tbl.Columns.Add(new DataColumn
        {
            DataType = typeof(string),
            ColumnName = "Enabled",
            Unique = false,
            Caption = "Enabled",
            DefaultValue = "[x]",
        });
        tbl.Columns.Add(nameColumn);
        tbl.Columns.Add(new DataColumn
        {
            DataType = typeof(string),
            ColumnName = "Value",
            Unique = false,
            Caption = "Value",
        });
        tbl.PrimaryKey = [nameColumn];
        return tbl;
    }

    public IEnumerable<RequestHeaders> GetHeaders()
    {
        foreach (DataRow row in _dt.Rows)
        {
            if (row["Enabled"] as string == "[x]")
            {
                yield return new RequestHeaders
                {
                    HeaderName = row["Name"] as string ?? "",
                    HeaderValue = row["Value"] as string ?? "",
                };
            }
        }
    }
}

public sealed class RequestHeaders
{
    public bool Enabled { get; set; }
    public required string HeaderName { get; set; }
    public required string HeaderValue { get; set; }
}

public abstract class HeaderDialog : Dialog
{
    protected readonly DataTable _dt;
    protected readonly TextField _nameField;
    protected readonly TextField _valueField;
    protected readonly TextView _textView;

    protected abstract string _title { get; }

    public HeaderDialog(DataTable dt)
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

public sealed class EditHeaderDialog : HeaderDialog
{
    private readonly int _rowIndex;
    private DataRow _row;

    public EditHeaderDialog(DataTable dt, int rowIndex) : base(dt)
    {
        _row = dt.Rows[rowIndex];
        _nameField.Text = _row["Name"] as string;
        _valueField.Text = _row["Value"] as string;
        _rowIndex = rowIndex;
    }

    protected override string _title => "Edit header";

    protected override void OkAccept(object? sender, HandledEventArgs args)
    {
        var newHeaderName = _nameField.Text;
        var newHeaderValue = _valueField.Text;

        if (string.IsNullOrEmpty(newHeaderName))
        {
            _textView.Text = "Header name can not be empty";
            _textView.Visible = true;
            _textView.Width = Dim.Fill(2);
            _textView.TextAlignment = Alignment.Center;
            _textView.VerticalTextAlignment = Alignment.Center;

            args.Handled = false;
            return;
        }

        if (_dt.Rows.Contains(newHeaderName))
        {
            var suspectedRow = _dt.Rows.Find(newHeaderName);
            var suspetedRowIndex = _dt.Rows.IndexOf(suspectedRow);
            if (suspetedRowIndex != _rowIndex)
            {
                _textView.Text = "Header already exists";
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

internal sealed class AddHeaderDialog : HeaderDialog
{
    public AddHeaderDialog(DataTable dt) : base(dt)
    {
    }

    protected override string _title => "Add header";

    protected override void OkAccept(object? sender, HandledEventArgs args)
    {
        var addedHeaderName = _nameField.Text;
        var addedValue = _valueField.Text;

        if (string.IsNullOrEmpty(addedHeaderName))
        {
            _textView.Text = "Header name can not be empty";
            _textView.Visible = true;
            _textView.Width = Dim.Fill(2);
            _textView.TextAlignment = Alignment.Center;
            _textView.VerticalTextAlignment = Alignment.Center;
            
            args.Handled = false;
            return;
        }

        if (_dt.Rows.Contains(addedHeaderName))
        {
            _textView.Text = "Header already exists";
            _textView.Visible = true;
            _textView.Width = Dim.Fill(2);
            _textView.TextAlignment = Alignment.Center;
            _textView.VerticalTextAlignment = Alignment.Center;

            args.Handled = false;
            return;
        }
        var row = _dt.NewRow();
        row["Name"] = addedHeaderName;
        row["Value"] = addedValue;
        _dt.Rows.Add(row);
        _dt.AcceptChanges();

        args.Handled = true;
    }
}
