using System.ComponentModel;
using System.Data;

using Terminal.Gui;

namespace Surfree.Host.Views.RequestViews;

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
