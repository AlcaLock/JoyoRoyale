using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Crucero.Application.DTOs
{
    public record HabitacionesDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la habitación es obligatorio.")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "La Descripcion es obligatoria.")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "La Cantidad minima es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La capacidad mínima debe ser al menos 1.")]
        public int CapacidadMinima { get; set; }

        [Required(ErrorMessage = "La Cantidad maxima es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La capacidad máxima debe ser al menos 1.")]
        public int CapacidadMaxima { get; set; }


        public byte[] Imagen { get; set; } = null!;

        [Required(ErrorMessage = "El tamaño de la habitacion es obligatorio.")]
        [Range(1, double.MaxValue, ErrorMessage = "El tamaño de la habitación debe ser un valor positivo.")]
        public double Tamano { get; set; }

        public virtual List<BarcoHabitacionesDTO> BarcoHabitaciones { get; set; } = new List<BarcoHabitacionesDTO>();

        public virtual List<PreciosHabitacionesDTO> PreciosHabitaciones { get; set; } = new List<PreciosHabitacionesDTO>();

        public virtual List<ReservasHabitacionesDTO> ReservasHabitaciones { get; set; } = new List<ReservasHabitacionesDTO>();
    }
}
