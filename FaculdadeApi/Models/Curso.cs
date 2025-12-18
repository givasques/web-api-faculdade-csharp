using System.ComponentModel.DataAnnotations;

namespace FaculdadeApi.Models;

public class Curso
{
    protected Curso () { }
    public Curso(int id, string nome, string descricao, int qntSemestres)
    {
        Id = id;
        Nome = nome;
        Descricao = descricao;
        QntSemestres = qntSemestres;
    }

    [Required]
    public int Id { get; private set; }
    [Required]
    [MaxLength(70)]
    public string Nome { get; private set; }
    [Required]
    public string Descricao { get; private set; }
    [Required]
    [Range(4, 10, ErrorMessage = "A quantidade de semestres válida é de 4 e 10")]
    public int QntSemestres { get; private set; }
}
