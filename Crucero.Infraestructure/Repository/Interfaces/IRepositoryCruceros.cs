
using JoyoRoyale.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoyoRoyale.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryCruceros
    {
        Task<ICollection<Cruceros>> ListAsync();
        Task<Cruceros> FindByIdAsync(int id);
        Task<int> AddAsync(Cruceros entity);
        Task UpdateAsync(Cruceros entity);
        Task DeleteAsync(int id);
        Task<ICollection<Cruceros>> FindByNameAsync(string nombre);
        Task<ICollection<Cruceros>> GetCrucerosByBarco(int idBarco);
    }
}
