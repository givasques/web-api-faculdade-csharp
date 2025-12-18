using System.ComponentModel.DataAnnotations;

namespace FaculdadeApi.Dtos.CursoDtos;

public class AddMateriaAGradeDto
{
    [Required]
    public int IdMateria { get; set; }
    [Required]
    [Range(30, 160, ErrorMessage = "O valor mínimo para a carga horária é 30h e máximo é 160h")]
    public int CargaHoraria { get; set; }
}
