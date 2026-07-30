using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crucero.Application.DTOs
{
    public record ReservasDTO
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }

        public int CruceroId { get; set; }

        public decimal Total { get; set; }

        public virtual CrucerosDTO Crucero { get; set; } = null!;

        public virtual ICollection<HuespedesDTO> Huespedes { get; set; } = new List<HuespedesDTO>();

        public virtual ICollection<ReservasComplementosDTO> ReservasComplementos { get; set; } = new List<ReservasComplementosDTO>();

        public virtual ICollection<ReservasHabitacionesDTO> ReservasHabitaciones { get; set; } = new List<ReservasHabitacionesDTO>();

        public virtual UsuariosDTO Usuario { get; set; } = null!;
    }

}
