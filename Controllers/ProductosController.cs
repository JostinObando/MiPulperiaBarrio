using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiPulperiaBarrio.Data;
using MiPulperiaBarrio.Models.Entidades;
using MiPulperiaBarrio.Models.Formularios;

namespace MiPulperiaBarrio.Controllers;

public class ProductosController : Controller
{
    private readonly PulperiaContext _context;

    public ProductosController(PulperiaContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string? buscar)
    {
        var consulta = _context.PulProductos.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(buscar))
        {
            var texto = buscar.Trim();

            consulta = consulta.Where(p =>
                EF.Functions.ILike(p.Nombre, $"%{texto}%") ||
                EF.Functions.ILike(p.Codigo, $"%{texto}%") ||
                EF.Functions.ILike(p.Categoria, $"%{texto}%"));
        }

        ViewBag.Buscar = buscar;

        return View(await consulta.OrderBy(p => p.Nombre).ToListAsync());
    }

    [HttpGet]
    public IActionResult Crear()
    {
        return View(new ProductoFormulario());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(ProductoFormulario modelo)
    {
        await Validar(modelo);

        if (!ModelState.IsValid)
            return View(modelo);

        var producto = new PulProducto
        {
            Codigo = modelo.Codigo.Trim(),
            Nombre = modelo.Nombre.Trim(),
            Categoria = modelo.Categoria.Trim(),
            Precio = modelo.Precio,
            Atributos = modelo.Atributos.Trim(),
            MaximoReserva = modelo.MaximoReserva,
            HorasReserva = modelo.HorasReserva,
            ExistenciaMinima = modelo.ExistenciaMinima,
            Activo = modelo.Activo
        };

        try
        {
            _context.PulProductos.Add(producto);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError("",
                "No se pudo registrar el producto. Revise el código y los datos ingresados.");

            return View(modelo);
        }

        TempData["Mensaje"] = "Producto registrado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var producto = await _context.PulProductos
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);

        if (producto == null)
            return NotFound();

        return View(new ProductoFormulario
        {
            Id = producto.Id,
            Codigo = producto.Codigo,
            Nombre = producto.Nombre,
            Categoria = producto.Categoria,
            Precio = producto.Precio,
            Atributos = FormatearJson(producto.Atributos),
            MaximoReserva = producto.MaximoReserva,
            HorasReserva = producto.HorasReserva,
            ExistenciaMinima = producto.ExistenciaMinima,
            Activo = producto.Activo
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, ProductoFormulario modelo)
    {
        if (id != modelo.Id)
            return BadRequest();

        await Validar(modelo);

        if (!ModelState.IsValid)
            return View(modelo);

        var producto = await _context.PulProductos
            .FirstOrDefaultAsync(p => p.Id == id);

        if (producto == null)
            return NotFound();

        producto.Codigo = modelo.Codigo.Trim();
        producto.Nombre = modelo.Nombre.Trim();
        producto.Categoria = modelo.Categoria.Trim();
        producto.Precio = modelo.Precio;
        producto.Atributos = modelo.Atributos.Trim();
        producto.MaximoReserva = modelo.MaximoReserva;
        producto.HorasReserva = modelo.HorasReserva;
        producto.ExistenciaMinima = modelo.ExistenciaMinima;
        producto.Activo = modelo.Activo;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError("",
                "No se pudo actualizar el producto. Revise los datos ingresados.");

            return View(modelo);
        }

        TempData["Mensaje"] = "Producto actualizado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CambiarEstado(int id)
    {
        var producto = await _context.PulProductos
            .FirstOrDefaultAsync(p => p.Id == id);

        if (producto == null)
            return NotFound();

        producto.Activo = !producto.Activo;

        try
        {
            await _context.SaveChangesAsync();
            TempData["Mensaje"] = "Estado del producto actualizado.";
        }
        catch (DbUpdateException)
        {
            TempData["Error"] = "No se pudo cambiar el estado del producto.";
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task Validar(ProductoFormulario modelo)
    {
        if (string.IsNullOrWhiteSpace(modelo.Codigo))
            ModelState.AddModelError(nameof(modelo.Codigo),
                "El código es obligatorio.");

        if (string.IsNullOrWhiteSpace(modelo.Nombre))
            ModelState.AddModelError(nameof(modelo.Nombre),
                "El nombre es obligatorio.");

        if (string.IsNullOrWhiteSpace(modelo.Categoria))
            ModelState.AddModelError(nameof(modelo.Categoria),
                "La categoría es obligatoria.");

        if (string.IsNullOrWhiteSpace(modelo.Atributos))
        {
            ModelState.AddModelError(nameof(modelo.Atributos),
                "Los atributos JSON son obligatorios.");
        }
        else
        {
            try
            {
                using var json = JsonDocument.Parse(modelo.Atributos);

                if (json.RootElement.ValueKind != JsonValueKind.Object)
                {
                    ModelState.AddModelError(nameof(modelo.Atributos),
                        "Los atributos deben ser un objeto JSON, por ejemplo {\"marca\":\"Ejemplo\"}.");
                }
            }
            catch (JsonException)
            {
                ModelState.AddModelError(nameof(modelo.Atributos),
                    "El contenido no tiene un formato JSON válido.");
            }
        }

        if (!ModelState.IsValid)
            return;

        var codigo = modelo.Codigo.Trim();

        if (await _context.PulProductos.AnyAsync(p =>
            p.Codigo == codigo && p.Id != modelo.Id))
        {
            ModelState.AddModelError(nameof(modelo.Codigo),
                "Ya existe un producto con este código.");
        }
    }

    private static string FormatearJson(string valor)
    {
        try
        {
            using var json = JsonDocument.Parse(valor);

            return JsonSerializer.Serialize(json.RootElement,
                new JsonSerializerOptions { WriteIndented = true });
        }
        catch (JsonException)
        {
            return valor;
        }
    }
}
