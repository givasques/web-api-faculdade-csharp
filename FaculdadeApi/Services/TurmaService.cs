using Dapper;
using FaculdadeApi.Dtos.AvaliacaoDtos;
using FaculdadeApi.Dtos.MateriaDtos;
using FaculdadeApi.Dtos.ProfessorDtos;
using FaculdadeApi.Dtos.TurmaDtos;
using System.Data.Common;
using System.Data;

namespace FaculdadeApi.Services;

public class TurmaService
{
    private readonly DbConnection _connection;
    public TurmaService(DbConnection connection)
    {
        _connection = connection;
    }

    private async Task OpenConnectionAsync()
    {
        if (_connection.State == ConnectionState.Closed)
            await _connection.OpenAsync();
    }

    public async Task<ReadTurmaDto> Create(CreateTurmaDto dto)
    {
        await OpenConnectionAsync();

        var sql = @"INSERT INTO tb_turma (id, id_curso, periodo, formato)
                    VALUES (@Id, @IdCurso, @Periodo, @Formato)
                    RETURNING id, id_curso AS idCurso, periodo, formato";

        var parametros = new
        {
            Id = dto.Id,
            IdCurso = dto.IdCurso,
            Periodo = dto.Periodo,
            Formato = dto.Formato
        };

        return await _connection.QuerySingleAsync<ReadTurmaDto>(sql, parametros); ;
    }
    public async Task<IEnumerable<ReadTurmaDto>> GetAll(int offSet, int limit)
    {
        await OpenConnectionAsync();

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

        return await _connection.QueryAsync<ReadTurmaDto>(sql, parametros); 
    }

    public async Task<ReadTurmaDto?> GetById(string id)
    {
        await OpenConnectionAsync();

        var sql = "SELECT id, id_curso AS idCurso, periodo, formato FROM tb_turma WHERE id = @Id";
        var parametros = new { Id = id.ToUpper() };

        return await _connection.QuerySingleOrDefaultAsync<ReadTurmaDto>(sql, parametros);
    }

    public async Task<int> DeleteById (string id)
    {
        await OpenConnectionAsync();

        return await _connection
            .ExecuteAsync("DELETE FROM tb_turma WHERE id = @Id", new { Id = id });
    }

    public async Task<ReadTurmaDto?> UpdateById(string id, UpdateTurmaDto dto)
    {
        await OpenConnectionAsync();

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

        return await _connection.QuerySingleOrDefaultAsync<ReadTurmaDto>(sql, parametros);
    }

    public async Task<ReadMateriasDaTurmaDto?> GetMateriasById(string id)
    {
        await OpenConnectionAsync();

        var turma = await _connection
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

        var materias = await _connection
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
        await OpenConnectionAsync();

        var existe = await _connection
            .ExecuteScalarAsync<bool>(@"SELECT EXISTS (SELECT id FROM tb_turma WHERE id = @Id)", new { Id = id });

        if (!existe) return null;

        var sql = @"SELECT av.id, mm.id_turma as idTurma, m.nome AS nomeMateria,
		                    av.data_aplicacao AS dataAplicacao, av.nota_max AS notaMaxima
                    FROM tb_avaliacao av
                    JOIN tb_materia_ministrada mm ON av.id_materia_ministrada = mm.id
                    JOIN tb_materia m ON mm.id_materia = m.id
                    WHERE mm.id_turma = @Id";

        var avaliacoes = await _connection
            .QueryAsync<ReadAvaliacaoSimplificadaDto>(sql, new { Id = id });

        return new ReadAvaliacoesPorTurmaDto { IdTurma = id, Avaliacoes = avaliacoes.Any() ? avaliacoes : [] };
    }
}
