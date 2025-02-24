using Surfree.Host.ViewModels;
using Surfree.Host.Views.RequestViews;
using Surfree.Host.Views.ResponseViews;

using Terminal.Gui;

namespace Surfree.Host.Views;

public class DetailsFrame : FrameView
{
    public DetailsFrame(RequestFrame req, ResponseFrame res, ThemeConfig themeConfig)
    {
        Id = "DetailsFrameView";
        Width = Dim.Fill();
        Height = Dim.Fill();
        BorderStyle = LineStyle.None;


        req.X = 1;
        req.Y = 0;
        req.Width = Dim.Fill();
        req.Height = Dim.Percent(50);

        res.X = 1;
        res.Y = Pos.Bottom(req);
        res.Width = Dim.Fill();
        res.Height = Dim.Percent(50);

        Add(req, res);

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
    }
}
