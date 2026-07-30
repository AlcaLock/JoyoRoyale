using Crucero.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crucero.Application.Services.Interfaces
{
    public interface IServiceComplementos
    {

        Task<ICollection<ComplementosDTO>> ListAsync();
        Task<ComplementosDTO> FindByIdAsync(int id);
        Task<int> AddAsync(ComplementosDTO dto);
        Task UpdateAsync(int id, ComplementosDTO dto);

    }
}
