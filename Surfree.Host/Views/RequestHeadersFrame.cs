using Surfree.Host.ViewModels;
using Surfree.Host.Views.ResponseViews;

using System.ComponentModel;
using System.Data;
using System.Reflection.PortableExecutable;

using Terminal.Gui;

namespace Surfree.Host.Views;

public class RequestHeadersFrame : FrameView
{
    private DataTable _dt;
    private TableView _headersTableView;

    public RequestHeadersFrame(RequestViewModel viewModel)
    {
        X = 1;
        Y = 0;
        Width = Dim.Fill(1);
        Height = Dim.Fill();
        BorderStyle = LineStyle.None;
        CanFocus = false;
        Title = "test";

        _dt = CreateHeadersTable(viewModel.Headers.Values);

        _headersTableView = new TableView
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill(1),
            Table = new DataTableSource(_dt),
        };
        _headersTableView.Style.ShowHorizontalBottomline = true;
        _headersTableView.BorderStyle = LineStyle.None;

        Add(_headersTableView);

        var addHeaderButton = new Button()
        {
            X = 0,
            Y = Pos.Bottom(_headersTableView),
            Text = "Add Header"
        };

        addHeaderButton.Accept += (sender, e) =>
        {
            var addDialog = new AddHeaderDialog(viewModel, _dt);
            Application.Run(addDialog);
        };

        Add(addHeaderButton);

        _headersTableView.CellActivated += (sender, e) =>
        {
            var row = _dt.Rows[e.Row];
            var existing = viewModel.Headers.TryGetValue((string)row["Name"], out var headerViewModel);
            if (!existing || headerViewModel is null)
            {
                throw new Exception("missing header in backing model");
            }
            if (e.Col == 0)
            {
                row["Enabled"] = row["Enabled"] as string == "  [x]  " ? "  [ ]  " : "  [x]  ";

                viewModel.Headers[(string)row["Name"]] = headerViewModel with { Enabled = row["Enabled"] as string == "  [x]  " };
            }
            else
            {
                var currentName = (string)row["Name"];

                var currentValue = row["Value"] as string;

                var editDialog = new EditHeaderDialog(_dt, e.Row);
                Application.Run(editDialog);

                var newName = (string)row["Name"];
                var newValue = (string)row["Value"];

                viewModel.Headers.Remove(currentName);
                viewModel.Headers.Add(newName, headerViewModel with { HeaderName = newName, HeaderValue = newValue });
            }

            _dt.AcceptChanges();
        };
    }

    private DataTable CreateHeadersTable(IEnumerable<RequestHeader> headers)
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
            DefaultValue = "  [x]  ",
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

        foreach (var header in headers)
        {
            var row = _dt.NewRow();
            row["Name"] = header.HeaderName;
            row["Value"] = header.HeaderValue;
            row["Enabled"] = header.Enabled ? "  [x]  " : "  [ ]  ";
            
            _dt.Rows.Add(row);
            _dt.AcceptChanges();
        }

        return tbl;
    }
}

public class ResponseHeadersFrame : FrameView
{
    private DataTable _dt;
    private TableView _headersTableView;

    public ResponseHeadersFrame(ResponseViewModel viewModel)
    {
        X = 1;
        Y = 0;
        Width = Dim.Fill(1);
        Height = Dim.Fill();
        BorderStyle = LineStyle.None;
        CanFocus = false;
        Title = "test";

        _dt = CreateHeadersTable(viewModel.Headers.Values);

        _headersTableView = new TableView
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill(1),
            Table = new DataTableSource(_dt),
        };
        _headersTableView.Style.ShowHorizontalBottomline = true;
        _headersTableView.BorderStyle = LineStyle.None;

        Add(_headersTableView);


        viewModel.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == nameof(ResponseViewModel.Headers))
            {
                _dt = CreateHeadersTable(viewModel.Headers.Values);
                _headersTableView.Table = new DataTableSource(_dt);
                
                _headersTableView.Draw();
            }
        };

    }

    private DataTable CreateHeadersTable(IEnumerable<RequestHeader> headers)
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
        tbl.PrimaryKey = [nameColumn];

        foreach (var header in headers)
        {
            var row = tbl.NewRow();
            row["Name"] = header.HeaderName;
            row["Value"] = header.HeaderValue;

            tbl.Rows.Add(row);
            tbl.AcceptChanges();
        }

        return tbl;
    }
}


public abstract class HeaderDialog : Dialog
{
    protected readonly DataTable _dt;
    protected readonly TextField _nameField;
    protected readonly TextField _valueField;
    protected readonly Label _errorLabel;

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

        _errorLabel = new Label()
        {
            Height = 1,
            Width = Dim.Percent(75),
            X = Pos.Center(),
            Y = Pos.Bottom(_valueField) + 1,
            Visible = false,
            ColorScheme = new ColorScheme(new Terminal.Gui.Attribute(ColorName.Red, ColorScheme.Normal.Background)),
            CanFocus = false
        };

        var buttons = new FrameView()
        {
            Height = Dim.Auto(),
            Width = Dim.Auto(),
            X = Pos.Center(),
            Y = Pos.Bottom(_errorLabel) + 2,
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

        Add(nameLabel, _nameField, valueLabel, _valueField, buttons, _errorLabel);
    }

    protected abstract void OkAccept(object? sender, HandledEventArgs args);

    protected virtual bool ValidateInput()
    {
        return true;
    }
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
            _errorLabel.Text = "Header name can not be empty";
            _errorLabel.Visible = true;
            _errorLabel.TextAlignment = Alignment.Center;
            _errorLabel.VerticalTextAlignment = Alignment.Center;

            args.Handled = false;
            return;
        }

        if (_dt.Rows.Contains(newHeaderName))
        {
            var suspectedRow = _dt.Rows.Find(newHeaderName);
            var suspetedRowIndex = _dt.Rows.IndexOf(suspectedRow);
            if (suspetedRowIndex != _rowIndex)
            {
                _errorLabel.Text = "Header already exists";
                _errorLabel.Visible = true;
                _errorLabel.TextAlignment = Alignment.Center;
                _errorLabel.VerticalTextAlignment = Alignment.Center;

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

public sealed class AddHeaderDialog : HeaderDialog
{
    private readonly RequestViewModel _viewModel;

    public AddHeaderDialog(RequestViewModel viewModel, DataTable dt) : base(dt)
    {
        _viewModel = viewModel;
    }

    protected override string _title => "Add header";

    protected override void OkAccept(object? sender, HandledEventArgs args)
    {
        var addedHeaderName = _nameField.Text;
        var addedValue = _valueField.Text;

        if (string.IsNullOrEmpty(addedHeaderName))
        {
            _errorLabel.Text = "Header name can not be empty";
            _errorLabel.Visible = true;
            _errorLabel.TextAlignment = Alignment.Center;
            _errorLabel.VerticalTextAlignment = Alignment.Center;

            args.Handled = false;
            return;
        }

        if (_dt.Rows.Contains(addedHeaderName))
        {
            _errorLabel.Text = "Header already exists";
            _errorLabel.Visible = true;
            _errorLabel.TextAlignment = Alignment.Center;
            _errorLabel.VerticalTextAlignment = Alignment.Center;

            args.Handled = false;
            return;
        }
        var row = _dt.NewRow();
        row["Name"] = addedHeaderName;
        row["Value"] = addedValue;
        _dt.Rows.Add(row);
        _dt.AcceptChanges();

        _viewModel.Headers.Add(addedHeaderName, new RequestHeader(addedHeaderName, addedValue, true));

        args.Handled = true;
    }
}
