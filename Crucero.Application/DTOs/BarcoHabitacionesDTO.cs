
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crucero.Application.DTOs
{
    public record BarcoHabitacionesDTO
    {
        public int Id { get; set; }

        public int BarcoId { get; set; }

        public int HabitacionId { get; set; }

        public int CantidadDisponible { get; set; }

        public  virtual BarcosDTO Barco { get; set; } = null!;

        public  virtual HabitacionesDTO Habitacion { get; set; } = null!;
    }
}
