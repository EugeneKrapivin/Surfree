namespace Surfree.Host.ViewModels;

public interface IHeadersViewModel
{
    Dictionary<string, RequestHeader> Headers { get; }
}
