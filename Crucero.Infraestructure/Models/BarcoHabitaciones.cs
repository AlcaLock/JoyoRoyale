using System;
using System.Collections.Generic;

namespace JoyoRoyale.Infraestructure.Models;

public partial class BarcoHabitaciones
{
    public int Id { get; set; }

    public int BarcoId { get; set; }

    public int HabitacionId { get; set; }

    public int CantidadDisponible { get; set; }

    public virtual Barcos Barco { get; set; } = null!;

    public virtual Habitaciones Habitacion { get; set; } = null!;
}
