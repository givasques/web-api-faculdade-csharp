using FaculdadeApi.Dtos.TurmaDtos;

namespace FaculdadeApi.Dtos.ProfessorDtos;

public class ReadMateriasMinistradasDto
{
    public ReadProfessorDto Professor { get; set; }
    public IEnumerable<ReadTurmaMateriaDto> Materias { get; set; }

}
