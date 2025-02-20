using Surfree.Host;
using Surfree.Host.Views;

using Terminal.Gui;

Application.Init();

var win = new MainWindow();
win.BorderStyle = LineStyle.Rounded;

var collections = new CollectionsView();
collections.X = 0;
collections.Y = 0;
collections.Width = Dim.Percent(15);
collections.Height = Dim.Fill(1);

var details = new DetailsFrame();
details.X = Pos.Right(collections);
details.Y = 0;
details.Width = Dim.Percent(85);
details.Height = Dim.Fill(1);


var statusBar = new StatusBar([
    new Shortcut(Key.Q.WithCtrl, "^Q Quit", () => Application.RequestStop()),
    new Shortcut(Key.Tab, "TAB Focus", TabHandler)
])
{
    X = 0,
    Y = Pos.Bottom(collections),
    Height = 1,
    Width = Dim.Fill()
};

win.Add(collections, details, statusBar);

Application.Run(win);
Application.Top?.Dispose();
Application.Shutdown();

static void TabHandler()
{
    Application.Driver.SetCursorVisibility(CursorVisibility.Invisible);
    Application.Top.AdvanceFocus(NavigationDirection.Forward, TabBehavior.NoStop);
}