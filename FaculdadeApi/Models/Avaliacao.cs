using System.ComponentModel.DataAnnotations;

namespace FaculdadeApi.Models;

public class Avaliacao
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string IdTurma { get; set; }
    [Required]
    public int IdMateria { get; set; }
    [Required]
    public DateOnly DataAplicacao { get; set; }
    [Required]
    [Range(1, 10, ErrorMessage = "Os valores permitidos para nota máxima da avaliação são de 0-10")]
    public int NotaMaxima { get; set; }
}
