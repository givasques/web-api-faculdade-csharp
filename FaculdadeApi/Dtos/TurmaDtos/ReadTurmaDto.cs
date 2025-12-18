using FaculdadeApi.Models;
using System.ComponentModel.DataAnnotations;

namespace FaculdadeApi.Dtos.TurmaDtos;

public class ReadTurmaDto
{
    public string Id { get; set; }
    public int IdCurso { get; set; }
    public string Periodo { get; set; }
    public string Formato { get; set; }
}
