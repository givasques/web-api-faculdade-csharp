using FaculdadeApi.Models;
using Npgsql;
using Dapper;
using AutoMapper;
using FaculdadeApi.Dtos;

namespace FaculdadeApi.Services;

public class AlunoService
{
    private readonly string _connectionString;
    private IMapper _mapper;

    public AlunoService(IConfiguration configuration, IMapper mapper)
    {
        _connectionString = configuration["ConnectionStrings:FaculdadeApi"]!;
        _mapper = mapper;
    }

    public async Task<ReadAlunoDto?> Create(CreateAlunoDto dto)
    {
        using var sqlConnection = new NpgsqlConnection(_connectionString);
        await sqlConnection.OpenAsync();

        var sql = "INSERT INTO tb_aluno (email, cpf, nome, data_nasc, id_turma) " +
            $"VALUES (@Email,@Cpf,@Nome,@DataNascimento,@IdTurma)" +
            $"RETURNING rm, email, cpf, nome, data_nasc, id_turma AS idTurma";

        var parametros = new
        {
            Email = dto.Email,
            Cpf = dto.Cpf,
            Nome = dto.Nome,
            DataNascimento = dto.DataNascimento.ToDateTime(TimeOnly.MinValue),
            IdTurma = dto.IdTurma
        };

        var alunoCriado = await sqlConnection.QuerySingleOrDefaultAsync<Aluno>(sql, parametros);

        if (alunoCriado is null) return null;

        return _mapper.Map<ReadAlunoDto>(alunoCriado);   
    }

    public async Task<IEnumerable<ReadAlunoDto>> GetAll()
    {
        using var sqlConnection = new NpgsqlConnection(_connectionString);
        await sqlConnection.OpenAsync();

        var sql = "SELECT rm, email, cpf, nome, data_nasc AS dataNascimento, id_turma " +
                    "AS idTurma FROM tb_aluno";

        var alunos = await sqlConnection.QueryAsync<Aluno>(sql);

        return _mapper.Map<IEnumerable<ReadAlunoDto>>(alunos);
    }

    public async Task<ReadAlunoDto?> GetByRm(int rm)
    {
        using var sqlConnection = new NpgsqlConnection(_connectionString);
        await sqlConnection.OpenAsync();


        var sql = "SELECT rm, email, cpf, nome, data_nasc AS dataNascimento, id_turma " +
                    $"AS idTurma FROM tb_aluno WHERE rm = @Rm";

        var parametros = new
        {
            Rm = rm
        };

        var aluno = await sqlConnection.QuerySingleOrDefaultAsync<Aluno>(sql, parametros);

        if (aluno is null) return null;

        return _mapper.Map<ReadAlunoDto>(aluno); 
    }

    public async Task<int> DeleteByRm(int rm)
    {
        using var sqlConnection = new NpgsqlConnection(_connectionString);
        await sqlConnection.OpenAsync();

        var sql = "DELETE FROM tb_aluno WHERE rm = @Rm";

        var parametros = new
        {
            Rm = rm
        };

        return await sqlConnection.ExecuteAsync(sql, parametros);  
    }

    public async Task<int> UpdateByRm (int rm, UpdateAlunoDto dto)
    {
        using var sqlConnection = new NpgsqlConnection(_connectionString);
        await sqlConnection.OpenAsync();

        var sql = @"UPDATE tb_aluno
                    SET email = @Email,
                        cpf = @Cpf,
                        nome = @Nome,
                        data_nasc = @DataNascimento,
                        id_turma = @IdTurma
                    WHERE rm = @Rm";

        var parametros = new
        {
            Email = dto.Email,
            Cpf = dto.Cpf,
            Nome = dto.Nome,
            DataNascimento = dto.DataNascimento.ToDateTime(TimeOnly.MinValue),
            IdTurma = dto.IdTurma,
            Rm = rm
        };

        return await sqlConnection.ExecuteAsync(sql, parametros);
    }

}
