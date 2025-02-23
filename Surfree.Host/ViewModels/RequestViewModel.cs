using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surfree.Host.ViewModels;

public class RequestViewModel : IHeadersViewModel, IBodyViewModel, IQueryParamsViewModel
{
    public HttpMethod Method { get; set; } = HttpMethod.Get;

    public Uri? Url { get; set; }

    public string Body { get; set; } = string.Empty;

    public Dictionary<string, List<string>> Query { get; set; } = [];

    public Dictionary<string, RequestHeader> Headers { get; set; } = [];

    public string ContentType { get; set; }
}