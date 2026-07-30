using System;
using System.Collections.Generic;

namespace JoyoRoyale.Infraestructure.Models;

public partial class Complementos
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public decimal Precio { get; set; }

    public string TipoAplicacion { get; set; } = null!;

    public virtual ICollection<ReservasComplementos> ReservasComplementos { get; set; } = new List<ReservasComplementos>();
}
