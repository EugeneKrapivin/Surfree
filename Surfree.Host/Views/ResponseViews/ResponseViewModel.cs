using Surfree.Host.ViewModels;

namespace Surfree.Host.Views.ResponseViews;

public class ResponseViewModel : IHeadersViewModel
{
    public Dictionary<string, RequestHeader> Headers { get; set; } = [];
    public string Body { get; set; }
    public Dictionary<string, string> Cookies { get; set; } = [];
    public Dictionary<string, string> Info { get; set; } = [];
}
