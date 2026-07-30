using System;
using System.Collections.Generic;

namespace JoyoRoyale.Infraestructure.Models;

public partial class Barcos
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public int Capacidad { get; set; }

    public byte[] Imagen { get; set; } = null!;

    public virtual ICollection<BarcoHabitaciones> BarcoHabitaciones { get; set; } = new List<BarcoHabitaciones>();

    public virtual ICollection<Cruceros> Cruceros { get; set; } = new List<Cruceros>();
}
