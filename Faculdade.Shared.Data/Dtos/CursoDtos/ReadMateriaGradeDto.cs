using Faculdade.Shared.Data.Dtos.MateriaDtos;
using System.ComponentModel.DataAnnotations;

namespace Faculdade.Shared.Data.Dtos.CursoDtos;

public class ReadMateriaGradeDto
{
    public ReadMateriaDto Materia { get; set; }
    public int CargaHoraria { get; set; }
}
