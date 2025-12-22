using Dapper;
using FaculdadeApi.Dtos.MateriaDtos;
using FaculdadeApi.Dtos.ProfessorDtos;
using FaculdadeApi.Dtos.TurmaDtos;
using System.Data.Common;
using System.Data;

namespace FaculdadeApi.Services;

public class ProfessorService
{
    private readonly DbConnection _connection;

    public ProfessorService(DbConnection connection)
    {
        _connection = connection;
    }

    private async Task OpenConnectionAsync()
    {
        if (_connection.State == ConnectionState.Closed)
            await _connection.OpenAsync();
    }

    public async Task<ReadProfessorDto> Create(CreateProfessorDto dto)
    {
        await OpenConnectionAsync();

        var sql = @"INSERT INTO tb_professor (nome, cpf, telefone, email)
                    VALUES (@Nome, @Cpf, @Telefone, @Email)
                    RETURNING id, nome, cpf, telefone, email";

        var parametros = new
        {
            Nome = dto.Nome,
            Cpf = dto.Cpf,
            Telefone = dto.Telefone,
            Email = dto.Email
        };

        return await _connection.QuerySingleAsync<ReadProfessorDto>(sql, parametros);
    }

    public async Task<IEnumerable<ReadProfessorDto>> GetAll(int offSet, int limit)
    {
        await OpenConnectionAsync();

        var sql = @"SELECT id, nome, cpf, telefone, email
                    FROM tb_professor
                    ORDER BY id
                    OFFSET @OffSet
                    LIMIT @Limit";

        return await _connection
            .QueryAsync<ReadProfessorDto>(sql, new { OffSet = offSet, Limit = limit });
    }

    public async Task<ReadProfessorDto?> GetById(int id)
    {
        await OpenConnectionAsync();

        var sql = @"SELECT id, nome, cpf, telefone, email 
                    FROM tb_professor 
                    WHERE id = @Id";

        return await _connection
            .QuerySingleOrDefaultAsync<ReadProfessorDto>(sql, new { Id = id });
    }

    public async Task<int> DeleteById(int id)
    {
        await OpenConnectionAsync();

        var sql = @"DELETE FROM tb_professor
                    WHERE id = @Id";

        return await _connection.ExecuteAsync(sql, new { Id = id });
    }

    public async Task<ReadProfessorDto?> UpdateById(int id, UpdateProfessorDto dto)
    {
        await OpenConnectionAsync();

        var sql = @"UPDATE tb_professor
                    SET nome = @Nome,
	                    cpf = @Cpf,
	                    telefone = @Telefone,
	                    email = @Email
                    WHERE id = @Id
                    RETURNING id, nome, cpf, telefone, email";

        var parametros = new
        {
            Nome = dto.Nome,
            Cpf = dto.Cpf,
            Telefone = dto.Telefone,
            Email = dto.Email,
            Id = id
        };

        return await _connection.QuerySingleOrDefaultAsync<ReadProfessorDto>(sql, parametros);
    }

    public async Task<ReadMateriasMinistradasDto?> GetMateriasMinistradasById (int id)
    {
        await OpenConnectionAsync();

        var professor = await _connection
            .QuerySingleOrDefaultAsync<ReadProfessorDto>
            (@"SELECT id, nome, cpf, telefone, email
               FROM tb_professor WHERE id = @Id",
            new { Id = id });

        if (professor is null) return null;

        var sql = @"SELECT t.id, t.id_curso AS IdCurso, t.periodo, t.formato, m.descricao, 
                    m.id, m.nome
                    FROM tb_turma t
                    JOIN tb_materia_ministrada mm ON t.id = mm.id_turma
                    JOIN tb_materia m ON mm.id_materia = m.id
                    WHERE mm.id_professor = @Id";

        var materias = await _connection
            .QueryAsync<ReadTurmaDto, ReadMateriaDto, ReadTurmaMateriaDto>
            (
                sql,
                (turma, materia) =>
                {
                    return new ReadTurmaMateriaDto { Turma = turma, Materia = materia};
                },
                new { Id = id },
                splitOn: "descricao"
            );

        return new ReadMateriasMinistradasDto { Professor = professor, Materias = materias.Any() ? materias : [] };
    }
}
