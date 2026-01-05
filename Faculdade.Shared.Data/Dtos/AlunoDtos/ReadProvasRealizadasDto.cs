namespace Faculdade.Shared.Data.Dtos.AlunoDtos
{
    public class ReadProvasRealizadasDto
    {
        public int RmAluno { get; set; }
        public IEnumerable<ReadProvaAvaliadaDto> ProvasRealizadas { get; set; }
    }
}
