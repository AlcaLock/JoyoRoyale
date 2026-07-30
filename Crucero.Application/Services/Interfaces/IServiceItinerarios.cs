using Crucero.Application.DTOs;
using JoyoRoyale.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crucero.Application.Services.Interfaces
{
    public interface IServiceItinerarios
    {

        Task<int> AddAsync(ItinerariosDTO dto);

        Task AddRangeAsync(List<ItinerariosDTO> dtos);
        Task UpdateAsync(int id, ItinerariosDTO dto);


        Task DeleteAsync(int id);

        Task<string> GetNombrePuertoSalidaAsync(int cruceroId);
        Task<string> GetNombrePuertoRegresoAsync(int cruceroId);

        Task<ItinerariosDTO> FindByIdAsync(int id);


        Task<ICollection<ItinerariosDTO>> ListAsync();




    }
}
