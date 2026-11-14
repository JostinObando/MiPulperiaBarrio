using System;
using System.Collections.Generic;

namespace MiPulperiaBarrio.Models.Entidades;

public partial class PulIngresosInventario
{
    public int Id { get; set; }

    public int SucursalId { get; set; }

    public int ProductoId { get; set; }

    public int Cantidad { get; set; }

    public DateTime FechaIngreso { get; set; }

    public virtual PulProducto Producto { get; set; } = null!;

    public virtual PulSucursale Sucursal { get; set; } = null!;
}
