using Mediator;

using Surfree.Host.Messages;
using Surfree.Host.ViewModels;
using Surfree.Host.Views.ResponseViews;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Surfree.Host.Handlers
{
    public class SendCommandHandler : ICommandHandler<SendRequestCommand>
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ResponseViewModel _responseViewModel;
        private readonly IMediator _mediator;

        public SendCommandHandler(IHttpClientFactory httpClientFactory, ResponseViewModel responseViewModel)
        {
            _httpClientFactory = httpClientFactory;
            _responseViewModel = responseViewModel;
        }
        public async ValueTask<Unit> Handle(SendRequestCommand command, CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(command.Method, command.Url);
            request.Content = new StringContent(command.Body, Encoding.UTF8, command.ContentType);
            foreach (var (name, value, enabled) in command.Headers.Values)
            {
                if (!enabled) continue;
                request.Headers.Add(name, value);
            }
            var query = new StringBuilder();
            foreach (var (name, values) in command.Query)
            {
                foreach (var value in values)
                {
                    query.Append($"{name}={value}&");
                }
            }
            if (query.Length > 0)
            {
                query.Remove(query.Length - 1, 1);
                request.RequestUri = new Uri($"{request.RequestUri}?{query}");
            }
            try
            {
                var response = await client.SendAsync(request, cancellationToken);

                _responseViewModel.Body = await response.Content.ReadAsStringAsync();
                _responseViewModel.Headers = response.Headers.ToDictionary(x => x.Key, x => new RequestHeader(x.Key, x.Value.FirstOrDefault(), true));
                _responseViewModel.Cookies = response.Headers.Where(x => x.Key == "Set-Cookie").SelectMany(x => x.Value).Select(x => x.Split(';')[0]).ToDictionary(x => x, x => x);
                _responseViewModel.Info = new Dictionary<string, string>
                {
                    { "Status Code", response.StatusCode.ToString() },
                    { "Reason Phrase", response.ReasonPhrase  ?? "N/A"},
                    { "Protocol Version", response.Version.ToString() }
                };

            }
            catch (Exception ex)
            {
                // todo handle exceptions
            }
            return Unit.Value;
        }
    }
}
