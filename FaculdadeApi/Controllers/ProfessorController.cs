using FaculdadeApi.Dtos.ProfessorDtos;
using FaculdadeApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FaculdadeApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ProfessorController : ControllerBase
{
    private readonly ProfessorService _professorService;

    public ProfessorController(ProfessorService professorService)
    {
        _professorService = professorService;
    }

    /// <summary>
    /// Cadastra um novo professor no sistema.
    /// </summary>
    /// <param name="dto">
    /// Objeto contendo os dados necessários para a criação do professor.
    /// </param>
    /// <returns>
    /// Retorna o professor recém-criado.
    /// </returns>
    /// <response code="201">Professor criado com sucesso.</response>
    /// <response code="400">Dados inválidos para criação do professor.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ReadProfessorDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateProfessor([FromBody] CreateProfessorDto dto)
    {
        var professor = await _professorService.Create(dto);
        if (professor is null) return BadRequest();
        return CreatedAtAction(nameof(GetProfessorById), new { Id = professor.Id }, professor);
    }

    /// <summary>
    /// Lista todos os professores cadastrados com paginação.
    /// </summary>
    /// <param name="offSet">Quantidade de registros a serem ignorados.</param>
    /// <param name="limit">Quantidade máxima de registros retornados.</param>
    /// <returns>
    /// Retorna uma lista de professores.
    /// </returns>
    /// <response code="200">Lista de professores retornada com sucesso.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ReadProfessorDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] int offSet = 0, [FromQuery] int limit = 10)
    {
        var professores = await _professorService.GetAll(offSet, limit);
        return Ok(professores ?? []);
    }

    /// <summary>
    /// Busca um professor pelo identificador.
    /// </summary>
    /// <param name="id">Identificador do professor.</param>
    /// <returns>
    /// Retorna os dados do professor encontrado.
    /// </returns>
    /// <response code="200">Professor encontrado.</response>
    /// <response code="404">Professor não encontrado.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ReadProfessorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProfessorById(int id)
    {
        var professor = await _professorService.GetById(id);
        if (professor is null) return NotFound();
        return Ok(professor);
    }

    /// <summary>
    /// Remove um professor pelo identificador.
    /// </summary>
    /// <param name="id">Identificador do professor.</param>
    /// <returns>
    /// Não retorna conteúdo.
    /// </returns>
    /// <response code="204">Professor removido com sucesso.</response>
    /// <response code="404">Professor não encontrado.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProfessorById(int id)
    {
        var linhasDeletadas = await _professorService.DeleteById(id);
        if (linhasDeletadas == 0) return NotFound();
        return NoContent();
    }

    /// <summary>
    /// Atualiza os dados de um professor existente.
    /// </summary>
    /// <param name="id">Identificador do professor.</param>
    /// <param name="dto">Objeto contendo os dados atualizados do professor.</param>
    /// <returns>
    /// Retorna o professor atualizado.
    /// </returns>
    /// <response code="200">Professor atualizado com sucesso.</response>
    /// <response code="404">Professor não encontrado.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ReadProfessorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProfessorById(int id, [FromBody] UpdateProfessorDto dto)
    {
        var professor = await _professorService.UpdateById(id, dto);
        if (professor is null) return NotFound();
        return Ok(professor);
    }

    /// <summary>
    /// Lista as matérias ministradas por um professor.
    /// </summary>
    /// <param name="id">Identificador do professor.</param>
    /// <returns>
    /// Retorna o professor, as respectivas turmas onde ministra e as matérias que são ministradas.
    /// </returns>
    /// <response code="200">Lista de matérias retornada com sucesso.</response>
    /// <response code="404">Professor não encontrado.</response>
    [HttpGet("{id}/materias")]
    [ProducesResponseType(typeof(ReadMateriasMinistradasDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMateriasMinistradasById (int id)
    {
        var materias = await _professorService.GetMateriasMinistradasById(id);
        if (materias is null) return NotFound();
        return Ok(materias);
    } 
}
