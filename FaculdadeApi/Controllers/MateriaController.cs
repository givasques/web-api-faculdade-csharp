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

    [HttpPost]
    public async Task<IActionResult> CreateMateria([FromBody] CreateMateriaDto dto)
    {
        var materia = await _materiaService.Create(dto);
        if (materia is null) return BadRequest();
        return CreatedAtAction(nameof(GetMateriaById), new { Id = materia.Id }, materia);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int offSet = 0, [FromQuery] int limit = 10)
    {
        var materias = await _materiaService.GetAll(offSet, limit);
        return Ok(materias ?? Enumerable.Empty<ReadMateriaDto>());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMateriaById(int id)
    {
        var materia = await _materiaService.GetById(id);
        if (materia is null) return NotFound();
        return Ok(materia);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMateriaById(int id)
    {
        var linhasDeletadas = await _materiaService.DeleteById(id);
        if (linhasDeletadas == 0) return NotFound();
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMateriaById(int id, [FromBody] UpdateMateriaDto dto)
    {
        var materia = await _materiaService.UpdateById(id, dto);
        if (materia is null) return NotFound();
        return Ok(materia);
    }
}
