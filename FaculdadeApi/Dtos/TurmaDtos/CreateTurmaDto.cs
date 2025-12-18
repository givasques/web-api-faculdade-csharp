using FaculdadeApi.Models;
using System.ComponentModel.DataAnnotations;

namespace FaculdadeApi.Dtos.TurmaDtos;

public class CreateTurmaDto
{
    [Required]
    public string Id { get; set; }
    [Required]
    public int IdCurso { get; set; }
    [Required]
    [AllowedValues("Noturno", "Matutino", ErrorMessage = "Os 2 períodos válidos são: Noturno e Matutino")]
    public string Periodo { get; set; }
    [Required]
    [AllowedValues("EAD", "Presencial", "Semi-Presencial", ErrorMessage = "Os 3 formatos válidos são: EAD, Presencial e Semi-Presencial")]
    public string Formato { get; set; }
}
