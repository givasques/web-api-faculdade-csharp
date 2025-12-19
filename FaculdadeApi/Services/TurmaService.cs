using AutoMapper;
using Dapper;
using FaculdadeApi.Dtos.AvaliacaoDtos;
using FaculdadeApi.Dtos.MateriaDtos;
using FaculdadeApi.Dtos.ProfessorDtos;
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
        await using var sqlConnection = new NpgsqlConnection(_connectionString);
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
        await using var sqlConnection = new NpgsqlConnection(_connectionString);
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
        await using var sqlConnection = new NpgsqlConnection(_connectionString);
        await sqlConnection.OpenAsync();

        var sql = "SELECT id, id_curso AS idCurso, periodo, formato FROM tb_turma WHERE id = @Id";
        var parametros = new { Id = id.ToUpper() };

        var turma = await sqlConnection.QuerySingleOrDefaultAsync<ReadTurmaDto>(sql, parametros);
        if (turma is null) return null;
        return turma;
    }

    public async Task<int> DeleteById (string id)
    {
        await using var sqlConnection = new NpgsqlConnection(_connectionString);
        await sqlConnection.OpenAsync();

        var sql = "DELETE FROM tb_turma WHERE id = @Id";
        var parametros = new { Id = id };

        return await sqlConnection.ExecuteAsync(sql, parametros);
    }

    public async Task<ReadTurmaDto?> UpdateById(string id, UpdateTurmaDto dto)
    {
        await using var sqlConnection = new NpgsqlConnection(_connectionString);
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

    public async Task<ReadMateriasDaTurmaDto?> GetMateriasById(string id)
    {
        await using var sqlConnection = new NpgsqlConnection(_connectionString);
        await sqlConnection.OpenAsync();

        var turma = await sqlConnection
            .QuerySingleOrDefaultAsync<ReadTurmaDto>
            (@"SELECT id, id_curso AS idCurso, periodo, formato
               FROM tb_turma
               WHERE id = @Id",
            new { Id = id });

        if (turma is null) return null;

        var sql = @"SELECT p.id, p.nome, p.cpf, p.telefone, p.email,
		                    m.descricao, m.id, m.nome
                    FROM tb_materia_ministrada mm 
                    JOIN tb_professor p ON mm.id_professor = p.id
                    JOIN tb_materia m ON mm.id_materia = m.id
                    WHERE mm.id_turma = @Id";

        var materias = await sqlConnection
            .QueryAsync<ReadProfessorDto, ReadMateriaDto, ReadProfessorMateriaDto>
            (
                sql,
                (professor, materia) =>
                {
                    return new ReadProfessorMateriaDto { Professor = professor, Materia = materia };
                },
                new { Id = id },
                splitOn: "descricao"
            );

        return new ReadMateriasDaTurmaDto { Turma = turma, Materias = materias.Any() ? materias : [] };
    }

    public async Task<ReadAvaliacoesPorTurmaDto?> GetAvaliacoesById(string id)
    {
        await using var sqlConnection = new NpgsqlConnection(_connectionString);
        await sqlConnection.OpenAsync();

        var idEncontrado = await sqlConnection
            .QuerySingleOrDefaultAsync<string>(@"SELECT id FROM tb_turma WHERE id = @Id", new { Id = id });

        if (idEncontrado is null) return null;

        var sql = @"SELECT av.id, mm.id_turma as idTurma, m.nome AS nomeMateria,
		                    av.data_aplicacao AS dataAplicacao, av.nota_max AS notaMaxima
                    FROM tb_avaliacao av
                    JOIN tb_materia_ministrada mm ON av.id_materia_ministrada = mm.id
                    JOIN tb_materia m ON mm.id_materia = m.id
                    WHERE mm.id_turma = @Id";

        var avaliacoes = await sqlConnection
            .QueryAsync<ReadAvaliacaoSimplificadaDto>(sql, new { Id = id });

        return new ReadAvaliacoesPorTurmaDto { IdTurma = id, Avaliacoes = avaliacoes.Any() ? avaliacoes : [] };
    }
}
