using System.ComponentModel.DataAnnotations;

namespace FaculdadeApi.Dtos.ProfessorDtos;

public class ReadProfessorDto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Cpf { get; set; }
    public string Telefone { get; set; }
    public string Email { get; set; }
}
