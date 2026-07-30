using Crucero.Application.DTOs;
using Crucero.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Crucero.Application.Services.Implementations
{
    public class ServicePaises : IServicePaises
    {
        public async Task<List<PaisDTO>> GetAllPaisesAsync()
        {
            // Simula la lista de países (puedes obtenerla desde una base de datos si lo prefieres)
            var paises = new List<PaisDTO>
            {
                new PaisDTO { Id = 1, Nombre = "Argentina" },
new PaisDTO { Id = 2, Nombre = "Belice" },
new PaisDTO { Id = 3, Nombre = "Costa Rica" },
new PaisDTO { Id = 4, Nombre = "El Salvador" },
new PaisDTO { Id = 5, Nombre = "Guatemala" },
new PaisDTO { Id = 6, Nombre = "Honduras" },
new PaisDTO { Id = 7, Nombre = "Nicaragua" },
new PaisDTO { Id = 8, Nombre = "Panamá" },
new PaisDTO { Id = 9, Nombre = "República Dominicana" }

            };

            // Mapeo de la lista de Países a DTOs
            var paisesDTO = paises.Select(p => new PaisDTO
            {
                Id = p.Id,
                Nombre = p.Nombre
            }).ToList();

            // Simula un retraso en la tarea (esto se puede quitar si estás usando una base de datos)
            await Task.Delay(100);

            return paisesDTO;
        }
    }
}
