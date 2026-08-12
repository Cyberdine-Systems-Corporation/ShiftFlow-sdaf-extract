using System.Net.Http.Headers;

namespace ShiftFlow.Web.Auth;

/// <summary>
/// Adjunta Bearer (preferido) y cookies de sesión Api en cada request;
/// captura <c>Set-Cookie</c> de las respuestas.
/// Debe usarse con <c>UseCookies = false</c> en el handler primario.
/// </summary>
public sealed class PropagateAllCookiesHandler(CookieContainerHolder holder) : DelegatingHandler
{
    /// <inheritdoc />
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(holder.AccessToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", holder.AccessToken);
        }

        string? cookieHeader = holder.CookieHeader;
        if (!string.IsNullOrWhiteSpace(cookieHeader))
        {
            request.Headers.Remove("Cookie");
            request.Headers.TryAddWithoutValidation("Cookie", cookieHeader);
        }

        HttpResponseMessage? response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

        List<string>? setCookies = ReadSetCookieHeaders(response.Headers);
        if (setCookies.Count > 0)
        {
            holder.AbsorbSetCookieHeaders(
                setCookies,
                response.RequestMessage?.RequestUri ?? request.RequestUri);
        }

        return response;
    }

    private static List<string> ReadSetCookieHeaders(HttpResponseHeaders headers)
    {
        List<string> list = new List<string>();

        if (headers.TryGetValues("Set-Cookie", out IEnumerable<string>? validated))
        {
            list.AddRange(validated);
        }

        if (headers.NonValidated.TryGetValues("Set-Cookie", out HeaderStringValues raw))
        {
            foreach (string value in raw)
            {
                if (!list.Contains(value, StringComparer.Ordinal))
                {
                    list.Add(value);
                }
            }
        }

        return list;
    }
}
