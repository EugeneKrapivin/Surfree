using Terminal.Gui;

namespace Surfree.Host.Views;

internal class ResponseFrame : FrameView
{
    public ResponseFrame()
    {
        Title = "Response";
        BorderStyle = LineStyle.Rounded;
        
        Height = Dim.Fill();
        Width = Dim.Fill();

        var tabView = new TabView()
        {
            Title = "_",
            X = 0,
            Y = 1,
            Width = Dim.Fill(),
            Height = Dim.Fill(),
            BorderStyle = LineStyle.None
        };

        var headersTab = new Tab()
        {
            DisplayText = "Headers",
            Width = Dim.Fill(),
            Height = Dim.Fill(),
        };
        headersTab.Add(new HeadersFrame());

        var bodyTab = new Tab()
        {
            DisplayText = "Body"
        };
        var cookiesTab = new Tab()
        {
            DisplayText = "Cookies"
        };
        var infoTab = new Tab()
        {
            DisplayText = "Info",
        };

        tabView.AddTab(headersTab, true);
        tabView.AddTab(bodyTab, false);
        tabView.AddTab(cookiesTab, false);
        tabView.AddTab(infoTab, false);

        Add(tabView);
    }
}
