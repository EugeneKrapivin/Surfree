using Terminal.Gui;

namespace Surfree.Host.Views;

internal class DetailsFrame : FrameView
{
    public DetailsFrame()
    {
        Width = Dim.Fill();
        Height = Dim.Fill();
        BorderStyle = LineStyle.None;

        var req = new RequestFrame();
        var res = new ResponseFrame();
        
        req.X = 0;
        req.Y = 0;
        req.Width = Dim.Fill();
        req.Height = Dim.Percent(50);

        res.X = 0;
        res.Y = Pos.Bottom(req);
        res.Width = Dim.Fill();
        res.Height = Dim.Percent(50);


        Add(req, res);
    }
}
