using System;
using System.Collections.Generic;

namespace MiPulperiaBarrio.Models.Entidades;

public partial class PulReserva
{
    public int Id { get; set; }

    public int ClienteId { get; set; }

    public int SucursalId { get; set; }

    public int ProductoId { get; set; }

    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public DateTime FechaReserva { get; set; }

    public DateTime FechaVencimiento { get; set; }

    public DateTime? FechaCierre { get; set; }

    public string Estado { get; set; } = null!;

    public virtual PulCliente Cliente { get; set; } = null!;

    public virtual PulInventario PulInventario { get; set; } = null!;
}
