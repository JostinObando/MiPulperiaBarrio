using System;
using System.Collections.Generic;
using MiPulperiaBarrio.Models.Entidades;
using Microsoft.EntityFrameworkCore;

namespace MiPulperiaBarrio.Data;

public partial class PulperiaContext : DbContext
{
    public PulperiaContext(DbContextOptions<PulperiaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<PulCliente> PulClientes { get; set; }

    public virtual DbSet<PulIngresosInventario> PulIngresosInventarios { get; set; }

    public virtual DbSet<PulInventario> PulInventarios { get; set; }

    public virtual DbSet<PulProducto> PulProductos { get; set; }

    public virtual DbSet<PulReserva> PulReservas { get; set; }

    public virtual DbSet<PulSucursale> PulSucursales { get; set; }

    public virtual DbSet<VwPulProductosInventario> VwPulProductosInventarios { get; set; }

    public virtual DbSet<VwPulReservasCliente> VwPulReservasClientes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("user_25", "match_kind", new[] { "friendly", "official" })
            .HasPostgresEnum("user_25", "match_status", new[] { "scheduled", "live", "finished" })
            .HasPostgresEnum("user_25", "sport", new[] { "soccer", "basketball", "baseball", "volleyball", "rugby", "hockey", "football" });

        modelBuilder.Entity<PulCliente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pul_clientes_pkey");

            entity.ToTable("pul_clientes", "user_09");

            entity.HasIndex(e => e.Correo, "pul_clientes_correo_key").IsUnique();

            entity.HasIndex(e => e.Identificacion, "pul_clientes_identificacion_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.Correo)
                .HasMaxLength(150)
                .HasColumnName("correo");
            entity.Property(e => e.Identificacion)
                .HasMaxLength(20)
                .HasColumnName("identificacion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(120)
                .HasColumnName("nombre");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .HasColumnName("telefono");
        });

        modelBuilder.Entity<PulIngresosInventario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pul_ingresos_inventario_pkey");

            entity.ToTable("pul_ingresos_inventario", "user_09");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.FechaIngreso)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("fecha_ingreso");
            entity.Property(e => e.ProductoId).HasColumnName("producto_id");
            entity.Property(e => e.SucursalId).HasColumnName("sucursal_id");

            entity.HasOne(d => d.Producto).WithMany(p => p.PulIngresosInventarios)
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ingreso_producto");

