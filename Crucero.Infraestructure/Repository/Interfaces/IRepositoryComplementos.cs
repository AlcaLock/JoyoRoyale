using JoyoRoyale.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoyoRoyale.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryComplementos
    {

        Task<ICollection<Complementos>> ListAsync();
        Task<Complementos> FindByIdAsync(int id);

        Task<int> AddAsync(Complementos entity);
        Task UpdateAsync(Complementos entity);
    }
}
