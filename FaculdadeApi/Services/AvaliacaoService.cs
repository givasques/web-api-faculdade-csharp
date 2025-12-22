using Dapper;
using FaculdadeApi.Dtos.AvaliacaoDtos;
using FaculdadeApi.Dtos.MateriaDtos;
using FaculdadeApi.Dtos.TurmaDtos;
using System.Data;
using System.Data.Common;

namespace FaculdadeApi.Services;

public class AvaliacaoService
{
    private readonly DbConnection _connection;
    public AvaliacaoService(DbConnection connection)
    {
        _connection = connection;
    }

    private async Task OpenConnectionAsync()
    {
        if (_connection.State == ConnectionState.Closed)
            await _connection.OpenAsync();
    }

    public async Task<ReadAvaliacaoDto?> Create(CreateAvaliacaoDto dto)
    {
        await OpenConnectionAsync();

        var idMateriaMinistrada = await _connection
            .ExecuteScalarAsync<int>
            (@"SELECT id
            FROM tb_materia_ministrada
            WHERE id_turma = @IdTurma AND id_materia = @IdMateria",
            new
            {
                IdTurma = dto.IdTurma,
                IdMateria = dto.IdMateria
            });

        if (idMateriaMinistrada == 0) throw new InvalidOperationException("Essa matéria não está sendo ministrada na turma informada");

        var sql = @"INSERT INTO tb_avaliacao (id_materia_ministrada, data_aplicacao, nota_max)
                    VALUES (@IdMateriaMinistrada, @DataAplicacao, @NotaMaxima)
                    RETURNING id";

        var parametros = new
        {
            IdMateriaMinistrada = idMateriaMinistrada,
            DataAplicacao = dto.DataAplicacao.ToDateTime(TimeOnly.MinValue),
            NotaMaxima = dto.NotaMaxima
        };

        var idAvaliacaoCriada = await _connection
            .ExecuteScalarAsync<int>(sql, parametros);

        if (idAvaliacaoCriada == 0) return null;

        return await GetById(idAvaliacaoCriada);
    }

    public async Task<IEnumerable<ReadAvaliacaoDto>> GetAll()
    {
        await OpenConnectionAsync();

        var sql = @"SELECT a.id, a.data_aplicacao AS dataAplicacao, a.nota_max AS notaMaxima,
		                    t.id_curso AS idCurso, t.id, t.periodo, t.formato,
		                    m.descricao, m.id, m.nome
                    FROM tb_avaliacao a
                    JOIN tb_materia_ministrada mm ON a.id_materia_ministrada = mm.id
                    JOIN tb_turma t ON mm.id_turma = t.id
                    JOIN tb_materia m ON mm.id_materia = m.id";

        return await _connection
            .QueryAsync<ReadAvaliacaoDto, ReadTurmaDto, ReadMateriaDto, ReadAvaliacaoDto>
            (
                sql,
                (avaliacao, turma, materia) =>
                {
                    avaliacao.Turma = turma;
                    avaliacao.Materia = materia;
                    return avaliacao;
                },
                splitOn: "idCurso, descricao"
            );
    }

    public async Task<ReadAvaliacaoDto?> GetById(int id)
    {
        await OpenConnectionAsync();

        var sql = @"SELECT a.id, a.data_aplicacao AS dataAplicacao, a.nota_max AS notaMaxima,
		                    t.id_curso AS idCurso, t.id, t.periodo, t.formato,
		                    m.descricao, m.id, m.nome
                    FROM tb_avaliacao a
                    JOIN tb_materia_ministrada mm ON a.id_materia_ministrada = mm.id
                    JOIN tb_turma t ON mm.id_turma = t.id
                    JOIN tb_materia m ON mm.id_materia = m.id
                    WHERE a.id = @Id";

        var avaliacoes = await _connection
            .QueryAsync<ReadAvaliacaoDto, ReadTurmaDto, ReadMateriaDto, ReadAvaliacaoDto>
            (
                sql,
                (avaliacao, turma, materia) =>
                {
                    avaliacao.Turma = turma;
                    avaliacao.Materia = materia;
                    return avaliacao;
                },
                new { Id = id },
                splitOn: "idCurso, descricao"
            );

        return avaliacoes.FirstOrDefault();
    }

    public async Task<int> DeleteById(int id)
    {
        await OpenConnectionAsync();

        return await _connection
            .ExecuteAsync(@"DELETE FROM tb_avaliacao WHERE id = @Id", new { Id = id });
    }

    public async Task<ReadAvaliacaoDto?> UpdateById(int id, UpdateAvaliacaoDto dto)
    {
        await OpenConnectionAsync();

        var linhasAlteradas = await _connection
            .ExecuteAsync
            (@"UPDATE tb_avaliacao
                SET data_aplicacao = @DataAplicacao,
                    nota_max = @NotaMaxima
                WHERE id = @Id", 
            new
            {
                Id = id,
                DataAplicacao = dto.DataAplicacao.ToDateTime(TimeOnly.MinValue),
                NotaMaxima = dto.NotaMaxima
            });

        if (linhasAlteradas == 0) return null;

        return await GetById(id);
    }
}
