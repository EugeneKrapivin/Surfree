using Mediator;

using Surfree.Host.Messages;
using Surfree.Host.ViewModels;

using System.Collections.ObjectModel;

using Terminal.Gui;

namespace Surfree.Host.Views.RequestViews;

public class RequestUrlFrame : FrameView
{
    private readonly IMediator _mediator;
    private ComboBox _methodComboBox;
    private Label _urlLabel;
    private TextField _urlText;
    private Label _validRune;
    private Button _sendButton;
    private Label _methodLabel;

    public RequestUrlFrame(RequestViewModel viewModel, IMediator mediator)
    {
        Width = Dim.Fill();
        Height = 4;

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
            HideDropdownListOnClick = true,
        };

        _methodComboBox.SetSource(new ObservableCollection<string>(["GET", "POST", "PUT", "DELETE", "HEAD", "OPTIONS", "PATCH"]));
        _methodComboBox.SelectedItem = 0;
        _methodComboBox.SelectedItemChanged += (s, args) =>
        {
            viewModel.Method = HttpMethod.Parse(args.Value.ToString());
        };
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
            Text = "https://",
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
        _sendButton.Accept += async (s, e) =>
        {
            await mediator.Send(new SendRequestCommand(viewModel)).ConfigureAwait(false);
        };
        Add(_sendButton);

        _urlText.TextChanging += urlTextChanged;

        ViewModel = viewModel;
        _mediator = mediator;
    }

    public RequestViewModel ViewModel { get; }

    private void urlTextChanged(object? sender, EventArgs e)
    {
        if (e is not CancelEventArgs<string> cancelEvent) return;

        if (Uri.TryCreate(cancelEvent.NewValue, new UriCreationOptions { }, out var url))
        {
            _validRune.Text = "✔";
            _validRune.ColorScheme = new ColorScheme() { Normal = new Terminal.Gui.Attribute(Color.BrightGreen, ColorName.Blue) };
            _validRune.SetNeedsDisplay();
            _validRune.Visible = true;
            _sendButton.Enabled = true;

            ViewModel.Url = url;
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
}