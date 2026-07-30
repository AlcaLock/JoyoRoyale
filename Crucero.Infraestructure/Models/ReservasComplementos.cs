using System;
using System.Collections.Generic;

namespace JoyoRoyale.Infraestructure.Models;

public partial class ReservasComplementos
{
    public int Id { get; set; }

    public int ComplementoId { get; set; }

    public int ReservaId { get; set; }

    public int Cantidad { get; set; }

    public decimal Total { get; set; }

    public virtual Complementos Complemento { get; set; } = null!;

    public virtual Reservas Reserva { get; set; } = null!;
}
