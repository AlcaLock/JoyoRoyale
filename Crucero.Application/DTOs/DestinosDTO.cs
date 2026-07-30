using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crucero.Application.DTOs
{
    public record DestinosDTO
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public virtual ICollection<PuertosDTO> Puertos { get; set; } = new List<PuertosDTO>();
    }

}
