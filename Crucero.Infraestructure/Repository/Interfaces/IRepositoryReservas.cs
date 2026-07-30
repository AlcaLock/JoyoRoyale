using JoyoRoyale.Infraestructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JoyoRoyale.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryReservas
    {
        Task<ICollection<Reservas>> ListAsync();
        Task<Reservas> FindByIdAsync(int id);
        Task<ICollection<Reservas>> ListByUserIdAsync(int usuarioId);
        Task<int> AddAsync(Reservas entity);
        Task UpdateAsync(Reservas entity);
        Task UpdateTotalAsync(int reservaId, decimal monto);
        Task DeleteAsync(int id);
        Task<ICollection<Reservas>> ListWithIncludesAsync();
        Task<ICollection<Reservas>> FindByUsuarioIdAsync(int usuarioId);
        Task<ICollection<Reservas>> FindByCruceroIdAsync(int cruceroId);
        Task<ICollection<Reservas>> GetReservasByCrucero(int cruceroId);
    }
}
