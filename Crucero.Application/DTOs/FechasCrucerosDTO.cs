
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crucero.Application.DTOs
{
    public record FechasCrucerosDTO
    {
        public int Id { get; set; }

        //[Required(ErrorMessage = "Debe ingresar una fecha de inicio.")]
        //public DateOnly FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es requerida.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Inicio")]
        [Range(typeof(DateOnly), "1/1/2000", "1/1/2100", ErrorMessage = "La fecha de inicio debe ser mayor o igual al día de hoy.")]
        public DateOnly FechaInicio { get; set; }


        public DateOnly FechaLimitePago { get; set; }

        public int CruceroId { get; set; }

        public virtual CrucerosDTO Crucero { get; set; } = null!;

        public virtual ICollection<PreciosHabitacionesDTO> PreciosHabitaciones { get; set; } = new List<PreciosHabitacionesDTO>();
    }

}
