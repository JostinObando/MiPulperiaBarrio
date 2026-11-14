using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiPulperiaBarrio.Data;
using MiPulperiaBarrio.Models.Entidades;
using MiPulperiaBarrio.Models.Formularios;

namespace MiPulperiaBarrio.Controllers;

public class ClientesController : Controller
{
    private readonly PulperiaContext _context;

    public ClientesController(PulperiaContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string? buscar)
    {
        var consulta = _context.PulClientes.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(buscar))
        {
            var texto = buscar.Trim();

            consulta = consulta.Where(c =>
                EF.Functions.ILike(c.Nombre, $"%{texto}%") ||
                EF.Functions.ILike(c.Identificacion, $"%{texto}%"));
        }

        ViewBag.Buscar = buscar;

        return View(await consulta.OrderBy(c => c.Nombre).ToListAsync());
    }

    [HttpGet]
    public IActionResult Crear()
    {
        return View(new ClienteFormulario());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(ClienteFormulario modelo)
    {
        await Validar(modelo);

        if (!ModelState.IsValid)
            return View(modelo);

        var cliente = new PulCliente
        {
            Identificacion = modelo.Identificacion.Trim(),
            Nombre = modelo.Nombre.Trim(),
            Correo = modelo.Correo.Trim(),
            Telefono = string.IsNullOrWhiteSpace(modelo.Telefono)
                ? null : modelo.Telefono.Trim(),
            Activo = modelo.Activo
        };

        try
        {
            _context.PulClientes.Add(cliente);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError("",
                "No se pudo registrar el cliente. Verifique que la identificación y el correo sean únicos.");

            return View(modelo);
        }

        TempData["Mensaje"] = "Cliente registrado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var cliente = await _context.PulClientes
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

        if (cliente == null)
            return NotFound();

        return View(new ClienteFormulario
        {
            Id = cliente.Id,
            Identificacion = cliente.Identificacion,
            Nombre = cliente.Nombre,
            Correo = cliente.Correo,
            Telefono = cliente.Telefono,
            Activo = cliente.Activo
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, ClienteFormulario modelo)
    {
        if (id != modelo.Id)
            return BadRequest();

        await Validar(modelo);

        if (!ModelState.IsValid)
            return View(modelo);

        var cliente = await _context.PulClientes
            .FirstOrDefaultAsync(c => c.Id == id);

        if (cliente == null)
            return NotFound();

        cliente.Identificacion = modelo.Identificacion.Trim();
        cliente.Nombre = modelo.Nombre.Trim();
        cliente.Correo = modelo.Correo.Trim();
        cliente.Telefono = string.IsNullOrWhiteSpace(modelo.Telefono)
            ? null : modelo.Telefono.Trim();
        cliente.Activo = modelo.Activo;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError("",
                "No se pudieron guardar los cambios. Verifique los datos ingresados.");

            return View(modelo);
        }

        TempData["Mensaje"] = "Cliente actualizado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CambiarEstado(int id)
    {
        var cliente = await _context.PulClientes
            .FirstOrDefaultAsync(c => c.Id == id);

        if (cliente == null)
            return NotFound();

        cliente.Activo = !cliente.Activo;

        try
        {
            await _context.SaveChangesAsync();
            TempData["Mensaje"] = "Estado del cliente actualizado.";
        }
        catch (DbUpdateException)
        {
            TempData["Error"] = "No se pudo cambiar el estado del cliente.";
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task Validar(ClienteFormulario modelo)
    {
        if (string.IsNullOrWhiteSpace(modelo.Identificacion))
            ModelState.AddModelError(nameof(modelo.Identificacion),
                "La identificación es obligatoria.");

        if (string.IsNullOrWhiteSpace(modelo.Nombre))
            ModelState.AddModelError(nameof(modelo.Nombre),
                "El nombre es obligatorio.");

        if (string.IsNullOrWhiteSpace(modelo.Correo))
            ModelState.AddModelError(nameof(modelo.Correo),
                "El correo es obligatorio.");

        if (!ModelState.IsValid)
            return;

        var identificacion = modelo.Identificacion.Trim();
        var correo = modelo.Correo.Trim();

        if (await _context.PulClientes.AnyAsync(c =>
            c.Identificacion == identificacion && c.Id != modelo.Id))
        {
            ModelState.AddModelError(nameof(modelo.Identificacion),
                "Ya existe un cliente con esta identificación.");
        }

        if (await _context.PulClientes.AnyAsync(c =>
            EF.Functions.ILike(c.Correo, correo) && c.Id != modelo.Id))
        {
            ModelState.AddModelError(nameof(modelo.Correo),
                "Ya existe un cliente con este correo.");
        }
    }
}
