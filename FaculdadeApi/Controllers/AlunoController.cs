using FaculdadeApi.Dtos.AlunoDtos;
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
    public async Task<IActionResult> GetAll([FromQuery] int offSet = 0, [FromQuery] int limit = 10)
    {
        var alunos = await _alunoService.GetAll(offSet, limit);
        return Ok(alunos ?? []);
    }

    [HttpGet("{rm}")]
    public async Task<IActionResult> GetAlunoByRm (int rm)
    {
        var aluno = await _alunoService.GetByRm(rm);
        if (aluno is null) return NotFound();
        return Ok(aluno);
    }

    [HttpGet("{rm}/provas")]
    public async Task<IActionResult> GetProvasByRm(int rm)
    {
        var provas = await _alunoService.GetProvasByRm(rm);
        if (provas is null) return NotFound();
        return Ok(provas);
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
        var aluno = await _alunoService.UpdateByRm(rm, dto);
        if (aluno is null) return NotFound();
        return Ok(aluno);
    }
}
