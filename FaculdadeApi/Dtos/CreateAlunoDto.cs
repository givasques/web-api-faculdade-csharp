using System.ComponentModel.DataAnnotations;

namespace FaculdadeApi.Dtos;

public class CreateAlunoDto
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; private set; }
    [Required]
    [StringLength(11)]
    public string Cpf { get; private set; }
    [Required]
    [MaxLength(50)]
    public string Nome { get; private set; }
    [Required]
    public DateOnly DataNascimento { get; private set; }
    [Required]
    public string IdTurma { get; private set; }
}
