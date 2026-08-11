// Ruta: Crucero.Web/Services/WebTipoCambioService.cs
using Crucero.Application.Config;
using Crucero.Application.DTOs;
using Crucero.Application.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using ServiceReference1;
using System.Globalization;
using System.Xml.Linq;

public class WebTipoCambioService : ITipoCambioService
{
    private readonly IMemoryCache _cache;
    private readonly IOptions<AppConfig> _options;

    public WebTipoCambioService(IMemoryCache cache, IOptions<AppConfig> options)
    {
        _cache = cache;
        _options = options;
    }

    public async Task<TipoCambioDto?> ObtenerYGuardarTipoCambioAsync()
    {
        var client = new wsindicadoreseconomicosSoapClient(
            wsindicadoreseconomicosSoapClient.EndpointConfiguration.wsindicadoreseconomicosSoap);

        var fechaHoy = DateTime.Now.ToString("dd/MM/yyyy");
        var bccr = _options.Value.BccrSettings;

        if (string.IsNullOrWhiteSpace(bccr.Token) || string.IsNullOrWhiteSpace(bccr.Email))
        {
            return null;
        }

        var xml = await client.ObtenerIndicadoresEconomicosXMLAsync(
            "317", fechaHoy, fechaHoy, bccr.NombreApp, "N", bccr.Email, bccr.Token
        );

        var xdoc = XDocument.Parse(xml);
        var elemento = xdoc.Descendants("INGC011_CAT_INDICADORECONOMIC").FirstOrDefault();
        if (elemento == null) return null;

        var fecha = DateTime.Parse(elemento.Element("DES_FECHA")?.Value ?? "");
        var valorTexto = elemento.Element("NUM_VALOR")?.Value ?? "0";

        if (!decimal.TryParse(valorTexto, NumberStyles.Any, CultureInfo.InvariantCulture, out var valor))
            return null;

        var dto = new TipoCambioDto
        {
            Fecha = fecha,
            Valor = Math.Round(valor, 2)
        };

        _cache.Set("TipoCambioDolar", dto, TimeSpan.FromHours(1));
        return dto;
    }

   
}
