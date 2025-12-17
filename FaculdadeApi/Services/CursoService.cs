using AutoMapper;
using Dapper;
using FaculdadeApi.Dtos;
using FaculdadeApi.Models;
using Npgsql;

namespace FaculdadeApi.Services;

public class CursoService
{
    private readonly string _connectionString;
    private readonly IMapper _mapper;

    public CursoService(IConfiguration configuration, IMapper mapper)
    {
        _connectionString = configuration["ConnectionStrings:FaculdadeApi"]!;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ReadCursoDto>> GetAll()
    {
        using var sqlConnection = new NpgsqlConnection(_connectionString);
        await sqlConnection.OpenAsync();

        var sql = "SELECT id, nome, descricao, qnt_semestres AS QntSemestres FROM tb_curso";
        var cursos = await sqlConnection.QueryAsync<Curso>(sql);
        return _mapper.Map<IEnumerable<ReadCursoDto>>(cursos);
    }

    public async Task<ReadCursoDto?> GetById(int id)
    {
        using var sqlConnection = new NpgsqlConnection(_connectionString);
        await sqlConnection.OpenAsync();

        var sql = @"SELECT id, nome, descricao, qnt_semestres AS QntSemestres 
                    FROM tb_curso
                    WHERE id = @Id";

        var parametros = new { Id = id };

        var curso = await sqlConnection.QuerySingleOrDefaultAsync<Curso>(sql, parametros);
        return _mapper.Map<ReadCursoDto>(curso);
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

        var curso = await sqlConnection.QuerySingleOrDefaultAsync<Curso>(sql, parametros);
        return _mapper.Map<ReadCursoDto>(curso);
    }

    public async Task<int> DeleteById(int id)
    {
        using var sqlConnection = new NpgsqlConnection(_connectionString);
        await sqlConnection.OpenAsync();

        var sql = "DELETE FROM tb_curso WHERE id = @Id";
        var parametros = new { Id = id };

        return await sqlConnection.ExecuteAsync(sql, parametros);
    }

    public async Task<int> UpdateById(int id, UpdateCursoDto dto)
    {
        using var sqlConnection = new NpgsqlConnection(_connectionString);
        await sqlConnection.OpenAsync();

        var sql = @"UPDATE tb_curso
                    SET nome = @Nome,
                        descricao = @Descricao,
                        qnt_semestres = @QntSemestres
                    WHERE id = @Id";

        var parametros = new
        {
            Nome = dto.Nome,
            Descricao = dto.Descricao,
            QntSemestres = dto.QntSemestres,
            Id = id
        };

        return await sqlConnection.ExecuteAsync(sql,parametros);
    }
}
