using System.ComponentModel;
using System.Data;

using Terminal.Gui;

namespace Surfree.Host.Views.RequestViews;

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
