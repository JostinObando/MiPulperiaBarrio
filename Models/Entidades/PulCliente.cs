using System;
using System.Collections.Generic;

namespace MiPulperiaBarrio.Models.Entidades;

public partial class PulCliente
{
    public int Id { get; set; }

    public string Identificacion { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string? Telefono { get; set; }

    public bool Activo { get; set; }

    public virtual ICollection<PulReserva> PulReservas { get; set; } = new List<PulReserva>();
}
