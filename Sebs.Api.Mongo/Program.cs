using Sebs.Api.Mongo.Data.Contexts;

var builder = WebApplication.CreateBuilder(args);

// Instantiate Mongo Db connection
builder.Services.AddSingleton<ISampleContext>(provider =>
{
    var connectionString = builder.Configuration.GetValue<string>("DatabaseSettings:ConnectionString");
    var databaseName = builder.Configuration.GetValue<string>("DatabaseSettings:DatabaseName");
    var collectionName = builder.Configuration.GetValue<string>("DatabaseSettings:CollectionName");
    return new SampleContext(connectionString, databaseName, collectionName);
});


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

app.MapControllers();

app.Run();
