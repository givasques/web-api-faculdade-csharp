using System.ComponentModel.DataAnnotations;

namespace FaculdadeApi.Models;

public class Turma
{
    protected Turma() { }
    public Turma(string id, int idCurso, string periodo, string formato)
    {
        Id = id;
        IdCurso = idCurso;
        Periodo = periodo;
        Formato = formato;
    }

    [Required]
    public string Id { get; private set; }
    [Required]
    public int IdCurso { get; private set; }
    [Required]
    [AllowedValues("Noturno", "Matutino", ErrorMessage = "Os 2 períodos válidos são: Noturno e Matutino")]
    public string Periodo { get; private set; }
    [Required]
    [AllowedValues("EAD", "Presencial", "Semi-Presencial", ErrorMessage = "Os 3 formatos válidos são: EAD, Presencial e Semi-Presencial")]
    public string Formato { get; private set; }
}
