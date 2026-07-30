using System;
using System.Collections.Generic;

namespace JoyoRoyale.Infraestructure.Models;

public partial class Itinerarios
{
    public int Id { get; set; }

    public int Dia { get; set; }

    public int PuertoId { get; set; }

    public string? Descripcion { get; set; }

    public int CruceroId { get; set; }

    public virtual Cruceros Crucero { get; set; } = null!;

    public virtual Puertos Puerto { get; set; } = null!;
}
