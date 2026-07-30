using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crucero.Application.DTOs
{
    public record HuespedesDTO
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public string Apellidos { get; set; } = null!;

        public int Edad { get; set; }

        public string DocumentoIdentidad { get; set; } = null!;
        public string Telefono { get; set; } = null!;

        public int ReservaId { get; set; }

        public virtual ReservasDTO Reserva { get; set; } = null!;
    }

}
