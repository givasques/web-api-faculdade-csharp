namespace FaculdadeApi.Dtos;

public class ReadAlunoDto
{
    public int Rm { get; private set; }
    public string Email { get; private set; }
    public string Cpf { get; private set; }
    public string Nome { get; private set; }
    public DateOnly DataNascimento { get; private set; }
    public string IdTurma { get; private set; }
}
