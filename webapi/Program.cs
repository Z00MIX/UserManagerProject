using System.Data;
using System.Data.SQLite;
using webapi.Interfaces;
using webapi.Repositories;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddScoped<IDbConnection>((sp)=>new SQLiteConnection(connectionString));
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddControllers().AddRazorRuntimeCompilation();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
