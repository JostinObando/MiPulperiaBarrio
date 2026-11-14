using System.ComponentModel.DataAnnotations;

namespace MiPulperiaBarrio.Models.Formularios;

public class ClienteFormulario
{
    public int Id { get; set; }

    [Required(ErrorMessage = "La identificación es obligatoria")]
    [StringLength(20)]
    [Display(Name = "Identificación")]
    public string Identificacion { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(120)]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo es obligatorio")]
    [StringLength(150)]
    [EmailAddress(ErrorMessage = "Ingrese un correo válido")]
    public string Correo { get; set; } = string.Empty;

    [StringLength(20)]
    [Display(Name = "Teléfono")]
    public string? Telefono { get; set; }

    public bool Activo { get; set; } = true;
}
