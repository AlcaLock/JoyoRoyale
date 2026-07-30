using Crucero.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crucero.Application.Services.Interfaces
{
    public interface IServiceUsuarios
    {
        Task<ICollection<UsuariosDTO>> ListAsync();
        Task<UsuariosDTO> FindByIdAsync(int id);
        Task<UsuariosDTO> LoginAsync(string id, string contrasenna);
        Task<string> AddAsync(UsuariosDTO dto);
        Task DeleteAsync(int id);
        Task UpdateAsync(int id, UsuariosDTO dto);
    }
}
