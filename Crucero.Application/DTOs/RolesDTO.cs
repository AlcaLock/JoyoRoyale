using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crucero.Application.DTOs
{
    public record RolesDTO
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public virtual List<UsuariosDTO> Usuario { get; set; } = null!;
    }

}
