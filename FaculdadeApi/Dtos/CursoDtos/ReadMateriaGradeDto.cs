using FaculdadeApi.Dtos.MateriaDtos;
using System.ComponentModel.DataAnnotations;

namespace FaculdadeApi.Dtos.CursoDtos;

public class ReadMateriaGradeDto
{
    public ReadMateriaDto Materia { get; set; }
    public int CargaHoraria { get; set; }
}
