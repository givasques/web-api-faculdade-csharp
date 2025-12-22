using Dapper;
using FaculdadeApi.Dtos.AlunoDtos;
using FaculdadeApi.Dtos.AvaliacaoDtos;
using System.Data;
using System.Data.Common;

namespace FaculdadeApi.Services;

public class AlunoService
{
    private readonly DbConnection _connection;

    public AlunoService(DbConnection connection)
    {
       _connection = connection;
    }

    private async Task OpenConnectionAsync()
    {
        if (_connection.State == ConnectionState.Closed)
            await _connection.OpenAsync();
    }

    public async Task<ReadAlunoDto?> Create(CreateAlunoDto dto)
    {
        await OpenConnectionAsync();

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

        var alunoCriado = await _connection.QuerySingleOrDefaultAsync<ReadAlunoDto>(sql, parametros);
        if (alunoCriado is null) return null;
        return alunoCriado;   
    }

    public async Task<IEnumerable<ReadAlunoDto>> GetAll(int offSet, int limit)
    {
        await OpenConnectionAsync();

        var sql = @"SELECT rm, email, cpf, nome, data_nasc AS dataNascimento, 
                    id_turma AS idTurma 
                    FROM tb_aluno
                    ORDER BY rm
                    OFFSET @OffSet
                    LIMIT @Limit";

        var parametros = new
        {
            OffSet = offSet,
            Limit = limit
        };

        var alunos = await _connection.QueryAsync<ReadAlunoDto>(sql, parametros);
        return alunos;
    }

    public async Task<ReadAlunoDto?> GetByRm(int rm)
    {
        await OpenConnectionAsync();


        var sql = "SELECT rm, email, cpf, nome, data_nasc AS dataNascimento, id_turma " +
                    $"AS idTurma FROM tb_aluno WHERE rm = @Rm";

        var parametros = new
        {
            Rm = rm
        };

        var aluno = await _connection.QuerySingleOrDefaultAsync<ReadAlunoDto>(sql, parametros);
        if (aluno is null) return null;
        return aluno; 
    }

    public async Task<int> DeleteByRm(int rm)
    {
        await OpenConnectionAsync();

        var sql = "DELETE FROM tb_aluno WHERE rm = @Rm";

        var parametros = new
        {
            Rm = rm
        };

        return await _connection.ExecuteAsync(sql, parametros);  
    }

    public async Task<ReadAlunoDto?> UpdateByRm (int rm, UpdateAlunoDto dto)
    {
        await OpenConnectionAsync();

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

        return await _connection.QuerySingleOrDefaultAsync<ReadAlunoDto>(sql, parametros);
    }

    public async Task<ReadProvasRealizadasDto?> GetProvasByRm(int rm)
    {
        await OpenConnectionAsync();

        var existe = await _connection
            .ExecuteScalarAsync<bool>(@"SELECT EXISTS (SELECT 1 FROM tb_aluno WHERE rm = @Rm)", new { Rm = rm });

        if (!existe) return null;


        var sql = @"SELECT av.id, mm.id_turma AS idTurma, m.nome AS nomeMateria, 
		                    av.data_aplicacao AS dataAplicacao, av.nota_max AS notaMaxima, rp.nota
                    FROM tb_realizacao_prova rp
                    JOIN tb_avaliacao av ON av.id = rp.id_avaliacao
                    JOIN tb_materia_ministrada mm ON av.id_materia_ministrada = mm.id
                    JOIN tb_materia m ON mm.id_materia = m.id
                    WHERE rp.rm_aluno = @Rm";

        var provas = await _connection
            .QueryAsync<ReadAvaliacaoSimplificadaDto, ReadProvaAvaliadaDto, ReadProvaAvaliadaDto>
            (
                sql,
                (avaliacao, provaAvaliada) =>
                {
                    provaAvaliada.Avaliacao = avaliacao;
                    return provaAvaliada;
                },
                new { Rm = rm },
                splitOn: "nota"
            );

        return new ReadProvasRealizadasDto { RmAluno = rm, ProvasRealizadas = provas.Any() ? provas : [] };
    }
}
