using Crucero.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crucero.Application.Services.Interfaces
{
    public interface ITipoCambioService
    {
        Task<TipoCambioDto?> ObtenerYGuardarTipoCambioAsync();
    }
}
