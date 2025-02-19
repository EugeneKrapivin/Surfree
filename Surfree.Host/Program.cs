using Surfree.Host;
using Surfree.Host.Views;

using Terminal.Gui;

Application.Init();

var win = new MainWindow();

var requestUrl = new RequestUrlFrame();
var collections = new CollectionsView();
var details = new DetailsFrame();

win.Add(requestUrl,collections, details);

collections.X = 0;
collections.Y = Pos.Bottom(requestUrl);
details.X = Pos.Right(collections);
details.Y = Pos.Bottom(requestUrl);


var statusBar = new StatusBar([
    new Shortcut(Key.Q.WithCtrl, "^Q Quit", () => Application.RequestStop()),
    new Shortcut(Key.Tab, "TAB Focus", TabHandler)
]);

win.Add(statusBar);

Application.Run(win);
Application.Top?.Dispose();
Application.Shutdown();

static void TabHandler()
{
    Application.Driver.SetCursorVisibility(CursorVisibility.Invisible);
    Application.Top.AdvanceFocus(NavigationDirection.Forward, TabBehavior.NoStop);
}