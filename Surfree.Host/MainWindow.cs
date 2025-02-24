using Surfree.Host.Views;

using Terminal.Gui;

namespace Surfree.Host;

public class MainWindow : Window
{
    private readonly CollectionsFrame _collections;
    private readonly DetailsFrame _details;

    public MainWindow(CollectionsFrame collections, DetailsFrame details, ThemeConfig themeConfig)
    {
        _collections = collections;
        _details = details;

        Id = "MainWindow";
        X = 0;
        Y = 0;
        Width = Dim.Fill(0);
        Height = Dim.Fill(0);
        Visible = true;
        Title = "Surfree";
        CanFocus = true;
        Enabled = true;
        BorderStyle = LineStyle.Rounded;

        var theme = themeConfig.Theme;

        ColorScheme = new ColorScheme 
        {
            Normal = new Terminal.Gui.Attribute(Color.Parse(theme.Secondary), Color.Parse(theme.Background)),
            Focus = new Terminal.Gui.Attribute(Color.Parse(theme.Primary), Color.Parse(theme.Background))
        };

        themeConfig.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(ThemeConfig.Theme))
            {
                theme = themeConfig.Theme;
                ColorScheme = new ColorScheme
                {
                    Normal = new Terminal.Gui.Attribute(Color.Parse(theme.Secondary), Color.Parse(theme.Background)),
                    Focus = new Terminal.Gui.Attribute(Color.Parse(theme.Primary), Color.Parse(theme.Background))
                };
            }
        };

        collections.X = 0;
        collections.Y = 0;
        collections.Width = Dim.Percent(15);
        collections.Height = Dim.Fill(1);

        details.X = Pos.Right(collections);
        details.Y = 0;
        details.Width = Dim.Percent(85);
        details.Height = Dim.Fill(1);

        var statusBar = new StatusBar([new Shortcut(Key.Q.WithCtrl, "^Q|C Quit", () => Application.RequestStop())])
        {
            X = 0,
            Y = Pos.Bottom(collections),
            Height = 1,
            Width = Dim.Fill()
        };

        Add(collections, details, statusBar);
    }
}