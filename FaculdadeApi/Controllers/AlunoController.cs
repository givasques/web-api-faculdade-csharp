using FaculdadeApi.Dtos;
using FaculdadeApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FaculdadeApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AlunoController : ControllerBase
{
    private readonly AlunoService _alunoService;

    public AlunoController(AlunoService alunoService)
    {
        _alunoService = alunoService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAluno([FromBody] CreateAlunoDto dto)
    {
        var aluno = await _alunoService.Create(dto);
        if (aluno is null) return BadRequest();
        return CreatedAtAction(nameof(GetAlunoByRm), new { rm = aluno.Rm }, aluno); 
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var alunos = await _alunoService.GetAll();
        return Ok(alunos ?? Enumerable.Empty<ReadAlunoDto>());
    }

    [HttpGet("{rm}")]
    public async Task<IActionResult> GetAlunoByRm (int rm)
    {
        var aluno = await _alunoService.GetByRm(rm);
        if (aluno is null) return NotFound();
        return Ok(aluno);
    }

    [HttpDelete("{rm}")]
    public async Task<IActionResult> DeleteAlunoByRm (int rm)
    {
        var linhasAfetadas = await _alunoService.DeleteByRm(rm);
        if (linhasAfetadas == 0) return NotFound();
        return NoContent();
    }

    [HttpPut("{rm}")]
    public async Task<IActionResult> UpdateAlunoByRm (int rm, [FromBody] UpdateAlunoDto dto)
    {
        var linhasAlteradas = await _alunoService.UpdateByRm(rm, dto);
        if (linhasAlteradas == 0) return NotFound();
        return NoContent();
    }
}
