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

    [HttpPost]
    public async Task<IActionResult> CreateTurma([FromBody] CreateTurmaDto dto)
    {
        var turma = await _turmaService.Create(dto);
        if (turma is null) return BadRequest();
        return CreatedAtAction(nameof(GetTurmaById), new { Id = dto.Id }, turma);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int offSet = 0, [FromQuery] int limit = 10)
    {
        var turmas = await _turmaService.GetAll(offSet, limit);
        return Ok(turmas ?? Enumerable.Empty<ReadTurmaDto>());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTurmaById(string id)
    {
        var turma = await _turmaService.GetById(id);
        if (turma is null) return NotFound();
        return Ok(turma);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTurmaById(string id)
    {
        var linhasDeletadas = await _turmaService.DeleteById(id);
        if(linhasDeletadas == 0) return NotFound();
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTurmaById(string id, [FromBody] UpdateTurmaDto dto)
    {
        var turma = await _turmaService.UpdateById(id, dto);
        if (turma is null) return NotFound();
        return Ok(turma);
    }

    [HttpGet("{id}/materias")]
    public async Task<IActionResult> GetMateriasById(string id)
    {
        var materias = await _turmaService.GetMateriasById(id);
        if (materias is null) return NotFound();
        return Ok(materias);
    }
}
