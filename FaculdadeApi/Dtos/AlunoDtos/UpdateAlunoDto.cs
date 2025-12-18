using System.ComponentModel.DataAnnotations;

namespace FaculdadeApi.Dtos.AlunoDtos;

public class UpdateAlunoDto
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Cpf { get; set; }
    [Required]
    public string Nome { get; set; }
    [Required]
    public DateOnly DataNascimento { get; set; }
    [Required]
    public string IdTurma { get; set; }
}
