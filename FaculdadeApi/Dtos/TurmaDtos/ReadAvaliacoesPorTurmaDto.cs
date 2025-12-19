using FaculdadeApi.Dtos.AvaliacaoDtos;

namespace FaculdadeApi.Dtos.TurmaDtos;

public class ReadAvaliacoesPorTurmaDto
{
    public string IdTurma { get; set; }
    public IEnumerable<ReadAvaliacaoSimplificadaDto> Avaliacoes { get; set; }
}
