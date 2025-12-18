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

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var turmas = await _turmaService.GetAll();
        return Ok(turmas ?? Enumerable.Empty<ReadTurmaDto>());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTurmaById(string id)
    {
        var turma = await _turmaService.GetById(id);
        if (turma is null) return NotFound();
        return Ok(turma);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTurma([FromBody] CreateTurmaDto dto)
    {
        var turma = await _turmaService.Create(dto);
        if (turma is null) return BadRequest();
        return CreatedAtAction(nameof(GetTurmaById), new { Id = dto.Id }, turma);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTurmaById(string id)
    {
        var linhasAlteradas = await _turmaService.DeleteById(id);
        if(linhasAlteradas == 0) return NotFound();
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTurmaById(string id, [FromBody] UpdateTurmaDto dto)
    {
        var turma = await _turmaService.UpdateById(id, dto);
        if (turma is null) return NotFound();
        return Ok(turma);
    }
}
