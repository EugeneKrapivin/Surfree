using System.ComponentModel;
using System.Data;

using Terminal.Gui;

namespace Surfree.Host.Views.RequestViews;

public sealed class AddQueryDialog : QueryDialog
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
            _textView.TextAlignment = Alignment.Center;
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