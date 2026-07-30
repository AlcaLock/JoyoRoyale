using System;
using System.Collections.Generic;

namespace JoyoRoyale.Infraestructure.Models;

public partial class Destinos
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Puertos> Puertos { get; set; } = new List<Puertos>();
}
