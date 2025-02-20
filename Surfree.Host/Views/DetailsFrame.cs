using Terminal.Gui;

namespace Surfree.Host.Views;

internal class DetailsFrame : FrameView
{
    public DetailsFrame()
    {
        Width = Dim.Fill();
        Height = Dim.Fill();
        BorderStyle = LineStyle.None;
        CanFocus = false;

        var req = new RequestFrame();
        var res = new ResponseFrame();
        
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
