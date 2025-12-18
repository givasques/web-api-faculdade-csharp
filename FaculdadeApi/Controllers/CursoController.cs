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

    [HttpPost]
    public async Task<IActionResult> CreateCurso([FromBody] CreateCursoDto dto)
    {
        var curso = await _cursoService.Create(dto);
        if (curso is null) return BadRequest();
        return CreatedAtAction(nameof(GetCursoById), new { Id = curso.Id }, curso);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int offSet = 0, [FromQuery] int limit = 10)
    {
        var cursos = await _cursoService.GetAll(offSet, limit);
        return Ok(cursos ?? Enumerable.Empty<ReadCursoDto>());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCursoById(int id)
    {
        var curso = await _cursoService.GetById(id);
        if (curso is null) return NotFound();
        return Ok(curso);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCursoById(int id)
    {
        var linhasDeletadas = await _cursoService.DeleteById(id);
        if (linhasDeletadas == 0) return NotFound();
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCursoById(int id, [FromBody] UpdateCursoDto dto)
    {
        var curso = await _cursoService.UpdateById(id, dto);
        if (curso is null) return NotFound();
        return Ok(curso);
    }

    [HttpPost("{id}/materias")]
    public async Task<IActionResult> AddMateriaAGrade(int id, [FromBody] AddMateriaAGradeDto materiaDto)
    {
        var gradeCurso = await _cursoService.AddMateriaAGrade(id, materiaDto);
        if (gradeCurso is null) return BadRequest();
        return Ok(gradeCurso);

    }

    [HttpGet("{id}/materias")]
    public async Task<IActionResult> GetGradePorId(int id)
    {
        var grade = await _cursoService.GetGradePorId(id);
        if (grade is null) return NotFound();
        return Ok(grade);
    }

    [HttpDelete("{idCurso}/materias/{idMateria}")]
    public async Task<IActionResult> DeleteMateriaDaGrade(int idCurso, int idMateria)
    {
        var linhasDeletadas = await _cursoService.DeleteMateriaDaGrade(idCurso, idMateria);
        if (linhasDeletadas == 0) return NotFound();
        return NoContent();
    }
}
