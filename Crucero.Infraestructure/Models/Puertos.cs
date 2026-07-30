using System;
using System.Collections.Generic;

namespace JoyoRoyale.Infraestructure.Models;

public partial class Puertos
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Pais { get; set; } = null!;

    public int DestinoId { get; set; }

    public virtual Destinos Destino { get; set; } = null!;

    public virtual ICollection<Itinerarios> Itinerarios { get; set; } = new List<Itinerarios>();
}
