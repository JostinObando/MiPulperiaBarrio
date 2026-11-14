using System;
using System.Collections.Generic;

namespace MiPulperiaBarrio.Models.Entidades;

public partial class PulProducto
{
    public int Id { get; set; }

    public string Codigo { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Categoria { get; set; } = null!;

    public decimal Precio { get; set; }

    public string Atributos { get; set; } = null!;

    public int MaximoReserva { get; set; }

    public int HorasReserva { get; set; }

    public int ExistenciaMinima { get; set; }

    public bool Activo { get; set; }

    public virtual ICollection<PulIngresosInventario> PulIngresosInventarios { get; set; } = new List<PulIngresosInventario>();

    public virtual ICollection<PulInventario> PulInventarios { get; set; } = new List<PulInventario>();
}
