using Faculdade.Shared.Data.Dtos.TurmaDtos;

namespace Faculdade.Shared.Data.Dtos.ProfessorDtos;

public class ReadMateriasMinistradasDto
{
    public ReadProfessorDto Professor { get; set; }
    public IEnumerable<ReadTurmaMateriaDto> Materias { get; set; }

}
