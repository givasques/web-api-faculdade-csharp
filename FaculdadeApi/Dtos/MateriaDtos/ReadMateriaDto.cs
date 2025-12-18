using System.ComponentModel.DataAnnotations;

namespace FaculdadeApi.Dtos.MateriaDtos;

public class ReadMateriaDto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
}
