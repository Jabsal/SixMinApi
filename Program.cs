using System.Reflection.Metadata.Ecma335;
using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SixMinApi.Data;
using SixMinApi.Dtos;
using SixMinApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var sqlConBuilder = new SqlConnectionStringBuilder();

sqlConBuilder.ConnectionString = builder.Configuration.GetConnectionString("SQLDbConnection");

sqlConBuilder.UserID = builder.Configuration["UserId"];
sqlConBuilder.Password = builder.Configuration["Password"];

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(sqlConBuilder.ConnectionString,
        sqlServerOptionsAction:
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 10,
                maxRetryDelay: TimeSpan.FromSeconds(30), 
                errorNumbersToAdd: null);
        });
});
                                    
builder.Services.AddScoped<ICommandRepo, CommandRepo>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("api/v1/Commands", async (ICommandRepo repo, IMapper mapper) =>
{
    var commands = await repo.GetCommandsAsync();
    return Results.Ok(mapper.Map<IEnumerable<CommandReadDto>>(commands));

});

app.MapGet("api/v1/commands{id}", async (ICommandRepo repo, IMapper mapper, int id) =>
{
    var commands = await repo.GetCommandByIdAsync(id);
    if (commands != null)
    {
        return Results.Ok(mapper.Map<CommandReadDto>(commands));
    }

    return Results.NotFound();
});

app.MapPost("api/v1/commands", async (ICommandRepo repo, IMapper mapper, CommandReadDto cmdCreateDto) =>
{
    var commandModel = mapper.Map<Command>(cmdCreateDto);

    await repo.CreateCommand(commandModel);
    await repo.SaveChanges();
});

app.MapPut("api/v1/commands/{id}", async (ICommandRepo repo, IMapper mapper, int id, CommandUpdateDto cmdUpdateDto) =>
{
    var command = await repo.GetCommandByIdAsync(id);
    if (command != null)
    {
        return Results.NotFound();
    }

    mapper.Map(cmdUpdateDto, command);

    await repo.SaveChanges();

    return Results.NoContent();
});

app.MapDelete("api/v1/commands/{id}", async (ICommandRepo repo, IMapper mapper, int id) =>
{
    var command = await repo.GetCommandByIdAsync(id);
    if (command == null)
    {
        return Results.NotFound();
    }

    repo.DeleteCommand(command);

    await repo.SaveChanges();

    return Results.NoContent();
});

app.Run();