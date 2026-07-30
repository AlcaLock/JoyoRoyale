using JoyoRoyale.Infraestructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoyoRoyale.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryPreciosHabitaciones
    {
        Task<int> AddAsync(PreciosHabitaciones entity);


        Task AddRangeAsync(List<PreciosHabitaciones> entidades);
        Task UpdateAsync(PreciosHabitaciones entity);


        Task DeleteAsync(int id);
        Task<decimal> GetPrecioByHabitacionAndFechaAsync(int habitacionId, int fechaCruceroId);

        Task<PreciosHabitaciones> FindByIdAsync(int id);


        Task<ICollection<PreciosHabitaciones>> ListAsync();





    }
}
