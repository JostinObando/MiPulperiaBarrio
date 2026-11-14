using System.ComponentModel.DataAnnotations;

namespace MiPulperiaBarrio.Models.Formularios;

public class ProductoFormulario
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El código es obligatorio")]
    [StringLength(30)]
    public string Codigo { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(150)]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "La categoría es obligatoria")]
    [StringLength(80)]
    public string Categoria { get; set; } = string.Empty;

    [Range(0.01, 9999999999.99,
        ErrorMessage = "El precio debe ser mayor que cero")]
    public decimal Precio { get; set; }

    [Required(ErrorMessage = "Los atributos son obligatorios")]
    [Display(Name = "Atributos JSON")]
    public string Atributos { get; set; } = "{}";

    [Range(1, int.MaxValue,
        ErrorMessage = "El máximo de reserva debe ser mayor que cero")]
    [Display(Name = "Máximo de unidades por reserva")]
    public int MaximoReserva { get; set; } = 1;

    [Range(1, int.MaxValue,
        ErrorMessage = "Las horas de reserva deben ser mayores que cero")]
    [Display(Name = "Horas de reserva")]
    public int HorasReserva { get; set; } = 24;

    [Range(0, int.MaxValue,
        ErrorMessage = "La existencia mínima no puede ser negativa")]
    [Display(Name = "Existencia mínima")]
    public int ExistenciaMinima { get; set; }

    public bool Activo { get; set; } = true;
}
