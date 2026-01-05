namespace Faculdade.Shared.Data.Dtos.TurmaDtos;

public class ReadMateriasDaTurmaDto
{
    public ReadTurmaDto Turma { get; set; }
    public IEnumerable<ReadProfessorMateriaDto> Materias { get; set; }
}
