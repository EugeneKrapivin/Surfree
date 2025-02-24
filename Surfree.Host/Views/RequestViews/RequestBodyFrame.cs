using Surfree.Host.ViewModels;

using System.Collections.ObjectModel;
using System.Data;

using Terminal.Gui;

namespace Surfree.Host.Views.RequestViews;

public sealed class RequestBodyFrame : FrameView
{
    private Label _contentTypeLabel;
    private ComboBox _contentTypeCombo;
    private TextView _editor;

    public RequestBodyFrame(RequestViewModel viewModel, ThemeConfig themeConfig)
    {
        ViewModel = viewModel;

        _theme = themeConfig.Theme;

        ColorScheme = new ColorScheme
        {
            Normal = new Terminal.Gui.Attribute(Color.Parse(_theme.Secondary), Color.Parse(_theme.Background)),
            Focus = new Terminal.Gui.Attribute(Color.Parse(_theme.Primary), Color.Parse(_theme.Background))
        };

        themeConfig.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(ThemeConfig.Theme))
            {
                _theme = themeConfig.Theme;
                ColorScheme = new ColorScheme
                {
                    Normal = new Terminal.Gui.Attribute(Color.Parse(_theme.Secondary), Color.Parse(_theme.Background)),
                    Focus = new Terminal.Gui.Attribute(Color.Parse(_theme.Primary), Color.Parse(_theme.Background))
                };
            }
        };

        InitComponent();

        _editor.ColorScheme = (_editor.ColorScheme ?? ColorScheme) with
        {
            Normal = new Terminal.Gui.Attribute(Color.Parse(_theme.Primary), Color.Parse(_theme.Background))
        };

        viewModel.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(RequestViewModel.ContentType))
            {
                // todo add syntax highlighting here
            }

            if (args.PropertyName == nameof(RequestViewModel.Method))
            {
                if (viewModel.Method == HttpMethod.Get 
                    || viewModel.Method == HttpMethod.Delete
                    || viewModel.Method == HttpMethod.Head)
                {
                    _editor.ReadOnly = true;
                    _editor.ColorScheme = _editor.ColorScheme with
                    {
                        Normal = new Terminal.Gui.Attribute(Color.Parse(_theme.Secondary), Color.Parse(_theme.Panel))
                    };
                }
                else
                {
                    _editor.ReadOnly = false;
                    _editor.ColorScheme = _editor.ColorScheme with
                    {
                        Normal = new Terminal.Gui.Attribute(Color.Parse(_theme.Primary), Color.Parse(_theme.Panel))
                    };
                }
            }
        };

        _contentTypeCombo.SelectedItemChanged += (sender, e) =>
        {
            _editor.TextChanged += (s, args) => viewModel.Body = _editor.Text;
            viewModel.ContentType = _contentTypeCombo.Text;
        };
    }

    private void InitComponent()
    {
        Title = "Body";
        BorderStyle = LineStyle.None;
        Height = Dim.Fill();
        Width = Dim.Fill(1);
        X = 1;

        _contentTypeLabel = new Label()
        {
            X = 1,
            Y = 0,
            Width = Dim.Auto(),
            Height = 1,
            Text = "Content-Type:"
        };

        _contentTypeCombo = new ComboBox()
        {
            X = Pos.Right(_contentTypeLabel) + 1,
            Y = 0,
            Width = 40,
            Height = 4,
            ReadOnly = true,
            CanFocus = false,
            HideDropdownListOnClick = true,
            ColorScheme = new()
            {
                Normal = new Terminal.Gui.Attribute(Color.Parse(_theme.Primary), Color.Parse(_theme.Surface))
            },
            Enabled = ViewModel.Method == HttpMethod.Get || ViewModel.Method == HttpMethod.Delete || ViewModel.Method == HttpMethod.Head
        };
        var source = new ObservableCollection<string>
        {
            "application/json",
            "application/x-www-form-urlencoded",
            "application/xml"
        };
        _contentTypeCombo.SetSource(source);
        _contentTypeCombo.SelectedItem = 0;

        _editor = new TextView
        {
            Text = "so lonely in here...",
            Width = Dim.Fill(1),
            Height = Dim.Fill(1),
            X = 1,
            Y = Pos.Bottom(_contentTypeCombo),
            BorderStyle = LineStyle.RoundedDotted,
            ReadOnly = ViewModel.Method == HttpMethod.Get || ViewModel.Method == HttpMethod.Delete || ViewModel.Method == HttpMethod.Head
        };

        Add(_editor, _contentTypeLabel, _contentTypeCombo);
    }

    public RequestViewModel ViewModel { get; }

    private Theme _theme;
}
