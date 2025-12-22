using FaculdadeApi.Dtos.CursoDtos;
using FaculdadeApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FaculdadeApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CursoController : ControllerBase
{
    private readonly CursoService _cursoService;

    public CursoController(CursoService cursoService)
    {
        _cursoService = cursoService;
    }

    /// <summary>
    /// Cadastra um novo curso no sistema.
    /// </summary>
    /// <param name="dto">
    /// Objeto contendo os dados necessários para a criação do curso.
    /// </param>
    /// <returns>
    /// Retorna o curso recém-criado.
    /// </returns>
    /// <response code="201">Curso criado com sucesso.</response>
    /// <response code="400">Dados inválidos para criação do curso.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ReadCursoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCurso([FromBody] CreateCursoDto dto)
    {
        var curso = await _cursoService.Create(dto);
        if (curso is null) return BadRequest();
        return CreatedAtAction(nameof(GetCursoById), new { Id = curso.Id }, curso);
    }

    /// <summary>
    /// Lista todos os cursos cadastrados com paginação.
    /// </summary>
    /// <param name="offSet">Quantidade de registros a serem ignorados.</param>
    /// <param name="limit">Quantidade máxima de registros retornados.</param>
    /// <returns>
    /// Retorna uma lista de cursos.
    /// </returns>
    /// <response code="200">Lista de cursos retornada com sucesso.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ReadCursoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] int offSet = 0, [FromQuery] int limit = 10)
    {
        var cursos = await _cursoService.GetAll(offSet, limit);
        return Ok(cursos ?? []);
    }

    /// <summary>
    /// Busca um curso pelo identificador.
    /// </summary>
    /// <param name="id">Identificador do curso.</param>
    /// <returns>
    /// Retorna os dados do curso encontrado.
    /// </returns>
    /// <response code="200">Curso encontrado.</response>
    /// <response code="404">Curso não encontrado.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ReadCursoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCursoById(int id)
    {
        var curso = await _cursoService.GetById(id);
        if (curso is null) return NotFound();
        return Ok(curso);
    }

    /// <summary>
    /// Remove um curso pelo identificador.
    /// </summary>
    /// <param name="id">Identificador do curso.</param>
    /// <returns>
    /// Não retorna conteúdo.
    /// </returns>
    /// <response code="204">Curso removido com sucesso.</response>
    /// <response code="404">Curso não encontrado.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCursoById(int id)
    {
        var linhasDeletadas = await _cursoService.DeleteById(id);
        if (linhasDeletadas == 0) return NotFound();
        return NoContent();
    }

    /// <summary>
    /// Atualiza os dados de um curso existente.
    /// </summary>
    /// <param name="id">Identificador do curso.</param>
    /// <param name="dto">Objeto contendo os dados atualizados do curso.</param>
    /// <returns>
    /// Retorna o curso atualizado.
    /// </returns>
    /// <response code="200">Curso atualizado com sucesso.</response>
    /// <response code="404">Curso não encontrado.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ReadCursoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCursoById(int id, [FromBody] UpdateCursoDto dto)
    {
        var curso = await _cursoService.UpdateById(id, dto);
        if (curso is null) return NotFound();
        return Ok(curso);
    }

    /// <summary>
    /// Adiciona uma matéria à grade curricular de um curso.
    /// </summary>
    /// <param name="id">Identificador do curso.</param>
    /// <param name="materiaDto">Identificador da matéria a ser adicionada a grade e carga horária</param>
    /// <returns>
    /// Retorna o curso e a matéria adicionada a sua grade curricular.
    /// </returns>
    /// <response code="200">Matéria adicionada à grade com sucesso.</response>
    /// <response code="400">Erro ao adicionar a matéria à grade.</response>
    [HttpPost("{id}/materias")]
    [ProducesResponseType(typeof(ReadGradeCursoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddMateriaAGrade(int id, [FromBody] AddMateriaAGradeDto materiaDto)
    {
        var gradeCurso = await _cursoService.AddMateriaAGrade(id, materiaDto);
        if (gradeCurso is null) return BadRequest();
        return Ok(gradeCurso);

    }

    /// <summary>
    /// Consulta a grade curricular de um curso pelo identificador.
    /// </summary>
    /// <param name="id">Identificador do curso.</param>
    /// <returns>
    /// Retorna a grade curricular do curso.
    /// </returns>
    /// <response code="200">Grade curricular retornada com sucesso.</response>
    /// <response code="404">Curso não encontrado.</response>
    [HttpGet("{id}/materias")]
    [ProducesResponseType(typeof(ReadGradeCursoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetGradePorId(int id)
    {
        var grade = await _cursoService.GetGradePorId(id);
        if (grade is null) return NotFound();
        return Ok(grade);
    }

    /// <summary>
    /// Remove uma matéria da grade curricular de um curso.
    /// </summary>
    /// <param name="idCurso">Identificador do curso.</param>
    /// <param name="idMateria">Identificador da matéria.</param>
    /// <returns>
    /// Não retorna conteúdo.
    /// </returns>
    /// <response code="204">Matéria removida da grade com sucesso.</response>
    /// <response code="404">Curso ou matéria não encontrados.</response>
    [HttpDelete("{idCurso}/materias/{idMateria}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteMateriaDaGrade(int idCurso, int idMateria)
    {
        var linhasDeletadas = await _cursoService.DeleteMateriaDaGrade(idCurso, idMateria);
        if (linhasDeletadas == 0) return NotFound();
        return NoContent();
    }
}
