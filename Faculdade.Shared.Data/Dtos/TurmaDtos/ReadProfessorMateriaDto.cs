using Faculdade.Shared.Data.Dtos.MateriaDtos;
using Faculdade.Shared.Data.Dtos.ProfessorDtos;

namespace Faculdade.Shared.Data.Dtos.TurmaDtos;

public class ReadProfessorMateriaDto
{
    public ReadProfessorDto Professor { get; set; }
    public ReadMateriaDto Materia { get; set; }
}
