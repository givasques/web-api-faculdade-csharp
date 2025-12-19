using FaculdadeApi.Dtos.MateriaDtos;
using FaculdadeApi.Dtos.TurmaDtos;

namespace FaculdadeApi.Dtos.ProfessorDtos;

public class ReadTurmaMateriaDto
{
    public ReadTurmaDto Turma { get; set; }
    public ReadMateriaDto Materia { get; set; }
}
