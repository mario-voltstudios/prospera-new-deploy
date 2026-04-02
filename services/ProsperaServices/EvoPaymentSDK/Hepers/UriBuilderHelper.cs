using System.Web;

namespace EvoPayment.Hepers;

public class UriBuilderHelper
{
    public readonly UriBuilder _builder;
    public UriBuilderHelper (string uri)
    {
        _builder = new UriBuilder("_default/"+uri);
    }

    public UriBuilderHelper AddQueryParameter(string? name, string? value)
    {
        var query = HttpUtility.ParseQueryString(_builder.Query);
        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
        {
            query[name] = value;
            _builder.Query = query.ToString();
        }
        return this;
    }

    public override string ToString()
    {
        return _builder.ToString().Replace("http://_default:80/", string.Empty);
    }
}