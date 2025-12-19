namespace FaculdadeApi.Dtos.TurmaDtos;

public class ReadMateriasDaTurmaDto
{
    public ReadTurmaDto Turma { get; set; }
    public IEnumerable<ReadProfessorMateriaDto> Materias { get; set; }
}
