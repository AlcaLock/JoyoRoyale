using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crucero.Application.DTOs
{
    public record ReservasHabitacionesDTO
    {
        public int Id { get; set; }

        public int ReservaId { get; set; }

        public int HabitacionId { get; set; }

        public int CantidadPasajeros { get; set; }

        public virtual HabitacionesDTO Habitacion { get; set; } = null!;
        
        public virtual ReservasDTO Reserva { get; set; } = null!;
    }
}
