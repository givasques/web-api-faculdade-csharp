using FaculdadeApi.Dtos.AvaliacaoDtos;

namespace FaculdadeApi.Dtos.AlunoDtos;

public class ReadProvaAvaliadaDto
{
    public ReadAvaliacaoSimplificadaDto Avaliacao { get; set; }
    public int Nota { get; set; }
}
