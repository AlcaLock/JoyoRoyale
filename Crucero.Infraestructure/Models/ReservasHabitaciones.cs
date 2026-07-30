using System;
using System.Collections.Generic;

namespace JoyoRoyale.Infraestructure.Models;

public partial class ReservasHabitaciones
{
    public int Id { get; set; }

    public int ReservaId { get; set; }

    public int HabitacionId { get; set; }

    public int CantidadPasajeros { get; set; }

    public virtual Habitaciones Habitacion { get; set; } = null!;

    public virtual Reservas Reserva { get; set; } = null!;
}
