using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiPulperiaBarrio.Data;
using MiPulperiaBarrio.Models.Entidades;

namespace MiPulperiaBarrio.Controllers;

public class ConsultasController : Controller
{
    private readonly PulperiaContext _context;

    public ConsultasController(PulperiaContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Inventario(
        string? buscar, int? sucursalId)
    {
        var consulta = _context.Set<VwPulProductosInventario>()
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(buscar))
        {
            var texto = buscar.Trim();

            consulta = consulta.Where(p =>
                EF.Functions.ILike(p.Producto!, $"%{texto}%") ||
                EF.Functions.ILike(p.Codigo!, $"%{texto}%") ||
                EF.Functions.ILike(p.Categoria!, $"%{texto}%"));
        }

        if (sucursalId.HasValue)
        {
            consulta = consulta.Where(p =>
                p.SucursalId == sucursalId.Value);
        }

        ViewBag.Buscar = buscar;
        ViewBag.SucursalId = sucursalId;

        ViewBag.Sucursales = await _context.PulSucursales
            .AsNoTracking()
            .OrderBy(s => s.Nombre)
            .Select(s => new { s.Id, s.Nombre })
            .ToListAsync();

        var datos = await consulta
            .OrderBy(p => p.Producto)
            .ThenBy(p => p.Sucursal)
            .ToListAsync();

        return View(datos);
    }

    [HttpGet]
    public async Task<IActionResult> Reservas(
        string? buscar, string? estado)
    {
        var consulta = _context.Set<VwPulReservasCliente>()
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(buscar))
        {
            var texto = buscar.Trim();

            consulta = consulta.Where(r =>
                EF.Functions.ILike(r.Cliente!, $"%{texto}%") ||
                EF.Functions.ILike(r.Identificacion!, $"%{texto}%"));
        }

        if (!string.IsNullOrWhiteSpace(estado))
        {
            var filtro = estado.Trim();

            consulta = consulta.Where(r =>
                r.Estado == filtro);
        }

        ViewBag.Buscar = buscar;
        ViewBag.Estado = estado;

        var datos = await consulta
            .OrderByDescending(r => r.FechaReserva)
            .ThenByDescending(r => r.ReservaId)
            .ToListAsync();

        return View(datos);
    }
}
