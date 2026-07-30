using System;
using System.Collections.Generic;

namespace JoyoRoyale.Infraestructure.Models;

public partial class PreciosHabitaciones
{
    public int Id { get; set; }

    public decimal Precio { get; set; }

    public int HabitacionId { get; set; }

    public int FechaCruceroId { get; set; }

    public virtual FechasCruceros FechaCrucero { get; set; } = null!;

    public virtual Habitaciones Habitacion { get; set; } = null!;
}
