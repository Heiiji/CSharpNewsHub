using NewsStoreApi.Models;
using SourcesStoreApi.Models;

using NewsStoreApi.Services;
using SourcesStoreApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<NewsStoreDatabaseSettings>(
    builder.Configuration.GetSection("NewsStoreDatabase"));
builder.Services.Configure<SourcesStoreDatabaseSettings>(
    builder.Configuration.GetSection("SourcesStoreDatabase"));

builder.Services.AddSingleton<NewsService>();
builder.Services.AddSingleton<SourcesService>();

builder.Services.AddControllers().AddJsonOptions(
            options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

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
app.MapControllers();

app.Run();