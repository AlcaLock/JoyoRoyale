using JoyoRoyale.Infraestructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoyoRoyale.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryPuertos
    {

        Task<int> AddAsync(Puertos puerto);

        Task UpdateAsync(Puertos puerto);

        Task DeleteAsync(int id);

         Task<Puertos> FindByIdAsync(int id);

        Task<ICollection<Puertos>> ListAsync();



    }
}
