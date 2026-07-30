using Crucero.Application.DTOs;
using JoyoRoyale.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crucero.Application.Services.Interfaces
{
    public interface IServicePuertos
    {

        Task<int> AddAsync(PuertosDTO dto);


        Task UpdateAsync(int id, PuertosDTO dto);

        Task DeleteAsync(int id);


        Task<PuertosDTO> FindByIdAsync(int id);


        Task<ICollection<PuertosDTO>> ListAsync();



    }
}
