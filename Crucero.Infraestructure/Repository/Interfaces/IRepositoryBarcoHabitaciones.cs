
using JoyoRoyale.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoyoRoyale.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryBarcoHabitaciones
    {
        Task AddRangeAsync(List<BarcoHabitaciones> entidades);
        Task<ICollection<BarcoHabitaciones>> ListAsync();
        Task<BarcoHabitaciones> FindByIdAsync(int id);
        Task<int> AddAsync(BarcoHabitaciones entity);
        Task UpdateAsync(BarcoHabitaciones entity);
        Task DeleteAsync(int id);
        Task DeleteByBarcoIdAsync(int barcoId);
        Task<ICollection<BarcoHabitaciones>> ListByBarcoIdAsync(int barcoId);

        Task<ICollection<BarcoHabitaciones>> FindByBarcoIdAsync(int barcoId);
        Task<ICollection<BarcoHabitaciones>> FindByHabitacionIdAsync(int habitacionId);
    }
}
