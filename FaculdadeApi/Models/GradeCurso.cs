using System.ComponentModel.DataAnnotations;

namespace FaculdadeApi.Models;

public class GradeCurso
{
    protected GradeCurso() { }
    public GradeCurso(int idCurso, int idMateria, int cargaHoraria)
    {
        IdCurso = idCurso;
        IdMateria = idMateria;
        CargaHoraria = cargaHoraria;
    }

    [Required]
    public int IdCurso { get; private set; }
    [Required]
    public int IdMateria { get; private set; }
    [Required]
    [Range(30,160, ErrorMessage = "O valor mínimo para a carga horária é 30h e máximo é 160h")]
    public int CargaHoraria { get; private set; }
}
