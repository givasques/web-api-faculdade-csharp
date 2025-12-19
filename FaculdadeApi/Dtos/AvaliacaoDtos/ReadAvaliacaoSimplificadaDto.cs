namespace FaculdadeApi.Dtos.AvaliacaoDtos;

public class ReadAvaliacaoSimplificadaDto
{
    public int Id { get; set; }
    public string idTurma { get; set; }
    public string NomeMateria { get; set; }
    public DateOnly DataAplicacao { get; set; }
    public int NotaMaxima { get; set; }
}
