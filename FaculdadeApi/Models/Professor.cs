using System.ComponentModel.DataAnnotations;

namespace FaculdadeApi.Models;

public class Professor
{
    protected Professor () { }
    public Professor(int id, string nome, string cpf, string telefone, string email)
    {
        Id = id;
        Nome = nome;
        Cpf = cpf;
        Telefone = telefone;
        Email = email;
    }

    [Required]
    public int Id { get; private set; }
    [Required]
    [MaxLength(70)]
    public string Nome { get; private set; }
    [Required]
    [StringLength(11, MinimumLength = 11)]
    public string Cpf { get; private set; }
    [Required]
    [RegularExpression(@"^\d{10,11}$", ErrorMessage = "Telefone inválido")]
    public string Telefone { get; private set; }
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; private set; }
}
