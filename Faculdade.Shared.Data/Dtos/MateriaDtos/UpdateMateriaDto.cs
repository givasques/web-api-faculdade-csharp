using System.ComponentModel.DataAnnotations;

namespace Faculdade.Shared.Data.Dtos.MateriaDtos;

public class UpdateMateriaDto
{
    [Required]
    [MaxLength(50)]
    public string Nome { get; set; }
    [Required]
    public string Descricao { get; set; }
}
