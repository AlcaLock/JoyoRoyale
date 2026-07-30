using Crucero.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Crucero.Application.Services
{
    public interface IServiceCruceros
    {
        Task<int> AddAsync(CrucerosDTO dto);
        Task DeleteAsync(int id);
        Task<ICollection<CrucerosDTO>> FindByNameAsync(string nombre);
        Task<CrucerosDTO> FindByIdAsync(int id);
        Task<ICollection<CrucerosDTO>> ListAsync();
        Task UpdateAsync(int id, CrucerosDTO dto);
        Task<ICollection<CrucerosDTO>> GetCrucerosByBarco(int idBarco);
    }
}