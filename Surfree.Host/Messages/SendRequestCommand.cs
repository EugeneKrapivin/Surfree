using Mediator;

using Surfree.Host.ViewModels;
using Surfree.Host.Views.ResponseViews;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surfree.Host.Messages;

public class SendRequestCommand : ICommand<Unit>
{
    public HttpMethod Method { get; }
    public Uri Url { get; }
    public string Body { get; }
    public string ContentType { get;}
    public Dictionary<string, RequestHeader> Headers { get; }
    public Dictionary<string, List<string>> Query { get; }

    public SendRequestCommand(RequestViewModel viewModel)
    {
        Method = viewModel.Method;
        Url = viewModel.Url;
        Body = viewModel.Body;
        ContentType = viewModel.ContentType;
        Headers = viewModel.Headers;
        Query = viewModel.Query;
        ViewModel = viewModel;
    }

    public RequestViewModel ViewModel { get; }
}

public class ResponseMessage : IRequest
{
    public ResponseMessage(HttpResponseMessage response)
    {
        Response = response;
    }

    public HttpResponseMessage Response { get; }
}
