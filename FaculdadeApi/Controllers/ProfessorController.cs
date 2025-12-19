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

    [HttpPost]
    public async Task<IActionResult> CreateProfessor([FromBody] CreateProfessorDto dto)
    {
        var professor = await _professorService.Create(dto);
        if (professor is null) return BadRequest();
        return CreatedAtAction(nameof(GetProfessorById), new { Id = professor.Id }, professor);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int offSet = 0, [FromQuery] int limit = 10)
    {
        var professores = await _professorService.GetAll(offSet, limit);
        return Ok(professores ?? Enumerable.Empty<ReadProfessorDto>());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProfessorById(int id)
    {
        var professor = await _professorService.GetById(id);
        if (professor is null) return NotFound();
        return Ok(professor);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProfessorById(int id)
    {
        var linhasDeletadas = await _professorService.DeleteById(id);
        if (linhasDeletadas == 0) return NotFound();
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProfessorById(int id, [FromBody] UpdateProfessorDto dto)
    {
        var professor = await _professorService.UpdateById(id, dto);
        if (professor is null) return NotFound();
        return Ok(professor);
    }

    [HttpGet("{id}/materias")]
    public async Task<IActionResult> GetMateriasMinistradasById (int id)
    {
        var materias = await _professorService.GetMateriasMinistradasById(id);
        if (materias is null) return NotFound();
        return Ok(materias);
    } 
}
