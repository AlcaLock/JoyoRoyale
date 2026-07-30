
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crucero.Application.DTOs
{
    public record ComplementosDTO
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public string? Descripcion { get; set; }

        public decimal Precio { get; set; }

        public string TipoAplicacion { get; set; } = null!;

        public virtual ICollection<ReservasComplementosDTO> ReservasComplementos { get; set; } = new List<ReservasComplementosDTO>();
    }
}
