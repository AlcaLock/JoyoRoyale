using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Crucero.Application.DTOs
{
    public record BarcosDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del barco es obligatorio.")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        public string Descripcion { get; set; } = null!;

        [Required(ErrorMessage = "La Cantidad es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La capacidad debe ser un número positivo.")]
        public int Capacidad { get; set; }


        public byte[] Imagen { get; set; } = null!;

        public virtual List<BarcoHabitacionesDTO> BarcoHabitaciones { get; set; } = new List<BarcoHabitacionesDTO>();

        public virtual List<CrucerosDTO> Cruceros { get; set; } = new List<CrucerosDTO>();
    }
}
