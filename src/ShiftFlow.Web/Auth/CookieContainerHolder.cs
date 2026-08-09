using System.Collections.Concurrent;
using System.Net;

namespace ShiftFlow.Web.Auth;

/// <summary>
/// Sesión Api del host Web: token Bearer, claims locales (para el pipeline HTTP) y jar de cookies.
/// Singleton MVP: un solo operador demo por proceso Web.
/// </summary>
public sealed class CookieContainerHolder
{
    private readonly ConcurrentDictionary<string, string> _values = new(StringComparer.OrdinalIgnoreCase);
    private string? _accessToken;
    private string? _userName;
    private string[] _roles = [];

    /// <summary>
    /// Contenedor clásico (compatibilidad); la fuente de verdad para cookies es <see cref="CookieHeader"/>.
    /// </summary>
    public CookieContainer Container { get; } = new();

    /// <summary>Número de cookies almacenadas en el jar explícito.</summary>
    public int Count => _values.Count;

    /// <summary>Token Bearer emitido por la Api en el login, o <see langword="null"/>.</summary>
    public string? AccessToken => _accessToken;

    /// <summary>Usuario de la sesión BFF (para <c>HttpContext</c> / PassThrough).</summary>
    public string? UserName => _userName;

    /// <summary>Roles de la sesión BFF.</summary>
    public IReadOnlyList<string> Roles => _roles;

    /// <summary>Indica si hay sesión usable en el host Web.</summary>
    public bool HasWebSession =>
        !string.IsNullOrWhiteSpace(_userName) &&
        (!string.IsNullOrWhiteSpace(_accessToken) || !_values.IsEmpty);

    /// <summary>Cabecera <c>Cookie</c> lista para enviar, o <see langword="null"/> si está vacío.</summary>
    public string? CookieHeader
    {
        get
        {
            if (_values.IsEmpty)
            {
                return null;
            }

            return string.Join("; ", _values.Select(static kv => $"{kv.Key}={kv.Value}"));
        }
    }

    /// <summary>
    /// Establece la sesión BFF (token + identidad) tras un login correcto.
    /// </summary>
    public void SetSession(string userName, IReadOnlyList<string> roles, string? accessToken)
    {
        _userName = userName;
        _roles = roles.ToArray();
        SetAccessToken(accessToken);
    }

    /// <summary>Guarda el token Bearer de la Api.</summary>
    public void SetAccessToken(string? token) =>
        _accessToken = string.IsNullOrWhiteSpace(token) ? null : token.Trim();

    /// <summary>
    /// Incorpora pares <c>name=value</c> (p. ej. del body de login o del primer segmento de Set-Cookie).
    /// </summary>
    /// <param name="nameValuePairs">Pares sin atributos de cookie.</param>
    public void AbsorbNameValuePairs(IEnumerable<string> nameValuePairs)
    {
        foreach (var pair in nameValuePairs)
        {
            StorePair(pair.Split(';', 2)[0]);
        }
    }

    /// <summary>
    /// Incorpora cabeceras <c>Set-Cookie</c> de una respuesta de la Api.
    /// </summary>
    /// <param name="setCookieHeaders">Valores crudos de Set-Cookie.</param>
    /// <param name="requestUri">URI de la petición (para el <see cref="CookieContainer"/> auxiliar).</param>
    public void AbsorbSetCookieHeaders(IEnumerable<string> setCookieHeaders, Uri? requestUri)
    {
        foreach (var header in setCookieHeaders)
        {
            StorePair(header.Split(';', 2)[0]);

            if (requestUri is not null &&
                requestUri.IsAbsoluteUri &&
                (requestUri.Scheme == Uri.UriSchemeHttp || requestUri.Scheme == Uri.UriSchemeHttps))
            {
                try
                {
                    Container.SetCookies(requestUri, header);
                }
                catch (CookieException)
                {
                    // Host/path raros: el diccionario ya guarda la cookie.
                }
            }
        }
    }

    /// <summary>Vacía token, claims y jar (p. ej. tras logout).</summary>
    public void Clear()
    {
        _accessToken = null;
        _userName = null;
        _roles = [];
        _values.Clear();
        Container.GetAllCookies().ToList().ForEach(c =>
        {
            c.Expired = true;
        });
    }

    private void StorePair(string nameValue)
    {
        var eq = nameValue.IndexOf('=');
        if (eq <= 0)
        {
            return;
        }

        var name = nameValue[..eq].Trim();
        var value = nameValue[(eq + 1)..].Trim();
        if (name.Length == 0)
        {
            return;
        }

        _values[name] = value;
    }
}
