using Dapper;
using FaculdadeApi.Dtos.MateriaDtos;
using Npgsql;
using System.Threading.Tasks;

namespace FaculdadeApi.Services;

public class MateriaService
{
    private readonly string _connectionString;
    public MateriaService(IConfiguration configuration)
    {
        _connectionString = configuration["ConnectionStrings:FaculdadeApi"]!;
    }

    public async Task<ReadMateriaDto?> Create(CreateMateriaDto dto)
    {
        using var sqlConnection = new NpgsqlConnection(_connectionString);
        await sqlConnection.OpenAsync();

        var sql = @"INSERT INTO tb_materia (nome, descricao) VALUES (@Nome, @Descricao)
                    RETURNING id, nome, descricao";

        var parametros = new
        {
            Nome = dto.Nome,
            Descricao = dto.Descricao
        };

        return await sqlConnection.QuerySingleOrDefaultAsync<ReadMateriaDto>(sql, parametros);
    }

    public async Task<IEnumerable<ReadMateriaDto>> GetAll(int offSet, int limit)
    {
        using var sqlConnection = new NpgsqlConnection(_connectionString);
        await sqlConnection.OpenAsync();

        var sql = "SELECT * FROM tb_materia OFFSET @OffSet LIMIT @Limit";
        var parametros = new
        {
            OffSet = offSet,
            Limit = limit
        };

        return await sqlConnection.QueryAsync<ReadMateriaDto>(sql, parametros);
    }

    public async Task<ReadMateriaDto?> GetById(int id)
    {
        using var sqlConnection = new NpgsqlConnection(_connectionString);
        await sqlConnection.OpenAsync();

        var sql = "SELECT * FROM tb_materia WHERE id = @Id";
        var parametros = new { Id = id };

        return await sqlConnection.QuerySingleOrDefaultAsync<ReadMateriaDto>(sql, parametros);
    }

    public async Task<int> DeleteById(int id)
    {
        using var sqlConnection = new NpgsqlConnection(_connectionString);
        await sqlConnection.OpenAsync();

        var sql = "DELETE FROM tb_materia WHERE id = @Id";
        var parametros = new { Id = id };

        return await sqlConnection.ExecuteAsync(sql, parametros);
    }

    public async Task<ReadMateriaDto?> UpdateById(int id, UpdateMateriaDto dto)
    {
        using var sqlConnection = new NpgsqlConnection(_connectionString);
        await sqlConnection.OpenAsync();

        var sql = @"UPDATE tb_materia
                    SET nome = @Nome,
                        descricao = @Descricao
                    WHERE id = @Id
                    RETURNING id, nome, descricao";

        var parametros = new
        {
            Nome = dto.Nome,
            Descricao = dto.Descricao,
            Id = id
        };

        return await sqlConnection.QuerySingleOrDefaultAsync<ReadMateriaDto>(sql, parametros);
    }
}
