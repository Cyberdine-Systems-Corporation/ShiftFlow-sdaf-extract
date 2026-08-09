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

        var cookieHeader = holder.CookieHeader;
        if (!string.IsNullOrWhiteSpace(cookieHeader))
        {
            request.Headers.Remove("Cookie");
            request.Headers.TryAddWithoutValidation("Cookie", cookieHeader);
        }

        var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

        var setCookies = ReadSetCookieHeaders(response.Headers);
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
        var list = new List<string>();

        if (headers.TryGetValues("Set-Cookie", out var validated))
        {
            list.AddRange(validated);
        }

        if (headers.NonValidated.TryGetValues("Set-Cookie", out var raw))
        {
            foreach (var value in raw)
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
