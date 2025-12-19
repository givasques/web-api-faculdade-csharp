using System.ComponentModel.DataAnnotations;

namespace FaculdadeApi.Dtos.AlunoDtos;

public class CreateAlunoDto
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    [Required]
    [StringLength(11, MinimumLength = 11)]
    public string Cpf { get; set; }
    [Required]
    [MaxLength(70)]
    public string Nome { get; set; }
    [Required]
    public DateOnly DataNascimento { get; set; }
    [Required]
    public string IdTurma { get; set; }
}
