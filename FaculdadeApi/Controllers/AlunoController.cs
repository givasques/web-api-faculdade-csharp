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

    /// <summary>
    /// Cadastra um novo aluno no sistema.
    /// </summary>
    /// <param name="dto">
    /// Objeto contendo os dados necessários para a criação do aluno.
    /// </param>
    /// <returns>
    /// Retorna o aluno recém-criado ou um erro caso a operação falhe.
    /// </returns>
    /// <response code="201">Aluno criado com sucesso.</response>
    /// <response code="400">Dados inválidos para criação do aluno.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ReadAlunoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAluno([FromBody] CreateAlunoDto dto)
    {
        var aluno = await _alunoService.Create(dto);
        if (aluno is null) return BadRequest();
        return CreatedAtAction(nameof(GetAlunoByRm), new { rm = aluno.Rm }, aluno); 
    }

    /// <summary>
    /// Lista os alunos cadastrados com paginação.
    /// </summary>
    /// <param name="offSet">Quantidade de registros a serem ignorados.</param>
    /// <param name="limit">Quantidade máxima de registros retornados.</param>
    /// <returns>
    /// Retorna uma lista de alunos.
    /// </returns>
    /// <response code="200">Lista de alunos retornada com sucesso.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ReadAlunoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] int offSet = 0, [FromQuery] int limit = 10)
    {
        var alunos = await _alunoService.GetAll(offSet, limit);
        return Ok(alunos ?? []);
    }

    /// <summary>
    /// Busca um aluno pelo RM.
    /// </summary>
    /// <param name="rm">Rm do aluno.</param>
    /// <returns>
    /// Retorna os dados do aluno encontrado.
    /// </returns>
    /// <response code="200">Aluno encontrado.</response>
    /// <response code="404">Aluno não encontrado.</response>
    [HttpGet("{rm}")]
    [ProducesResponseType(typeof(ReadAlunoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAlunoByRm (int rm)
    {
        var aluno = await _alunoService.GetByRm(rm);
        if (aluno is null) return NotFound();
        return Ok(aluno);
    }

    /// <summary>
    /// Busca as provas realizadas por um aluno.
    /// </summary>
    /// <param name="rm">Rm do aluno.</param>
    /// <returns>
    /// Retorna as provas vinculadas ao aluno.
    /// </returns>
    /// <response code="200">Provas encontradas com sucesso.</response>
    /// <response code="404">Aluno não encontrado.</response>
    [HttpGet("{rm}/provas")]
    [ProducesResponseType(typeof(ReadProvasRealizadasDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProvasByRm(int rm)
    {
        var provas = await _alunoService.GetProvasByRm(rm);
        if (provas is null) return NotFound();
        return Ok(provas);
    }

    /// <summary>
    /// Remove um aluno pelo RM.
    /// </summary>
    /// <param name="rm">Rm do aluno.</param>
    /// <returns>
    /// Não retorna conteúdo.
    /// </returns>
    /// <response code="204">Aluno removido com sucesso.</response>
    /// <response code="404">Aluno não encontrado.</response>
    [HttpDelete("{rm}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAlunoByRm (int rm)
    {
        var linhasAfetadas = await _alunoService.DeleteByRm(rm);
        if (linhasAfetadas == 0) return NotFound();
        return NoContent();
    }

    /// <summary>
    /// Atualiza os dados de um aluno existente.
    /// </summary>
    /// <param name="rm">Rm do aluno.</param>
    /// <param name="dto">Objeto contendo os dados atualizados do aluno.</param>
    /// <returns>
    /// Retorna o aluno atualizado.
    /// </returns>
    /// <response code="200">Aluno atualizado com sucesso.</response>
    /// <response code="404">Aluno não encontrado.</response>
    [HttpPut("{rm}")]
    [ProducesResponseType(typeof(ReadAlunoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAlunoByRm (int rm, [FromBody] UpdateAlunoDto dto)
    {
        var aluno = await _alunoService.UpdateByRm(rm, dto);
        if (aluno is null) return NotFound();
        return Ok(aluno);
    }
}
