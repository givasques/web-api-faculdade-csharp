using Faculdade.Shared.Data.Dtos.MateriaDtos;
using Faculdade.Shared.Data.Dtos.TurmaDtos;

namespace Faculdade.Shared.Data.Dtos.ProfessorDtos;

public class ReadTurmaMateriaDto
{
    public ReadTurmaDto Turma { get; set; }
    public ReadMateriaDto Materia { get; set; }
}
