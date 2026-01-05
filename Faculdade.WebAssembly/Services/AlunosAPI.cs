using Faculdade.Shared.Data.Dtos.AlunoDtos;
using System.Collections.Generic;
using System.Net.Http.Json;

namespace Faculdade.WebAssembly.Services
{
    public class AlunosAPI
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AlunosAPI> _logger;

        public AlunosAPI(IHttpClientFactory factory, ILogger<AlunosAPI> logger)
        {
            _httpClient = factory.CreateClient("API");
            _logger = logger;
        }

        public async Task<ICollection<ReadAlunoDto>> GetAlunos(int offset, int limit)
        {
            try
            {
                _logger.LogInformation("Consultando alunos no banco de dados... ");
                var alunos = await _httpClient.GetFromJsonAsync<ICollection<ReadAlunoDto>>($"Aluno?offSet={offset}&limit={limit}");
                _logger.LogInformation("Consulta realizada com sucesso.");
                return alunos;
            }
            catch (Exception e)
            {
                string mensagem = "Erro ao buscar alunos.";
                _logger.LogError(mensagem, e);
                throw new ApplicationException(mensagem, e);
            }
        }

        public async Task<UpdateAlunoDto> GetAlunoByRmToUpdate (int rm)
        {
            try
            {
                _logger.LogInformation($"Buscando aluno com RM {rm} no banco de dados... ");
                var aluno = await _httpClient.GetFromJsonAsync<UpdateAlunoDto>($"Aluno/{rm}");
                _logger.LogInformation("Consulta realizada com sucesso.");
                return aluno;
            }
            catch (Exception e)
            {
                string mensagem = "Erro ao buscar aluno.";
                _logger.LogError(mensagem, e);
                throw new ApplicationException(mensagem, e);
            }
        }

        public async Task UpdateAluno (int rm, UpdateAlunoDto aluno)
        {
            try
            {
                _logger.LogInformation($"Atualizando aluno RM {rm} no banco de dados...");

                var response = await _httpClient.PutAsJsonAsync($"Aluno/{rm}", aluno);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Aluno atualizado com sucesso!");
                }
                else
                {
                    var erro = response.Content.ReadAsStringAsync();

                    _logger.LogError($"Erro HTTP {response.StatusCode} ao atualizar aluno: {erro}");

                    throw new ApplicationException($"Erro ao atualizar aluno. Status: {response.StatusCode} - Erro: {erro}");
                }
                
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
