using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crucero.Application.DTOs
{
    public record ReservasComplementosDTO
    {
        public int Id { get; set; }

        public int ComplementoId { get; set; }

        public int ReservaId { get; set; }

        public int Cantidad { get; set; }

        public decimal Total { get; set; }

        public virtual ComplementosDTO Complemento { get; set; } = null!;

        public virtual ReservasDTO Reserva { get; set; } = null!;
    }
}
