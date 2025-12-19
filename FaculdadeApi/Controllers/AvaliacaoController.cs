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

    [HttpPost]
    public async Task<IActionResult> CreateAvaliacao(CreateAvaliacaoDto dto)
    {
        var avaliacao = await _avaliacaoService.Create(dto);
        if (avaliacao is null) return BadRequest();
        return CreatedAtAction(nameof(GetAvaliacaoById), new { Id = avaliacao.Id }, avaliacao);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var avaliacoes = await _avaliacaoService.GetAll(); 
        return Ok(avaliacoes ?? []);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAvaliacaoById(int id)
    {
        var avaliacao = await _avaliacaoService.GetById(id);
        if (avaliacao is null) return NotFound();
        return Ok(avaliacao);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAvaliacaoById(int id)
    {
        var linhasDeletadas = await _avaliacaoService.DeleteById(id);
        if(linhasDeletadas == 0) return NotFound();
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAvaliacaoById(int id, [FromBody] UpdateAvaliacaoDto dto)
    {
        var avaliacao = await _avaliacaoService.UpdateById(id, dto);
        if (avaliacao is null) return NotFound();
        return Ok(avaliacao);
    }
}
