using System.ComponentModel.DataAnnotations;

namespace MiPulperiaBarrio.Models.Formularios;

public class SucursalFormulario
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100)]
    [Display(Name = "Nombre")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "La provincia es obligatoria")]
    [StringLength(60)]
    public string Provincia { get; set; } = string.Empty;

    [Required(ErrorMessage = "La dirección es obligatoria")]
    [StringLength(200)]
    [Display(Name = "Dirección")]
    public string Direccion { get; set; } = string.Empty;

    [StringLength(20)]
    [Display(Name = "Teléfono")]
    public string? Telefono { get; set; }

    public bool Activa { get; set; } = true;
}
