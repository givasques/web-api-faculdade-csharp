using AutoMapper;
using Dapper;
using FaculdadeApi.Dtos.CursoDtos;
using FaculdadeApi.Dtos.MateriaDtos;
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
        await using var sqlConnection = new NpgsqlConnection(_connectionString);
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
        await using var sqlConnection = new NpgsqlConnection(_connectionString);
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
        await using var sqlConnection = new NpgsqlConnection(_connectionString);
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
        await using var sqlConnection = new NpgsqlConnection(_connectionString);
        await sqlConnection.OpenAsync();

        var sql = "DELETE FROM tb_curso WHERE id = @Id";
        var parametros = new { Id = id };

        return await sqlConnection.ExecuteAsync(sql, parametros);
    }

    public async Task<ReadCursoDto?> UpdateById(int id, UpdateCursoDto dto)
    {
        await using var sqlConnection = new NpgsqlConnection(_connectionString);
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

    public async Task<ReadGradeCursoDto?> AddMateriaAGrade(int idCurso, AddMateriaAGradeDto materiaDto)
    {
        await using var sqlConnection = new NpgsqlConnection(_connectionString);
        await sqlConnection.OpenAsync();

        await using var transaction = await sqlConnection.BeginTransactionAsync();

        try
        {
            var sql = @"INSERT INTO tb_grade_curso (id_curso, id_materia, carga_horaria)
                    VALUES (@IdCurso, @IdMateria, @CargaHoraria)";

            var parametros = new
            {
                IdCurso = idCurso,
                IdMateria = materiaDto.IdMateria,
                CargaHoraria = materiaDto.CargaHoraria
            };

            await sqlConnection.ExecuteAsync(sql, parametros, transaction);

            sql = @"SELECT c.id, c.nome, c.descricao, c.qnt_semestres AS qntSemestres
                FROM tb_curso c
                WHERE c.id = @IdCurso";

            var curso = await sqlConnection
            .QuerySingleOrDefaultAsync<ReadCursoDto>(sql, new { IdCurso = idCurso }, transaction);

            sql = @"SELECT m.id, m.nome, m.descricao, gc.carga_horaria AS cargaHoraria
                FROM tb_materia m
                JOIN tb_grade_curso gc ON gc.id_materia = m.id
                WHERE m.id = @IdMateria AND gc.id_curso = @IdCurso";

            var materias = await sqlConnection
            .QueryAsync<ReadMateriaDto, ReadMateriaGradeDto, ReadMateriaGradeDto>
            (sql, (materia, grade) =>
            {
                grade.Materia = materia;
                return grade;
            },
                new { IdMateria = materiaDto.IdMateria, IdCurso = idCurso },
                transaction,
                splitOn: "cargaHoraria");

            await transaction.CommitAsync();

            return new ReadGradeCursoDto()
            {
                Curso = curso,
                Materias = materias.Any() ? [materias.First()] : []
            };

        } catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<ReadGradeCursoDto> GetGradePorId(int id)
    {
        await using var sqlConnection = new NpgsqlConnection(_connectionString);
        await sqlConnection.OpenAsync();

        var sql = @"SELECT id, nome, descricao, qnt_semestres AS QntSemestres
                    FROM tb_curso WHERE id = @Id";

        var curso = await sqlConnection
            .QuerySingleOrDefaultAsync<ReadCursoDto>(sql, new { Id = id });

        if (curso is null) return null;

        sql = @"SELECT m.id, m.nome, m.descricao, gc.carga_horaria AS cargaHoraria
                    FROM tb_curso c
                    JOIN tb_grade_curso gc ON gc.id_curso = c.id
                    JOIN tb_materia m ON gc.id_materia = m.id
                    WHERE c.id = @Id";

        var materias = await sqlConnection
            .QueryAsync<ReadMateriaDto, ReadMateriaGradeDto, ReadMateriaGradeDto>(sql, (materia, grade) =>
            {
                grade.Materia = materia;
                return grade;
            },
            new { Id = id },
            splitOn: "cargaHoraria");

        var grade = new ReadGradeCursoDto()
        {
            Curso = curso,
            Materias = materias
        };

        return grade;
    }

    public async Task<int> DeleteMateriaDaGrade(int idCurso, int idMateria)
    {
        await using var sqlConnection = new NpgsqlConnection(_connectionString);
        await sqlConnection.OpenAsync();

        var sql = @"DELETE FROM tb_grade_curso
                    WHERE id_curso = @IdCurso AND id_materia = @IdMateria";

        return await sqlConnection
            .ExecuteAsync(sql, new { IdCurso = idCurso, IdMateria = idMateria });
    }
}
