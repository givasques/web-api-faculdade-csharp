using Dapper;
using FaculdadeApi.Dtos.MateriaDtos;
using System.Data.Common;
using System.Data;

namespace FaculdadeApi.Services;

public class MateriaService
{
    private readonly DbConnection _connection;
    public MateriaService(DbConnection connection)
    {
        _connection = connection;
    }

    private async Task OpenConnectionAsync()
    {
        if (_connection.State == ConnectionState.Closed)
            await _connection.OpenAsync();
    }

    public async Task<ReadMateriaDto> Create(CreateMateriaDto dto)
    {
        await OpenConnectionAsync();

        var sql = @"INSERT INTO tb_materia (nome, descricao) VALUES (@Nome, @Descricao)
                    RETURNING id, nome, descricao";

        var parametros = new
        {
            Nome = dto.Nome,
            Descricao = dto.Descricao
        };

        return await _connection.QuerySingleAsync<ReadMateriaDto>(sql, parametros);
    }

    public async Task<IEnumerable<ReadMateriaDto>> GetAll(int offSet, int limit)
    {
        await OpenConnectionAsync();

        var sql = @"SELECT id, nome, descricao 
                    FROM tb_materia 
                    OFFSET @OffSet 
                    LIMIT @Limit";

        var parametros = new
        {
            OffSet = offSet,
            Limit = limit
        };

        return await _connection.QueryAsync<ReadMateriaDto>(sql, parametros);
    }

    public async Task<ReadMateriaDto?> GetById(int id)
    {
        await OpenConnectionAsync();

        var sql = @"SELECT id, nome, descricao 
                    FROM tb_materia 
                    WHERE id = @Id";

        var parametros = new { Id = id };

        return await _connection.QuerySingleOrDefaultAsync<ReadMateriaDto>(sql, parametros);
    }

    public async Task<int> DeleteById(int id)
    {
        await OpenConnectionAsync();

        var sql = "DELETE FROM tb_materia WHERE id = @Id";
        var parametros = new { Id = id };

        return await _connection.ExecuteAsync(sql, parametros);
    }

    public async Task<ReadMateriaDto?> UpdateById(int id, UpdateMateriaDto dto)
    {
        await OpenConnectionAsync();

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

        return await _connection.QuerySingleOrDefaultAsync<ReadMateriaDto>(sql, parametros);
    }
}
