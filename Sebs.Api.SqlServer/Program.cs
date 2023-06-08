using Microsoft.EntityFrameworkCore;
using Sebs.Api.SqlServer;
using Sebs.Api.SqlServer.Data.Contexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<SampleContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetValue<string>("SqlServerSettings:ConnectionString"));
});
//Initial migration of SQL database
builder.Services.MigrateDatabase<SampleContext>();


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

app.UseAuthorization();

app.MapControllers();

app.Run();
