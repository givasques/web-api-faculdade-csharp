using FaculdadeApi.Models;
using Npgsql;
using Dapper;
using AutoMapper;
using FaculdadeApi.Dtos.AlunoDtos;

namespace FaculdadeApi.Services;

public class AlunoService
{
    private readonly string _connectionString;

    public AlunoService(IConfiguration configuration)
    {
        _connectionString = configuration["ConnectionStrings:FaculdadeApi"]!;
    }

    public async Task<ReadAlunoDto?> Create(CreateAlunoDto dto)
    {
        using var sqlConnection = new NpgsqlConnection(_connectionString);
        await sqlConnection.OpenAsync();

        var sql = "INSERT INTO tb_aluno (email, cpf, nome, data_nasc, id_turma) " +
            $"VALUES (@Email,@Cpf,@Nome,@DataNascimento,@IdTurma)" +
            $"RETURNING rm, email, cpf, nome, data_nasc AS dataNascimento, id_turma AS idTurma";

        var parametros = new
        {
            Email = dto.Email,
            Cpf = dto.Cpf,
            Nome = dto.Nome,
            DataNascimento = dto.DataNascimento.ToDateTime(TimeOnly.MinValue),
            IdTurma = dto.IdTurma
        };

        var alunoCriado = await sqlConnection.QuerySingleOrDefaultAsync<ReadAlunoDto>(sql, parametros);
        if (alunoCriado is null) return null;
        return alunoCriado;   
    }

    public async Task<IEnumerable<ReadAlunoDto>> GetAll(int offSet, int limit)
    {
        using var sqlConnection = new NpgsqlConnection(_connectionString);
        await sqlConnection.OpenAsync();

        var sql = @"SELECT rm, email, cpf, nome, data_nasc AS dataNascimento, 
                    id_turma AS idTurma 
                    FROM tb_aluno
                    OFFSET @OffSet
                    LIMIT @Limit";

        var parametros = new
        {
            OffSet = offSet,
            Limit = limit
        };

        var alunos = await sqlConnection.QueryAsync<ReadAlunoDto>(sql, parametros);
        return alunos;
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

        var aluno = await sqlConnection.QuerySingleOrDefaultAsync<ReadAlunoDto>(sql, parametros);
        if (aluno is null) return null;
        return aluno; 
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

    public async Task<ReadAlunoDto?> UpdateByRm (int rm, UpdateAlunoDto dto)
    {
        using var sqlConnection = new NpgsqlConnection(_connectionString);
        await sqlConnection.OpenAsync();

        var sql = @"UPDATE tb_aluno
                    SET email = @Email,
                        cpf = @Cpf,
                        nome = @Nome,
                        data_nasc = @DataNascimento,
                        id_turma = @IdTurma
                    WHERE rm = @Rm
                    RETURNING rm, email, cpf, nome, data_nasc AS DataNascimento,
                                id_turma AS IdTurma";

        var parametros = new
        {
            Email = dto.Email,
            Cpf = dto.Cpf,
            Nome = dto.Nome,
            DataNascimento = dto.DataNascimento.ToDateTime(TimeOnly.MinValue),
            IdTurma = dto.IdTurma,
            Rm = rm
        };

        return await sqlConnection.QuerySingleOrDefaultAsync<ReadAlunoDto>(sql, parametros);
    }

}
