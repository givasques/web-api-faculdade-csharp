using FaculdadeApi.Services;
using Npgsql;
using System.Data;
using System.Data.Common;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<DbConnection>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("FaculdadeApi");
    return new NpgsqlConnection(connectionString);
});

// Add services to the container.

builder.Services.AddScoped<AlunoService>();
builder.Services.AddScoped<CursoService>();
builder.Services.AddScoped<TurmaService>();
builder.Services.AddScoped<MateriaService>();
builder.Services.AddScoped<ProfessorService>();
builder.Services.AddScoped<AvaliacaoService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
