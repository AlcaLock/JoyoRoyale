using System;
using System.Collections.Generic;

namespace JoyoRoyale.Infraestructure.Models;

public partial class Usuarios
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public DateOnly FechaNacimiento { get; set; }

    public string Pais { get; set; } = null!;

    public string Contrasena { get; set; } = null!;

    public int RolId { get; set; }

    public virtual ICollection<Reservas> Reservas { get; set; } = new List<Reservas>();

    public virtual Roles Rol { get; set; } = null!;
}
