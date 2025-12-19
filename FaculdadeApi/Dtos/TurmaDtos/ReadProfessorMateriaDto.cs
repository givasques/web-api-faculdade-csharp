using FaculdadeApi.Dtos.MateriaDtos;
using FaculdadeApi.Dtos.ProfessorDtos;

namespace FaculdadeApi.Dtos.TurmaDtos;

public class ReadProfessorMateriaDto
{
    public ReadProfessorDto Professor { get; set; }
    public ReadMateriaDto Materia { get; set; }
}
