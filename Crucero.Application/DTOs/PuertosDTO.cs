using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crucero.Application.DTOs
{
    public record PuertosDTO
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public string Pais { get; set; } = null!;

        public int DestinoId { get; set; }

        public virtual DestinosDTO Destino { get; set; } = null!;

        public virtual ICollection<ItinerariosDTO> Itinerarios { get; set; } = new List<ItinerariosDTO>();
    }
}
