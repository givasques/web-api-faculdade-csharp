using Faculdade.Shared.Data.Dtos.ProfessorDtos;
using System.Net.Http.Json;

namespace Faculdade.WebAssembly.Services
{
    public class ProfessoresAPI
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ProfessoresAPI> _logger;

        public ProfessoresAPI(IHttpClientFactory factory, ILogger<ProfessoresAPI> logger)
        {
            _httpClient = factory.CreateClient("API");
            _logger = logger;
        }

        public async Task<ICollection<ReadProfessorDto>> GetProfessores(int offset, int limit)
        {
            try
            {
                _logger.LogInformation("Consultando professores no banco de dados... ");
                var professores = await _httpClient.GetFromJsonAsync<ICollection<ReadProfessorDto>>($"Professor?offSet={offset}&limit={limit}");
                _logger.LogInformation("Consulta realizada com sucesso.");
                return professores;
            }
            catch (Exception e)
            {
                string mensagem = "Erro ao buscar professores.";
                _logger.LogError(mensagem, e);
                throw new ApplicationException(mensagem, e);
            }
        }

        public async Task<UpdateProfessorDto> GetProfessorByIdToUpdate(int id)
        {
            try
            {
                _logger.LogInformation($"Buscando professor com ID {id} no banco de dados... ");
                var professor = await _httpClient.GetFromJsonAsync<UpdateProfessorDto>($"Professor/{id}");
                _logger.LogInformation("Consulta realizada com sucesso.");
                return professor;
            }
            catch (Exception e)
            {
                string mensagem = "Erro ao buscar professor.";
                _logger.LogError(mensagem, e);
                throw new ApplicationException(mensagem, e);
            }
        }

        public async Task UpdateProfessor(int id, UpdateProfessorDto professor)
        {
            try
            {
                _logger.LogInformation($"Atualizando professor com ID {id} no banco de dados...");

                var response = await _httpClient.PutAsJsonAsync($"Professor/{id}", professor);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Professor atualizado com sucesso!");
                }
                else
                {
                    var erro = response.Content.ReadAsStringAsync();

                    _logger.LogError($"Erro HTTP {response.StatusCode} ao atualizar professor: {erro}");

                    throw new ApplicationException($"Erro ao atualizar professor. Status: {response.StatusCode} - Erro: {erro}");
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}

