using System.ComponentModel.DataAnnotations;

namespace FaculdadeApi.Dtos.ProfessorDtos;

public class CreateProfessorDto
{
    [Required]
    [MaxLength(70)]
    public string Nome { get; set; }
    [Required]
    [StringLength(11, MinimumLength = 11)]
    public string Cpf { get; set; }
    [Required]
    [RegularExpression(@"^\d{10,11}$", ErrorMessage = "Telefone inválido")]
    public string Telefone { get; set; }
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
}
