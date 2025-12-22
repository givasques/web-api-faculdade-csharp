using FaculdadeApi.Dtos.TurmaDtos;
using FaculdadeApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FaculdadeApi.Controllers;

[ApiController]
[Route("[controller]")]
public class TurmaController : ControllerBase
{
    private readonly TurmaService _turmaService;

    public TurmaController(TurmaService turmaService)
    {
        _turmaService = turmaService;
    }

    /// <summary>
    /// Cadastra uma nova turma no sistema.
    /// </summary>
    /// <param name="dto">
    /// Objeto contendo os dados necessários para a criação da turma.
    /// </param>
    /// <returns>
    /// Retorna a turma recém-criada.
    /// </returns>
    /// <response code="201">Turma criada com sucesso.</response>
    /// <response code="400">Dados inválidos para criação da turma.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ReadTurmaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateTurma([FromBody] CreateTurmaDto dto)
    {
        var turma = await _turmaService.Create(dto);
        if (turma is null) return BadRequest();
        return CreatedAtAction(nameof(GetTurmaById), new { Id = dto.Id }, turma);
    }

    /// <summary>
    /// Lista todas as turmas cadastradas com paginação.
    /// </summary>
    /// <param name="offSet">Quantidade de registros a serem ignorados.</param>
    /// <param name="limit">Quantidade máxima de registros retornados.</param>
    /// <returns>
    /// Retorna uma lista de turmas.
    /// </returns>
    /// <response code="200">Lista de turmas retornada com sucesso.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ReadTurmaDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] int offSet = 0, [FromQuery] int limit = 10)
    {
        var turmas = await _turmaService.GetAll(offSet, limit);
        return Ok(turmas ?? []);
    }

    /// <summary>
    /// Busca uma turma pelo identificador.
    /// </summary>
    /// <param name="id">Identificador da turma.</param>
    /// <returns>
    /// Retorna os dados da turma encontrada.
    /// </returns>
    /// <response code="200">Turma encontrada.</response>
    /// <response code="404">Turma não encontrada.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ReadTurmaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTurmaById(string id)
    {
        var turma = await _turmaService.GetById(id);
        if (turma is null) return NotFound();
        return Ok(turma);
    }

    /// <summary>
    /// Lista as matérias ministradas em uma turma.
    /// </summary>
    /// <param name="id">Identificador da turma.</param>
    /// <returns>
    /// Retorna a turma e suas matérias ministradas.
    /// </returns>
    /// <response code="200">Lista de matérias retornada com sucesso.</response>
    /// <response code="404">Turma não encontrada.</response>
    [HttpGet("{id}/materias")]
    [ProducesResponseType(typeof(ReadMateriasDaTurmaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMateriasById(string id)
    {
        var materias = await _turmaService.GetMateriasById(id);
        if (materias is null) return NotFound();
        return Ok(materias);
    }

    /// <summary>
    /// Lista as avaliações associadas a uma turma.
    /// </summary>
    /// <param name="id">Identificador da turma.</param>
    /// <returns>
    /// Retorna as avaliações da turma.
    /// </returns>
    /// <response code="200">Lista de avaliações retornada com sucesso.</response>
    /// <response code="404">Turma não encontrada.</response>
    [HttpGet("{id}/avaliacoes")]
    [ProducesResponseType(typeof(ReadAvaliacoesPorTurmaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAvaliacoesById(string id)
    {
        var avaliacoes = await _turmaService.GetAvaliacoesById(id);
        if (avaliacoes is null) return NotFound();
        return Ok(avaliacoes);
    }

    /// <summary>
    /// Remove uma turma pelo identificador.
    /// </summary>
    /// <param name="id">Identificador da turma.</param>
    /// <returns>
    /// Não retorna conteúdo.
    /// </returns>
    /// <response code="204">Turma removida com sucesso.</response>
    /// <response code="404">Turma não encontrada.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTurmaById(string id)
    {
        var linhasDeletadas = await _turmaService.DeleteById(id);
        if(linhasDeletadas == 0) return NotFound();
        return NoContent();
    }

    /// <summary>
    /// Atualiza os dados de uma turma existente.
    /// </summary>
    /// <param name="id">Identificador da turma.</param>
    /// <param name="dto">Objeto contendo os dados atualizados da turma.</param>
    /// <returns>
    /// Retorna a turma atualizada.
    /// </returns>
    /// <response code="200">Turma atualizada com sucesso.</response>
    /// <response code="404">Turma não encontrada.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ReadTurmaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateTurmaById(string id, [FromBody] UpdateTurmaDto dto)
    {
        var turma = await _turmaService.UpdateById(id, dto);
        if (turma is null) return NotFound();
        return Ok(turma);
    }
}
