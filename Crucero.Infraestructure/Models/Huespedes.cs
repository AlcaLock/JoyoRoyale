using System;
using System.Collections.Generic;

namespace JoyoRoyale.Infraestructure.Models;

public partial class Huespedes
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public int Edad { get; set; }

    public string DocumentoIdentidad { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public int ReservaId { get; set; }

    public virtual Reservas Reserva { get; set; } = null!;
}
