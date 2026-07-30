using System;
using System.Collections.Generic;

namespace JoyoRoyale.Infraestructure.Models;

public partial class Reservas
{
    public int Id { get; set; }

    public int UsuarioId { get; set; }

    public int CruceroId { get; set; }

    public decimal Total { get; set; }

    public virtual Cruceros Crucero { get; set; } = null!;

    public virtual ICollection<Huespedes> Huespedes { get; set; } = new List<Huespedes>();

    public virtual ICollection<ReservasComplementos> ReservasComplementos { get; set; } = new List<ReservasComplementos>();

    public virtual ICollection<ReservasHabitaciones> ReservasHabitaciones { get; set; } = new List<ReservasHabitaciones>();

    public virtual Usuarios Usuario { get; set; } = null!;
}
