using System.ComponentModel.DataAnnotations;

namespace FaculdadeApi.Dtos.AvaliacaoDtos;

public class UpdateAvaliacaoDto
{
    [Required]
    public DateOnly DataAplicacao { get; set; }
    [Required]
    [Range(1, 10, ErrorMessage = "Os valores permitidos para nota máxima da avaliação são de 0-10")]
    public int NotaMaxima { get; set; }
}
