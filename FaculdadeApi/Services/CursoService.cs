using Dapper;
using FaculdadeApi.Dtos.CursoDtos;
using FaculdadeApi.Dtos.MateriaDtos;
using System.Data.Common;
using System.Data;

namespace FaculdadeApi.Services;

public class CursoService
{
    private readonly DbConnection _connection;

    public CursoService(DbConnection connection)
    {
        _connection = connection;
    }

    private async Task OpenConnectionAsync()
    {
        if (_connection.State == ConnectionState.Closed)
            await _connection.OpenAsync();
    }

    public async Task<ReadCursoDto?> Create(CreateCursoDto dto)
    {
        await OpenConnectionAsync();

        var sql = @"INSERT INTO tb_curso (nome, descricao, qnt_semestres)
                    VALUES (@Nome,@Descricao,@QntSemestres)
                    RETURNING id, nome, descricao, qnt_semestres AS qntSemestres";

        var parametros = new
        {
            Nome = dto.Nome,
            Descricao = dto.Descricao,
            QntSemestres = dto.QntSemestres
        };

        return await _connection.QuerySingleOrDefaultAsync<ReadCursoDto>(sql, parametros);
    }

    public async Task<IEnumerable<ReadCursoDto>> GetAll(int offSet, int limit)
    {
        await OpenConnectionAsync();

        var sql = @"SELECT id, nome, descricao, qnt_semestres AS qntSemestres 
                    FROM tb_curso
                    OFFSET @OffSet
                    LIMIT @Limit";

        var parametros = new
        {
            OffSet = offSet,
            Limit = limit
        };

        return await _connection.QueryAsync<ReadCursoDto>(sql, parametros);
    }

    public async Task<ReadCursoDto?> GetById(int id)
    {
        await OpenConnectionAsync();

        var sql = @"SELECT id, nome, descricao, qnt_semestres AS qntSemestres 
                    FROM tb_curso
                    WHERE id = @Id";

        var parametros = new { Id = id };

        var curso = await _connection.QuerySingleOrDefaultAsync<ReadCursoDto>(sql, parametros);
        if (curso is null) return null;
        return curso;
    }

    public async Task<int> DeleteById(int id)
    {
        await OpenConnectionAsync();

        var sql = "DELETE FROM tb_curso WHERE id = @Id";
        var parametros = new { Id = id };

        return await _connection.ExecuteAsync(sql, parametros);
    }

    public async Task<ReadCursoDto?> UpdateById(int id, UpdateCursoDto dto)
    {
        await OpenConnectionAsync();

        var sql = @"UPDATE tb_curso
                    SET nome = @Nome,
                        descricao = @Descricao,
                        qnt_semestres = @QntSemestres
                    WHERE id = @Id
                    RETURNING id, nome, descricao, qnt_semestres AS qntSemestres";

        var parametros = new
        {
            Nome = dto.Nome,
            Descricao = dto.Descricao,
            QntSemestres = dto.QntSemestres,
            Id = id
        };

        return await _connection.QuerySingleOrDefaultAsync<ReadCursoDto>(sql, parametros);
    }

    public async Task<ReadGradeCursoDto?> AddMateriaAGrade(int idCurso, AddMateriaAGradeDto materiaDto)
    {
        await OpenConnectionAsync();

        await using var transaction = await _connection.BeginTransactionAsync();

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

            await _connection.ExecuteAsync(sql, parametros, transaction);

            sql = @"SELECT c.id, c.nome, c.descricao, c.qnt_semestres AS qntSemestres
                FROM tb_curso c
                WHERE c.id = @IdCurso";

            var curso = await _connection
            .QuerySingleOrDefaultAsync<ReadCursoDto>(sql, new { IdCurso = idCurso }, transaction);

            sql = @"SELECT m.id, m.nome, m.descricao, gc.carga_horaria AS cargaHoraria
                FROM tb_materia m
                JOIN tb_grade_curso gc ON gc.id_materia = m.id
                WHERE m.id = @IdMateria AND gc.id_curso = @IdCurso";

            var materias = await _connection
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

    public async Task<ReadGradeCursoDto?> GetGradePorId(int id)
    {
        await OpenConnectionAsync();

        var sql = @"SELECT id, nome, descricao, qnt_semestres AS qntSemestres
                    FROM tb_curso WHERE id = @Id";

        var curso = await _connection
            .QuerySingleOrDefaultAsync<ReadCursoDto>(sql, new { Id = id });

        if (curso is null) return null;

        sql = @"SELECT m.id, m.nome, m.descricao, gc.carga_horaria AS cargaHoraria
                    FROM tb_curso c
                    JOIN tb_grade_curso gc ON gc.id_curso = c.id
                    JOIN tb_materia m ON gc.id_materia = m.id
                    WHERE c.id = @Id";

        var materias = await _connection
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
        await OpenConnectionAsync();

        var sql = @"DELETE FROM tb_grade_curso
                    WHERE id_curso = @IdCurso AND id_materia = @IdMateria";

        return await _connection
            .ExecuteAsync(sql, new { IdCurso = idCurso, IdMateria = idMateria });
    }
}
