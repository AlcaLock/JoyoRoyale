using JoyoRoyale.Infraestructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoyoRoyale.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryItinerarios
    {

        Task<int> AddAsync(Itinerarios entity);


        Task UpdateAsync(Itinerarios entity);

        Task AddRangeAsync(List<Itinerarios> entidades);
        Task DeleteAsync(int id);

        Task<Puertos> GetPuertoSalidaByCruceroIdAsync(int cruceroId);
        Task<Puertos> GetPuertoRegresoByCruceroIdAsync(int cruceroId);


        Task<Itinerarios> FindByIdAsync(int id);


        Task<ICollection<Itinerarios>> ListAsync();


    }
}
