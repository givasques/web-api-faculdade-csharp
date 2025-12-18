using AutoMapper;
using Dapper;
using FaculdadeApi.Dtos.TurmaDtos;
using FaculdadeApi.Models;
using Npgsql;
using System.Threading.Tasks;

namespace FaculdadeApi.Services;

public class TurmaService
{
    private readonly string _connectionString;
    public TurmaService(IConfiguration configuration)
    {
        _connectionString = configuration["ConnectionStrings:FaculdadeApi"]!;
    }

    public async Task<ReadTurmaDto?> Create(CreateTurmaDto dto)
    {
        using var sqlConnection = new NpgsqlConnection(_connectionString);
        await sqlConnection.OpenAsync();

        var sql = @"INSERT INTO tb_turma VALUES (@Id, @IdCurso, @Periodo, @Formato)
                    RETURNING id, id_curso AS idCurso, periodo, formato";

        var parametros = new
        {
            Id = dto.Id,
            IdCurso = dto.IdCurso,
            Periodo = dto.Periodo,
            Formato = dto.Formato
        };

        var turma = await sqlConnection.QuerySingleOrDefaultAsync<ReadTurmaDto>(sql, parametros);
        return turma;
    }
    public async Task<IEnumerable<ReadTurmaDto>> GetAll(int offSet, int limit)
    {
        using var sqlConnection = new NpgsqlConnection(_connectionString);
        await sqlConnection.OpenAsync();

        var sql = @"SELECT id, id_curso AS idCurso, periodo, formato 
                    FROM tb_turma
                    ORDER BY id
                    OFFSET @OffSet
                    LIMIT @Limit";

        var parametros = new
        {
            OffSet = offSet,
            Limit = limit
        };

        var turmas = await sqlConnection.QueryAsync<ReadTurmaDto>(sql, parametros);
        return turmas;
    }

    public async Task<ReadTurmaDto?> GetById(string id)
    {
        using var sqlConnection = new NpgsqlConnection(_connectionString);
        await sqlConnection.OpenAsync();

        var sql = "SELECT id, id_curso AS idCurso, periodo, formato FROM tb_turma WHERE id = @Id";
        var parametros = new { Id = id.ToUpper() };

        var turma = await sqlConnection.QuerySingleOrDefaultAsync<ReadTurmaDto>(sql, parametros);
        if (turma is null) return null;
        return turma;
    }

    public async Task<int> DeleteById (string id)
    {
        using var sqlConnection = new NpgsqlConnection(_connectionString);
        await sqlConnection.OpenAsync();

        var sql = "DELETE FROM tb_turma WHERE id = @Id";
        var parametros = new { Id = id };

        return await sqlConnection.ExecuteAsync(sql, parametros);
    }

    public async Task<ReadTurmaDto?> UpdateById(string id, UpdateTurmaDto dto)
    {
        using var sqlConnection = new NpgsqlConnection(_connectionString);
        await sqlConnection.OpenAsync();

        var sql = @"UPDATE tb_turma
                    SET id_curso = @IdCurso,
                        periodo = @Periodo,
                        formato = @Formato
                    WHERE id = @Id
                    RETURNING id, id_curso as idCurso, periodo, formato";

        var parametros = new
        {
            IdCurso = dto.IdCurso,
            Periodo = dto.Periodo,
            Formato = dto.Formato,
            Id = id
        };

        var turma = await sqlConnection.QuerySingleOrDefaultAsync<ReadTurmaDto>(sql, parametros);
        return turma;
    }
}
