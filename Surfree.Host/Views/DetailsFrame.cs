using Surfree.Host.ViewModels;
using Surfree.Host.Views.RequestViews;
using Surfree.Host.Views.ResponseViews;

using Terminal.Gui;

namespace Surfree.Host.Views;

public class DetailsFrame : FrameView
{
    public DetailsFrame(RequestFrame req, ResponseFrame res)
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
    }
}
