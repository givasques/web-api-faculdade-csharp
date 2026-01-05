using System.ComponentModel.DataAnnotations;

namespace Faculdade.Shared.Data.Dtos.MateriaDtos;

public class ReadMateriaDto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
}
