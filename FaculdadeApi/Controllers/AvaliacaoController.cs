using FaculdadeApi.Dtos.AvaliacaoDtos;
using FaculdadeApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FaculdadeApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AvaliacaoController : ControllerBase
{
    private readonly AvaliacaoService _avaliacaoService;

    public AvaliacaoController(AvaliacaoService avaliacaoService)
    {
        _avaliacaoService = avaliacaoService;
    }

    /// <summary>
    /// Cadastra uma nova avaliação no sistema.
    /// </summary>
    /// <param name="dto">
    /// Objeto contendo os dados necessários para a criação da avaliação.
    /// </param>
    /// <returns>
    /// Retorna a avaliação recém-criada.
    /// </returns>
    /// <response code="201">Avaliação criada com sucesso.</response>
    /// <response code="400">Dados inválidos para criação da avaliação.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ReadAvaliacaoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAvaliacao(CreateAvaliacaoDto dto)
    {
        var avaliacao = await _avaliacaoService.Create(dto);
        if (avaliacao is null) return BadRequest();
        return CreatedAtAction(nameof(GetAvaliacaoById), new { Id = avaliacao.Id }, avaliacao);
    }

    /// <summary>
    /// Lista todas as avaliações cadastradas.
    /// </summary>
    /// <returns>
    /// Retorna uma lista de avaliações.
    /// </returns>
    /// <response code="200">Lista de avaliações retornada com sucesso.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ReadAvaliacaoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var avaliacoes = await _avaliacaoService.GetAll(); 
        return Ok(avaliacoes ?? []);
    }

    /// <summary>
    /// Busca uma avaliação pelo identificador.
    /// </summary>
    /// <param name="id">Identificador da avaliação.</param>
    /// <returns>
    /// Retorna os dados da avaliação encontrada.
    /// </returns>
    /// <response code="200">Avaliação encontrada.</response>
    /// <response code="404">Avaliação não encontrada.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ReadAvaliacaoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAvaliacaoById(int id)
    {
        var avaliacao = await _avaliacaoService.GetById(id);
        if (avaliacao is null) return NotFound();
        return Ok(avaliacao);
    }

    /// <summary>
    /// Remove uma avaliação pelo identificador.
    /// </summary>
    /// <param name="id">Identificador da avaliação.</param>
    /// <returns>
    /// Não retorna conteúdo.
    /// </returns>
    /// <response code="204">Avaliação removida com sucesso.</response>
    /// <response code="404">Avaliação não encontrada.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAvaliacaoById(int id)
    {
        var linhasDeletadas = await _avaliacaoService.DeleteById(id);
        if(linhasDeletadas == 0) return NotFound();
        return NoContent();
    }

    /// <summary>
    /// Atualiza os dados de uma avaliação existente.
    /// </summary>
    /// <param name="id">Identificador da avaliação.</param>
    /// <param name="dto">Objeto contendo os dados atualizados da avaliação.</param>
    /// <returns>
    /// Retorna a avaliação atualizada.
    /// </returns>
    /// <response code="200">Avaliação atualizada com sucesso.</response>
    /// <response code="404">Avaliação não encontrada.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ReadAvaliacaoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAvaliacaoById(int id, [FromBody] UpdateAvaliacaoDto dto)
    {
        var avaliacao = await _avaliacaoService.UpdateById(id, dto);
        if (avaliacao is null) return NotFound();
        return Ok(avaliacao);
    }
}
