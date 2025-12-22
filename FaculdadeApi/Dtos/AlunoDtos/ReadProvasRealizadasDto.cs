using FaculdadeApi.Dtos.AvaliacaoDtos;

namespace FaculdadeApi.Dtos.AlunoDtos
{
    public class ReadProvasRealizadasDto
    {
        public int RmAluno { get; set; }
        public IEnumerable<ReadProvaAvaliadaDto> ProvasRealizadas { get; set; }
    }
}
