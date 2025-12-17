using System.ComponentModel.DataAnnotations;

namespace FaculdadeApi.Models;

public class Curso
{
    [Required]
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(30)]
    public string Nome { get; set; }
    [Required]
    public string Descricao { get; set; }
    [Required]
    [Range(4, 10, ErrorMessage = "A quantidade de semestres válida é de 4 e 10")]
    public int QntSemestres { get; set; }
}
