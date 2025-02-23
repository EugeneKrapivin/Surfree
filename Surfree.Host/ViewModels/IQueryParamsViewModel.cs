namespace Surfree.Host.ViewModels;

public interface IQueryParamsViewModel
{
    Dictionary<string, List<string>> Query { get; }
}
