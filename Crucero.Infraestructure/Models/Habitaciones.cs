using System;
using System.Collections.Generic;

namespace JoyoRoyale.Infraestructure.Models;

public partial class Habitaciones
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public int CapacidadMinima { get; set; }

    public int CapacidadMaxima { get; set; }

    public double Tamano { get; set; }

    public byte[] Imagen { get; set; } = null!;

    public virtual ICollection<BarcoHabitaciones> BarcoHabitaciones { get; set; } = new List<BarcoHabitaciones>();

    public virtual ICollection<PreciosHabitaciones> PreciosHabitaciones { get; set; } = new List<PreciosHabitaciones>();

    public virtual ICollection<ReservasHabitaciones> ReservasHabitaciones { get; set; } = new List<ReservasHabitaciones>();
}
