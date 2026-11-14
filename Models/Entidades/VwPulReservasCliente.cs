using System;
using System.Collections.Generic;

namespace MiPulperiaBarrio.Models.Entidades;

public partial class VwPulReservasCliente
{
    public int? ReservaId { get; set; }

    public int? ClienteId { get; set; }

    public string? Cliente { get; set; }

    public string? Identificacion { get; set; }

    public int? SucursalId { get; set; }

    public string? Sucursal { get; set; }

    public int? ProductoId { get; set; }

    public string? Producto { get; set; }

    public int? Cantidad { get; set; }

    public decimal? PrecioUnitario { get; set; }

    public decimal? MontoTotal { get; set; }

    public DateTime? FechaReserva { get; set; }

    public DateTime? FechaVencimiento { get; set; }

    public DateTime? FechaCierre { get; set; }

    public string? Estado { get; set; }

    public string? Resultado { get; set; }
}
