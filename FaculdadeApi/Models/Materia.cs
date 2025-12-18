using System.ComponentModel.DataAnnotations;

namespace FaculdadeApi.Models;

public class Materia
{
    protected Materia () { }
    public Materia(int id, string nome, string descricao)
    {
        Id = id;
        Nome = nome;
        Descricao = descricao;
    }

    [Required]
    public int Id { get; private set; }
    [Required]
    [MaxLength(50)]
    public string Nome { get; private set; }
    [Required]
    public string Descricao { get; private set; }
}
