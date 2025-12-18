using System.ComponentModel.DataAnnotations;

namespace FaculdadeApi.Models;

public class Aluno
{
    protected Aluno() { }
    public Aluno(int rm, string email, string cpf, string nome, DateOnly dataNascimento, string idTurma)
    {
        Rm = rm;
        Email = email;
        Cpf = cpf;
        Nome = nome;
        DataNascimento = dataNascimento;
        IdTurma = idTurma;
    }

    [Required]
    public int Rm { get; private set; }
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
