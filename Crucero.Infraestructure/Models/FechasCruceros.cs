using System;
using System.Collections.Generic;

namespace JoyoRoyale.Infraestructure.Models;

public partial class FechasCruceros
{
    public int Id { get; set; }

    public DateOnly FechaInicio { get; set; }

    public DateOnly FechaLimitePago { get; set; }

    public int CruceroId { get; set; }

    public virtual Cruceros Crucero { get; set; } = null!;

    public virtual ICollection<PreciosHabitaciones> PreciosHabitaciones { get; set; } = new List<PreciosHabitaciones>();
}
