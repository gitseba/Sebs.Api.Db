using Microsoft.Extensions.Caching.Distributed;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Redis 
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.InstanceName = "Sample Redis App";
    options.Configuration = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
});


//SAMPLE CODE for seed redis with temp data -> DEMO Purpose
var serviceProvider = builder.Services.BuildServiceProvider();
var distributedCache = serviceProvider.GetRequiredService<IDistributedCache>();

var options = new DistributedCacheEntryOptions();
options.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(360);
//If data inside cache is not used for some amount of time, use the slide expiration 
options.SlidingExpiration = TimeSpan.FromSeconds(60);

var defaultData = new Dictionary<string, string>()
{
    { "A", "a" },
    { "B", "b" },
    // Add more key-value pairs as needed
};
foreach (var kvp in defaultData)
{
    await distributedCache.SetStringAsync(kvp.Key, kvp.Value, options);
}
/************************************************************/




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
