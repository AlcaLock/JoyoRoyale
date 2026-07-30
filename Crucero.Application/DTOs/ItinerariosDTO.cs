using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crucero.Application.DTOs
{
    public record ItinerariosDTO
    {
        public int Id { get; set; }

        public int Dia { get; set; }

        public int PuertoId { get; set; }
        [Required(ErrorMessage = "Debe ingresar una descripción.")]
        public string? Descripcion { get; set; }

        public int CruceroId { get; set; }

        public virtual CrucerosDTO Crucero { get; set; } = null!;

        public virtual PuertosDTO Puerto { get; set; } = null!;
    }
}
