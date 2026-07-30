// Ruta: Crucero.Web/Services/TipoCambioHostedService.cs
using Crucero.Application.Services.Interfaces;

public class TipoCambioHostedService : IHostedService
{
    private readonly ITipoCambioService _tipoCambioService;

    public TipoCambioHostedService(ITipoCambioService tipoCambioService)
    {
        _tipoCambioService = tipoCambioService;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var tipoCambio = await _tipoCambioService.ObtenerYGuardarTipoCambioAsync();
        Console.WriteLine($"Tipo de cambio al iniciar: {tipoCambio?.Valor}");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
