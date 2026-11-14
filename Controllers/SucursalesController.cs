using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiPulperiaBarrio.Data;
using MiPulperiaBarrio.Models.Entidades;
using MiPulperiaBarrio.Models.Formularios;

namespace MiPulperiaBarrio.Controllers;

public class SucursalesController : Controller
{
    private readonly PulperiaContext _context;

    public SucursalesController(PulperiaContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var sucursales = await _context.PulSucursales
            .AsNoTracking()
            .OrderBy(s => s.Id)
            .ToListAsync();

        return View(sucursales);
    }

    [HttpGet]
    public IActionResult Crear()
    {
        return View(new SucursalFormulario());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(SucursalFormulario modelo)
    {
        Validar(modelo);

        if (!ModelState.IsValid)
            return View(modelo);

        var sucursal = new PulSucursale
        {
            Nombre = modelo.Nombre.Trim(),
            Provincia = modelo.Provincia.Trim(),
            Direccion = modelo.Direccion.Trim(),
            Telefono = string.IsNullOrWhiteSpace(modelo.Telefono)
                ? null : modelo.Telefono.Trim(),
            Activa = modelo.Activa
        };

        try
        {
            _context.PulSucursales.Add(sucursal);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError("", "No se pudo registrar la sucursal.");
            return View(modelo);
        }

        TempData["Mensaje"] = "Sucursal registrada correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var sucursal = await _context.PulSucursales
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id);

        if (sucursal == null)
            return NotFound();

        var modelo = new SucursalFormulario
        {
            Id = sucursal.Id,
            Nombre = sucursal.Nombre,
            Provincia = sucursal.Provincia,
            Direccion = sucursal.Direccion,
            Telefono = sucursal.Telefono,
            Activa = sucursal.Activa
        };

        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, SucursalFormulario modelo)
    {
        if (id != modelo.Id)
            return BadRequest();

        Validar(modelo);

        if (!ModelState.IsValid)
            return View(modelo);

        var sucursal = await _context.PulSucursales
            .FirstOrDefaultAsync(s => s.Id == id);

        if (sucursal == null)
            return NotFound();

        sucursal.Nombre = modelo.Nombre.Trim();
        sucursal.Provincia = modelo.Provincia.Trim();
        sucursal.Direccion = modelo.Direccion.Trim();
        sucursal.Telefono = string.IsNullOrWhiteSpace(modelo.Telefono)
            ? null : modelo.Telefono.Trim();
        sucursal.Activa = modelo.Activa;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError("", "No se pudieron guardar los cambios.");
            return View(modelo);
        }

        TempData["Mensaje"] = "Sucursal actualizada correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CambiarEstado(int id)
    {
        var sucursal = await _context.PulSucursales
            .FirstOrDefaultAsync(s => s.Id == id);

        if (sucursal == null)
            return NotFound();

        sucursal.Activa = !sucursal.Activa;

        try
        {
            await _context.SaveChangesAsync();
            TempData["Mensaje"] = "Estado actualizado correctamente.";
        }
        catch (DbUpdateException)
        {
            TempData["Error"] = "No se pudo actualizar el estado.";
        }

        return RedirectToAction(nameof(Index));
    }

    private void Validar(SucursalFormulario modelo)
    {
        if (string.IsNullOrWhiteSpace(modelo.Nombre))
            ModelState.AddModelError(nameof(modelo.Nombre), "El nombre es obligatorio.");

        if (string.IsNullOrWhiteSpace(modelo.Provincia))
            ModelState.AddModelError(nameof(modelo.Provincia), "La provincia es obligatoria.");

        if (string.IsNullOrWhiteSpace(modelo.Direccion))
            ModelState.AddModelError(nameof(modelo.Direccion), "La dirección es obligatoria.");
    }
}
