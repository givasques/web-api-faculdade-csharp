using Dapper;
using FaculdadeApi.Dtos.AvaliacaoDtos;
using FaculdadeApi.Dtos.MateriaDtos;
using FaculdadeApi.Dtos.TurmaDtos;
using Npgsql;

namespace FaculdadeApi.Services;

public class AvaliacaoService
{
    private readonly string _connectionString;
    public AvaliacaoService(IConfiguration configuration)
    {
        _connectionString = configuration["ConnectionStrings:FaculdadeApi"]!;
    }

    public async Task<ReadAvaliacaoDto?> Create(CreateAvaliacaoDto dto)
    {
        await using var sqlConnection = new NpgsqlConnection(_connectionString);
        await sqlConnection.OpenAsync();

        var idMateriaMinistrada = await sqlConnection
            .QuerySingleOrDefaultAsync<int>
            (@"SELECT id
            FROM tb_materia_ministrada
            WHERE id_turma = @IdTurma AND id_materia = @IdMateria",
            new
            {
                IdTurma = dto.IdTurma,
                IdMateria = dto.IdMateria
            });

        if (idMateriaMinistrada == 0) throw new ApplicationException("Essa matéria não está sendo ministrada na turma informada");

        var sql = @"INSERT INTO tb_avaliacao (id_materia_ministrada, data_aplicacao, nota_max)
                    VALUES (@IdMateriaMinistrada, @DataAplicacao, @NotaMaxima)
                    RETURNING id";

        var parametros = new
        {
            IdMateriaMinistrada = idMateriaMinistrada,
            DataAplicacao = dto.DataAplicacao.ToDateTime(TimeOnly.MinValue),
            NotaMaxima = dto.NotaMaxima
        };

        var idAvaliacaoCriada = await sqlConnection
            .QuerySingleOrDefaultAsync<int>(sql, parametros);

        if (idAvaliacaoCriada == 0) return null;

        return await GetById(idAvaliacaoCriada);
    }

    public async Task<IEnumerable<ReadAvaliacaoDto>?> GetAll()
    {
        await using var sqlConnection = new NpgsqlConnection(_connectionString);
        await sqlConnection.OpenAsync();

        var sql = @"SELECT a.id, a.data_aplicacao AS dataAplicacao, a.nota_max AS notaMaxima,
		                    t.id_curso AS idCurso, t.id, t.periodo, t.formato,
		                    m.descricao, m.id, m.nome
                    FROM tb_avaliacao a
                    JOIN tb_materia_ministrada mm ON a.id_materia_ministrada = mm.id
                    JOIN tb_turma t ON mm.id_turma = t.id
                    JOIN tb_materia m ON mm.id_materia = m.id";

        return await sqlConnection
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
        await using var sqlConnection = new NpgsqlConnection(_connectionString);
        await sqlConnection.OpenAsync();

        var sql = @"SELECT a.id, a.data_aplicacao AS dataAplicacao, a.nota_max AS notaMaxima,
		                    t.id_curso AS idCurso, t.id, t.periodo, t.formato,
		                    m.descricao, m.id, m.nome
                    FROM tb_avaliacao a
                    JOIN tb_materia_ministrada mm ON a.id_materia_ministrada = mm.id
                    JOIN tb_turma t ON mm.id_turma = t.id
                    JOIN tb_materia m ON mm.id_materia = m.id
                    WHERE a.id = @Id";

        var avaliacoes = await sqlConnection
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
        await using var sqlConnection = new NpgsqlConnection(_connectionString);
        await sqlConnection.OpenAsync();

        return await sqlConnection
            .ExecuteAsync(@"DELETE FROM tb_avaliacao WHERE id = @Id", new { Id = id });
    }

    public async Task<ReadAvaliacaoDto?> UpdateById(int id, UpdateAvaliacaoDto dto)
    {
        await using var sqlConnection = new NpgsqlConnection(_connectionString);
        await sqlConnection.OpenAsync();


        var linhasAlteradas = await sqlConnection
            .ExecuteAsync
            (@"UPDATE tb_avaliacao
                SET data_aplicacao = @DataAplicacao,
                    nota_max = @NotaMaxima", 
            new
            {
                DataAplicacao = dto.DataAplicacao.ToDateTime(TimeOnly.MinValue),
                NotaMaxima = dto.NotaMaxima
            });

        if (linhasAlteradas == 0) return null;

        return await GetById(id);
    }
}
