
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Crucero.Application.DTOs
{
    public record CrucerosDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre del crucero es obligatorio.")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "Debe ingresar una descripción.")]
        public string Descripcion { get; set; } = null!;

        [Required(ErrorMessage = "Debe subir una imagen del crucero.")]
        public byte[] Imagen { get; set; } = null!;

        [Range(1, int.MaxValue, ErrorMessage = "La duración en días debe ser mayor a 2.")]
        public int Dias { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un barco.")]
        public int BarcoId { get; set; }

        [JsonIgnore]
        public virtual BarcosDTO Barco { get; set; } = null!;

        public List<ItinerariosDTO> Itinerarios { get; set; } = new List<ItinerariosDTO>();
        public List<FechasCrucerosDTO> FechasCruceros { get; set; } = new List<FechasCrucerosDTO>();

        [JsonIgnore]
        public virtual List<ReservasDTO> Reservas { get; set; } = new List<ReservasDTO>();
    }

}
