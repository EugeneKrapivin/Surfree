using Pastel;

using System.Collections.ObjectModel;
using Terminal.Gui;

namespace Surfree.Host.Views;

internal class RequestUrlFrame : FrameView
{
    private ComboBox _methodComboBox;
    private Label _urlLabel;
    private TextField _urlText;
    private Label _validRune;
    private Button _sendButton;
    private Label _methodLabel;

    public RequestUrlFrame()
    {
        Width = Dim.Fill();
        Height = 4;
        CanFocus = false;

        _methodLabel = new Label()
        {
            X = 1,
            Y = 1,
            Text = "Method:"
        };
        Add(_methodLabel);
        
        _methodComboBox = new ComboBox()
        {
            X = Pos.Right(_methodLabel) + 1,
            Y = Pos.Top(_methodLabel),
            Width = 10,
            Height = 4,
            CanFocus = true,
            HideDropdownListOnClick = true,
        };

        _methodComboBox.SetSource(new ObservableCollection<string>(["GET", "POST", "PUT", "DELETE", "HEAD", "OPTIONS", "PATCH"]));
        _methodComboBox.SelectedItem = 0;
        Add(_methodComboBox);

        _urlLabel = new Label()
        {
            X = Pos.Right(_methodComboBox) + 3,
            Y = Pos.Top(_methodComboBox),
            Text = "URL:"
        };
        Add(_urlLabel);

        _urlText = new TextField()
        {
            X = Pos.Right(_urlLabel) + 1,
            Y = Pos.Top(_urlLabel),
            Width = 40,
        };
        Add(_urlText);

        _validRune = new Label()
        {
            Visible = false,
            CanFocus = false,
            X = Pos.Right(_urlText) + 1,
            Y = Pos.Top(_urlText),
            Height = 1,
            Width = 1,
        };
        Add(_validRune);
        _sendButton = new Button()
        {
            X = Pos.Right(_validRune) + 2,
            Y = Pos.Top(_validRune),
            Text = "Send",
            Enabled = Uri.IsWellFormedUriString(_urlText.Text, UriKind.Absolute)
        };
        Add(_sendButton);

        _urlText.TextChanging += urlTextChanged;
    }

    private void urlTextChanged(object? sender, EventArgs e)
    {
        var cancelEvent = e as CancelEventArgs<string>;
        if (cancelEvent is null) return;

        if (Uri.TryCreate(cancelEvent.NewValue, new UriCreationOptions { }, out var _))
        {
            _validRune.Text = "✔";
            _validRune.ColorScheme = new ColorScheme() { Normal = new Terminal.Gui.Attribute(Color.BrightGreen, ColorName.Blue) };
            _validRune.SetNeedsDisplay();
            _validRune.Visible = true;
            _sendButton.Enabled = true;
        }
        else
        {
            _validRune.Text = "✘";
            _validRune.ColorScheme = new ColorScheme() { Normal = new Terminal.Gui.Attribute(Color.Red, ColorName.Blue) };
            _validRune.SetNeedsDisplay();
            _validRune.Visible = true;
            _sendButton.Enabled = false;
        }
    }

    public RequestUrlModel GetUrlModel()
    {
        return new RequestUrlModel()
        {
            Uri = new Uri(_urlText.Text),
            Method = new HttpMethod(_methodComboBox.SelectedItem.ToString())
        };
    }
}

public class RequestUrlModel
{
    public Uri Uri { get; set; }
    public HttpMethod Method { get; set; }
}