namespace FaculdadeApi.Dtos.AlunoDtos;

public class ReadAlunoDto
{
    public int Rm { get; set; }
    public string Email { get; set; }
    public string Cpf { get; set; }
    public string Nome { get; set; }
    public DateOnly DataNascimento { get; set; }
    public string IdTurma { get; set; }
}
