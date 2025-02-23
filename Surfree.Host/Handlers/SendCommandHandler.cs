using Mediator;

using Surfree.Host.Messages;

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
        private readonly IMediator _mediator;

        public SendCommandHandler(IHttpClientFactory httpClientFactory, IMediator mediator)
        {
            _httpClientFactory = httpClientFactory;
            _mediator = mediator;
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
            var response = await client.SendAsync(request, cancellationToken);

            await _mediator.Publish(new ResponseMessage(response), cancellationToken);

            return Unit.Value;
        }
    }
}
