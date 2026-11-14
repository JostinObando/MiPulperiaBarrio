using System;
using System.Collections.Generic;

namespace MiPulperiaBarrio.Models.Entidades;

public partial class VwPulProductosInventario
{
    public int? SucursalId { get; set; }

    public string? Sucursal { get; set; }

    public string? Provincia { get; set; }

    public int? ProductoId { get; set; }

    public string? Codigo { get; set; }

    public string? Producto { get; set; }

    public string? Categoria { get; set; }

    public decimal? Precio { get; set; }

    public string? Atributos { get; set; }

    public int? MaximoReserva { get; set; }

    public int? HorasReserva { get; set; }

    public int? ExistenciaMinima { get; set; }

    public int? CantidadDisponible { get; set; }

    public int? CantidadReservada { get; set; }
}
