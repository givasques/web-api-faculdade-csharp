using FaculdadeApi.Dtos.MateriaDtos;
using FaculdadeApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FaculdadeApi.Controllers;

[ApiController]
[Route("[controller]")]
public class MateriaController : ControllerBase
{
    private readonly MateriaService _materiaService;

    public MateriaController(MateriaService materiaService)
    {
        _materiaService = materiaService;
    }

    /// <summary>
    /// Cadastra uma nova matéria no sistema.
    /// </summary>
    /// <param name="dto">
    /// Objeto contendo os dados necessários para a criação da matéria.
    /// </param>
    /// <returns>
    /// Retorna a matéria recém-criada.
    /// </returns>
    /// <response code="201">Matéria criada com sucesso.</response>
    /// <response code="400">Dados inválidos para criação da matéria.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ReadMateriaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateMateria([FromBody] CreateMateriaDto dto)
    {
        var materia = await _materiaService.Create(dto);
        if (materia is null) return BadRequest();
        return CreatedAtAction(nameof(GetMateriaById), new { Id = materia.Id }, materia);
    }

    /// <summary>
    /// Lista todas as matérias cadastradas com paginação.
    /// </summary>
    /// <param name="offSet">Quantidade de registros a serem ignorados.</param>
    /// <param name="limit">Quantidade máxima de registros retornados.</param>
    /// <returns>
    /// Retorna uma lista de matérias.
    /// </returns>
    /// <response code="200">Lista de matérias retornada com sucesso.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ReadMateriaDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] int offSet = 0, [FromQuery] int limit = 10)
    {
        var materias = await _materiaService.GetAll(offSet, limit);
        return Ok(materias ?? []);
    }

    /// <summary>
    /// Busca uma matéria pelo identificador.
    /// </summary>
    /// <param name="id">Identificador da matéria.</param>
    /// <returns>
    /// Retorna os dados da matéria encontrada.
    /// </returns>
    /// <response code="200">Matéria encontrada.</response>
    /// <response code="404">Matéria não encontrada.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ReadMateriaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMateriaById(int id)
    {
        var materia = await _materiaService.GetById(id);
        if (materia is null) return NotFound();
        return Ok(materia);
    }

    /// <summary>
    /// Remove uma matéria pelo identificador.
    /// </summary>
    /// <param name="id">Identificador da matéria.</param>
    /// <returns>
    /// Não retorna conteúdo.
    /// </returns>
    /// <response code="204">Matéria removida com sucesso.</response>
    /// <response code="404">Matéria não encontrada.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteMateriaById(int id)
    {
        var linhasDeletadas = await _materiaService.DeleteById(id);
        if (linhasDeletadas == 0) return NotFound();
        return NoContent();
    }

    /// <summary>
    /// Atualiza os dados de uma matéria existente.
    /// </summary>
    /// <param name="id">Identificador da matéria.</param>
    /// <param name="dto">Objeto contendo os dados atualizados da matéria.</param>
    /// <returns>
    /// Retorna a matéria atualizada.
    /// </returns>
    /// <response code="200">Matéria atualizada com sucesso.</response>
    /// <response code="404">Matéria não encontrada.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ReadMateriaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateMateriaById(int id, [FromBody] UpdateMateriaDto dto)
    {
        var materia = await _materiaService.UpdateById(id, dto);
        if (materia is null) return NotFound();
        return Ok(materia);
    }
}
