using System;
using System.Collections.Generic;

namespace MiPulperiaBarrio.Models.Entidades;

public partial class PulSucursale
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Provincia { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public string? Telefono { get; set; }

    public bool Activa { get; set; }

    public virtual ICollection<PulIngresosInventario> PulIngresosInventarios { get; set; } = new List<PulIngresosInventario>();

    public virtual ICollection<PulInventario> PulInventarios { get; set; } = new List<PulInventario>();
}
