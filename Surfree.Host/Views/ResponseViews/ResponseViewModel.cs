using CommunityToolkit.Mvvm.ComponentModel;

using Surfree.Host.ViewModels;

namespace Surfree.Host.Views.ResponseViews;

public partial class ResponseViewModel : ObservableObject
{
    [ObservableProperty]
    public partial Dictionary<string, RequestHeader> Headers { get; set; } = new Dictionary<string, RequestHeader>(StringComparer.InvariantCultureIgnoreCase);

    [ObservableProperty]
    public partial string Body { get; set; }

    [ObservableProperty]
    public partial Dictionary<string, string> Cookies { get; set; } = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

    [ObservableProperty]
    public partial Dictionary<string, string> Info { get; set; } = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
}
