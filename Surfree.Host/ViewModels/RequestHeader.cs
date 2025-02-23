namespace Surfree.Host.ViewModels;

public record class RequestHeader(string HeaderName, string HeaderValue, bool Enabled = true);
