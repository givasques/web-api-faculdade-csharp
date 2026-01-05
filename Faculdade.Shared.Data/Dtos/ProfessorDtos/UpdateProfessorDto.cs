using System.ComponentModel.DataAnnotations;

namespace Faculdade.Shared.Data.Dtos.ProfessorDtos;

public class UpdateProfessorDto
{
    [Required]
    [MaxLength(70)]
    public string Nome { get; set; }
    [Required]
    [StringLength(11, MinimumLength = 11)]
    public string Cpf { get; set; }
    [Required]
    [RegularExpression(@"^(\(?\d{2}\)?\s?)?9\d{4}-?\d{4}$", ErrorMessage = "Telefone inválido - tente o formato (xx) xxxxx-xxxx")]
    public string Telefone { get; set; }
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
}
