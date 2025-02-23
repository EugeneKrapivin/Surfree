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

    services.AddScoped<MainWindow>();
    services.AddScoped<CollectionsFrame>();
    
    /* request views & view-models */
    services.AddScoped<DetailsFrame>();
    services.AddScoped<RequestFrame>();
    services.AddScoped<RequestUrlFrame>();

    services.AddScoped<RequestViewModel>();

    /* response views & view-models */
    services.AddScoped<ResponseFrame>();
    services.AddScoped<ResponseViewModel>();

    return services.BuildServiceProvider();
}