            entity.HasOne(d => d.Sucursal).WithMany(p => p.PulIngresosInventarios)
                .HasForeignKey(d => d.SucursalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ingreso_sucursal");
        });

        modelBuilder.Entity<PulInventario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pul_inventarios_pkey");

            entity.ToTable("pul_inventarios", "user_09");

            entity.HasIndex(e => new { e.SucursalId, e.ProductoId }, "uq_inventario_sucursal_producto").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CantidadDisponible).HasColumnName("cantidad_disponible");
            entity.Property(e => e.CantidadReservada).HasColumnName("cantidad_reservada");
            entity.Property(e => e.ProductoId).HasColumnName("producto_id");
            entity.Property(e => e.SucursalId).HasColumnName("sucursal_id");

            entity.HasOne(d => d.Producto).WithMany(p => p.PulInventarios)
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_inventario_producto");

            entity.HasOne(d => d.Sucursal).WithMany(p => p.PulInventarios)
                .HasForeignKey(d => d.SucursalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_inventario_sucursal");
        });

        modelBuilder.Entity<PulProducto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pul_productos_pkey");

            entity.ToTable("pul_productos", "user_09");

            entity.HasIndex(e => e.Codigo, "pul_productos_codigo_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.Atributos)
                .HasDefaultValueSql("'{}'::jsonb")
                .HasColumnType("jsonb")
                .HasColumnName("atributos");
            entity.Property(e => e.Categoria)
                .HasMaxLength(80)
                .HasColumnName("categoria");
            entity.Property(e => e.Codigo)
                .HasMaxLength(30)
                .HasColumnName("codigo");
            entity.Property(e => e.ExistenciaMinima).HasColumnName("existencia_minima");
            entity.Property(e => e.HorasReserva).HasColumnName("horas_reserva");
            entity.Property(e => e.MaximoReserva).HasColumnName("maximo_reserva");
            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .HasColumnName("nombre");
            entity.Property(e => e.Precio)
                .HasPrecision(12, 2)
                .HasColumnName("precio");
        });

        modelBuilder.Entity<PulReserva>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pul_reservas_pkey");

            entity.ToTable("pul_reservas", "user_09");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.ClienteId).HasColumnName("cliente_id");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .HasDefaultValueSql("'RESERVADO'::character varying")
                .HasColumnName("estado");
            entity.Property(e => e.FechaCierre).HasColumnName("fecha_cierre");
            entity.Property(e => e.FechaReserva)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("fecha_reserva");
            entity.Property(e => e.FechaVencimiento).HasColumnName("fecha_vencimiento");
            entity.Property(e => e.PrecioUnitario)
                .HasPrecision(12, 2)
                .HasColumnName("precio_unitario");
            entity.Property(e => e.ProductoId).HasColumnName("producto_id");
            entity.Property(e => e.SucursalId).HasColumnName("sucursal_id");

            entity.HasOne(d => d.Cliente).WithMany(p => p.PulReservas)
                .HasForeignKey(d => d.ClienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_reserva_cliente");

            entity.HasOne(d => d.PulInventario).WithMany(p => p.PulReservas)
                .HasPrincipalKey(p => new { p.SucursalId, p.ProductoId })
                .HasForeignKey(d => new { d.SucursalId, d.ProductoId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_reserva_inventario");
        });

        modelBuilder.Entity<PulSucursale>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pul_sucursales_pkey");

            entity.ToTable("pul_sucursales", "user_09");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activa)
                .HasDefaultValue(true)
                .HasColumnName("activa");
            entity.Property(e => e.Direccion)
                .HasMaxLength(200)
                .HasColumnName("direccion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.Provincia)
                .HasMaxLength(60)
                .HasColumnName("provincia");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .HasColumnName("telefono");
        });

        modelBuilder.Entity<VwPulProductosInventario>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_pul_productos_inventario", "user_09");

            entity.Property(e => e.Atributos)
                .HasColumnType("jsonb")
                .HasColumnName("atributos");
            entity.Property(e => e.CantidadDisponible).HasColumnName("cantidad_disponible");
            entity.Property(e => e.CantidadReservada).HasColumnName("cantidad_reservada");
            entity.Property(e => e.Categoria)
                .HasMaxLength(80)
                .HasColumnName("categoria");
            entity.Property(e => e.Codigo)
                .HasMaxLength(30)
                .HasColumnName("codigo");
            entity.Property(e => e.ExistenciaMinima).HasColumnName("existencia_minima");
            entity.Property(e => e.HorasReserva).HasColumnName("horas_reserva");
            entity.Property(e => e.MaximoReserva).HasColumnName("maximo_reserva");
            entity.Property(e => e.Precio)
                .HasPrecision(12, 2)
                .HasColumnName("precio");
            entity.Property(e => e.Producto)
                .HasMaxLength(150)
                .HasColumnName("producto");
            entity.Property(e => e.ProductoId).HasColumnName("producto_id");
            entity.Property(e => e.Provincia)
                .HasMaxLength(60)
                .HasColumnName("provincia");
            entity.Property(e => e.Sucursal)
                .HasMaxLength(100)
                .HasColumnName("sucursal");
            entity.Property(e => e.SucursalId).HasColumnName("sucursal_id");
        });

        modelBuilder.Entity<VwPulReservasCliente>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_pul_reservas_clientes", "user_09");

            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.Cliente)
                .HasMaxLength(120)
                .HasColumnName("cliente");
            entity.Property(e => e.ClienteId).HasColumnName("cliente_id");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .HasColumnName("estado");
            entity.Property(e => e.FechaCierre).HasColumnName("fecha_cierre");
            entity.Property(e => e.FechaReserva).HasColumnName("fecha_reserva");
            entity.Property(e => e.FechaVencimiento).HasColumnName("fecha_vencimiento");
            entity.Property(e => e.Identificacion)
                .HasMaxLength(20)
                .HasColumnName("identificacion");
            entity.Property(e => e.MontoTotal).HasColumnName("monto_total");
            entity.Property(e => e.PrecioUnitario)
                .HasPrecision(12, 2)
                .HasColumnName("precio_unitario");
            entity.Property(e => e.Producto)
                .HasMaxLength(150)
                .HasColumnName("producto");
            entity.Property(e => e.ProductoId).HasColumnName("producto_id");
            entity.Property(e => e.ReservaId).HasColumnName("reserva_id");
            entity.Property(e => e.Resultado).HasColumnName("resultado");
            entity.Property(e => e.Sucursal)
                .HasMaxLength(100)
                .HasColumnName("sucursal");
            entity.Property(e => e.SucursalId).HasColumnName("sucursal_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}



