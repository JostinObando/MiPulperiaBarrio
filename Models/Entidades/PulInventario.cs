using System;
using System.Collections.Generic;

namespace MiPulperiaBarrio.Models.Entidades;

public partial class PulInventario
{
    public int Id { get; set; }

    public int SucursalId { get; set; }

    public int ProductoId { get; set; }

    public int CantidadDisponible { get; set; }

    public int CantidadReservada { get; set; }

    public virtual PulProducto Producto { get; set; } = null!;

    public virtual ICollection<PulReserva> PulReservas { get; set; } = new List<PulReserva>();

    public virtual PulSucursale Sucursal { get; set; } = null!;
}
