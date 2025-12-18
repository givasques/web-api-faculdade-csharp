namespace FaculdadeApi.Dtos.CursoDtos;

public class ReadGradeCursoDto
{
    public ReadCursoDto Curso { get; set; }
    public IEnumerable<ReadMateriaGradeDto> Materias { get; set; }
}
