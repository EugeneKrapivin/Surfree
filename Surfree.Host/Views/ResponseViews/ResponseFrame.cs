using Mediator;

using Surfree.Host.Messages;
using Surfree.Host.Views.RequestViews;

using System.Net;
using System.Reflection;

using Terminal.Gui;

namespace Surfree.Host.Views.ResponseViews;

public class ResponseFrame : FrameView
{
    private Tab _bodyTab;
    private Tab _cookiesTab;
    private Tab _infoTab;
    private TabView _tabView;
    private Tab _headersTab;

    public ResponseViewModel ViewModel { get; private set; }

    public ResponseFrame(ResponseViewModel viewModel)
    {
        Title = "Response";
        BorderStyle = LineStyle.Rounded;

        Height = Dim.Fill();
        Width = Dim.Fill();

        CanFocus = false;

        _tabView = new TabView()
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill(),
            BorderStyle = LineStyle.None
        };

        _headersTab = new Tab()
        {
            DisplayText = "Headers",
            Width = Dim.Fill(),
            Height = Dim.Fill(),
            View = new ResponseHeadersFrame(viewModel)
        };

        _bodyTab = new Tab()
        {
            DisplayText = "Body",
            Width = Dim.Fill(),
            Height = Dim.Fill(),
            View = new ResponseBodyFrame(viewModel)
        };

        _cookiesTab = new Tab()
        {
            DisplayText = "Cookies",
            Width = Dim.Fill(),
            Height = Dim.Fill(),
            View = new ResponseCookiesFrame(viewModel)
        };
        _infoTab = new Tab()
        {
            DisplayText = "Info",
            Width = Dim.Fill(),
            Height = Dim.Fill(),
            View = new ResponseInfoFrame(viewModel)
        };

        _tabView.AddTab(_infoTab, true);
        _tabView.AddTab(_headersTab, false);
        _tabView.AddTab(_bodyTab, false);
        _tabView.AddTab(_cookiesTab, false);
        
        Add(_tabView);
        ViewModel = viewModel;
    }
}
