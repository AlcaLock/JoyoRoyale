using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crucero.Application.DTOs
{
    public record PreciosHabitacionesDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El precio de la habitacion es requerida.")]
        public decimal Precio { get; set; }

        public int HabitacionId { get; set; }

        public int FechaCruceroId { get; set; }

        public virtual FechasCrucerosDTO FechaCrucero { get; set; } = null!;

        public virtual HabitacionesDTO Habitacion { get; set; } = null!;
    }
}
