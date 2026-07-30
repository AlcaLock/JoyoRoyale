using JoyoRoyale.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoyoRoyale.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryUsuarios
    {
        Task<ICollection<Usuarios>> ListAsync();
        Task<Usuarios> FindByIdAsync(int id);
        Task<Usuarios> LoginAsync(string id, string password);
        Task<string> AddAsync(Usuarios entity);
        Task DeleteAsync(int id);
        Task UpdateAsync();
    }
}
