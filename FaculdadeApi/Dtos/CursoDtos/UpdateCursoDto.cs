using System.ComponentModel.DataAnnotations;

namespace FaculdadeApi.Dtos.CursoDtos;

public class UpdateCursoDto
{
    [Required]
    [MaxLength(70)]
    public string Nome { get; set; }
    [Required]
    public string Descricao { get; set; }
    [Required]
    [Range(4, 10, ErrorMessage = "A quantidade de semestres válida é de 4 e 10")]
    public int QntSemestres { get; set; }
}
