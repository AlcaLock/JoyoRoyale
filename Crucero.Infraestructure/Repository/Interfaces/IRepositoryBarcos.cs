using JoyoRoyale.Infraestructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JoyoRoyale.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryBarcos
    {
        Task<ICollection<Barcos>> ListAsync();
        Task<Barcos> FindByIdAsync(int id);
        Task<int> AddAsync(Barcos entity, List<(int HabitacionId, int CantidadDisponible)> habitaciones);
        Task<Barcos> GetByIdWithHabitacionesAsync(int id);
        Task UpdateAsync(Barcos entity, List<(int HabitacionId, int CantidadDisponible)> habitaciones);
        Task DeleteAsync(int id);
        Task<ICollection<Barcos>> FindByNameAsync(string nombre);
        Task<ICollection<Barcos>> GetBarcosByCrucero(int idCrucero);
         Task<List<BarcoHabitaciones>> GetHabitacionesPorBarcoIdAsync(int barcoId);
    }
}
