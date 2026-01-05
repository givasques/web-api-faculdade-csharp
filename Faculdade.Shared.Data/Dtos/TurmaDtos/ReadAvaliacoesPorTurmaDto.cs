using Faculdade.Shared.Data.Dtos.AvaliacaoDtos;

namespace Faculdade.Shared.Data.Dtos.TurmaDtos;

public class ReadAvaliacoesPorTurmaDto
{
    public string IdTurma { get; set; }
    public IEnumerable<ReadAvaliacaoSimplificadaDto> Avaliacoes { get; set; }
}
