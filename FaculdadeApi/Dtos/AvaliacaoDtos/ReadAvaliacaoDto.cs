using FaculdadeApi.Dtos.MateriaDtos;
using FaculdadeApi.Dtos.TurmaDtos;

namespace FaculdadeApi.Dtos.AvaliacaoDtos;

public class ReadAvaliacaoDto
{
    public int Id { get; set; }
    public ReadTurmaDto Turma { get; set; }
    public ReadMateriaDto Materia { get; set; }
    public DateOnly DataAplicacao { get; set; }
    public int NotaMaxima { get; set; }
}
