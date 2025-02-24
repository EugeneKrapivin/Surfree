using CommunityToolkit.Mvvm.ComponentModel;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surfree.Host.ViewModels;

public partial class RequestViewModel : ObservableObject
{
    [ObservableProperty]
    public partial HttpMethod Method { get; set; } = HttpMethod.Get;

    [ObservableProperty]
    public partial Uri? Url { get; set; }

    [ObservableProperty]
    public partial string Body { get; set; } = string.Empty;

    [ObservableProperty]
    public partial Dictionary<string, List<string>> Query { get; set; } = [];

    [ObservableProperty]
    public partial Dictionary<string, RequestHeader> Headers { get; set; } = [];

    [ObservableProperty]
    public partial string ContentType { get; set; }
}