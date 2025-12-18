using AutoMapper;
using Dapper;
using FaculdadeApi.Dtos.CursoDtos;
using FaculdadeApi.Models;
using Npgsql;

namespace FaculdadeApi.Services;

public class CursoService
{
    private readonly string _connectionString;

    public CursoService(IConfiguration configuration)
    {
        _connectionString = configuration["ConnectionStrings:FaculdadeApi"]!;
    }

    public async Task<ReadCursoDto?> Create(CreateCursoDto dto)
    {
        using var sqlConnection = new NpgsqlConnection(_connectionString);
        await sqlConnection.OpenAsync();

        var sql = @"INSERT INTO tb_curso (nome, descricao, qnt_semestres)
                    VALUES (@Nome,@Descricao,@QntSemestres)
                    RETURNING id, nome, descricao, qnt_semestres AS QntSemestres";

        var parametros = new
        {
            Nome = dto.Nome,
            Descricao = dto.Descricao,
            QntSemestres = dto.QntSemestres
        };

        var curso = await sqlConnection.QuerySingleOrDefaultAsync<ReadCursoDto>(sql, parametros);
        if (curso is null) return null;
        return curso;
    }

    public async Task<IEnumerable<ReadCursoDto>> GetAll(int offSet, int limit)
    {
        using var sqlConnection = new NpgsqlConnection(_connectionString);
        await sqlConnection.OpenAsync();

        var sql = @"SELECT id, nome, descricao, qnt_semestres AS QntSemestres 
                    FROM tb_curso
                    OFFSET @OffSet
                    LIMIT @Limit";

        var parametros = new
        {
            OffSet = offSet,
            Limit = limit
        };

        return await sqlConnection.QueryAsync<ReadCursoDto>(sql, parametros);
    }

    public async Task<ReadCursoDto?> GetById(int id)
    {
        using var sqlConnection = new NpgsqlConnection(_connectionString);
        await sqlConnection.OpenAsync();

        var sql = @"SELECT id, nome, descricao, qnt_semestres AS QntSemestres 
                    FROM tb_curso
                    WHERE id = @Id";

        var parametros = new { Id = id };

        var curso = await sqlConnection.QuerySingleOrDefaultAsync<ReadCursoDto>(sql, parametros);
        if (curso is null) return null;
        return curso;
    }

    public async Task<int> DeleteById(int id)
    {
        using var sqlConnection = new NpgsqlConnection(_connectionString);
        await sqlConnection.OpenAsync();

        var sql = "DELETE FROM tb_curso WHERE id = @Id";
        var parametros = new { Id = id };

        return await sqlConnection.ExecuteAsync(sql, parametros);
    }

    public async Task<ReadCursoDto?> UpdateById(int id, UpdateCursoDto dto)
    {
        using var sqlConnection = new NpgsqlConnection(_connectionString);
        await sqlConnection.OpenAsync();

        var sql = @"UPDATE tb_curso
                    SET nome = @Nome,
                        descricao = @Descricao,
                        qnt_semestres = @QntSemestres
                    WHERE id = @Id
                    RETURNING id, nome, descricao, qnt_semestres AS QntSemestres";

        var parametros = new
        {
            Nome = dto.Nome,
            Descricao = dto.Descricao,
            QntSemestres = dto.QntSemestres,
            Id = id
        };

        return await sqlConnection.QuerySingleOrDefaultAsync<ReadCursoDto>(sql, parametros);
    }
}
