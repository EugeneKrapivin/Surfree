using Microsoft.Extensions.DependencyInjection;

using Surfree.Host;
using Surfree.Host.ViewModels;
using Surfree.Host.Views;
using Surfree.Host.Views.RequestViews;
using Surfree.Host.Views.ResponseViews;

using Terminal.Gui;

var services = ConfigureServices();

Application.Init();

Application.Run(services.GetRequiredService<MainWindow>());
Application.Top?.Dispose();
Application.Shutdown();


static IServiceProvider ConfigureServices()
{
    var services = new ServiceCollection();

    services.AddMediator();
    services.AddHttpClient();
    /* TUI bindings */

    services.AddTransient<MainWindow>();
    services.AddTransient<CollectionsFrame>();
    
    /* request views & view-models */
    services.AddTransient<DetailsFrame>();
    services.AddTransient<RequestFrame>();
    services.AddTransient<RequestUrlFrame>();

    services.AddTransient<RequestViewModel>();

    /* response views & view-models */
    services.AddTransient<ResponseFrame>();
    services.AddTransient<ResponseViewModel>();

    return services.BuildServiceProvider();
}
