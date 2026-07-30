using System;
using System.Collections.Generic;

namespace JoyoRoyale.Infraestructure.Models;

public partial class Cruceros
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public byte[] Imagen { get; set; } = null!;

    public int Dias { get; set; }

    public int BarcoId { get; set; }

    public virtual Barcos Barco { get; set; } = null!;

    public virtual ICollection<FechasCruceros> FechasCruceros { get; set; } = new List<FechasCruceros>();

    public virtual ICollection<Itinerarios> Itinerarios { get; set; } = new List<Itinerarios>();

    public virtual ICollection<Reservas> Reservas { get; set; } = new List<Reservas>();
}
