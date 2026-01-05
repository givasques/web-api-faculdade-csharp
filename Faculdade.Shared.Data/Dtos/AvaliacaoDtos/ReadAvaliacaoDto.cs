using Faculdade.Shared.Data.Dtos.MateriaDtos;
using Faculdade.Shared.Data.Dtos.TurmaDtos;

namespace Faculdade.Shared.Data.Dtos.AvaliacaoDtos;

public class ReadAvaliacaoDto
{
    public int Id { get; set; }
    public ReadTurmaDto Turma { get; set; }
    public ReadMateriaDto Materia { get; set; }
    public DateOnly DataAplicacao { get; set; }
    public int NotaMaxima { get; set; }
}
