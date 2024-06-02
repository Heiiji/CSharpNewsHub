using Quartz;
using Quartz.Impl;

using NewsStoreApi.Models;
using SourcesStoreApi.Models;

using NewsStoreApi.Services;
using SourcesStoreApi.Services;
using Quartz.Simpl;

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


// Configuration de Quartz
builder.Services.AddQuartz(q => {
    q.UseJobFactory<MicrosoftDependencyInjectionJobFactory>();

    var jobKey = new JobKey("Crawl", "MyGroup");
    q.AddJob<Crawl>(opts => opts.WithIdentity(jobKey));

    // Créer le trigger pour lancer le job regulierement
    q.AddTrigger(opts => opts
        .ForJob(jobKey) // Lier à la description du job
        .WithIdentity("CrawlTrigger", "MyGroup")
        .WithCronSchedule("0 * * * *") // Excecute toute les heures
    );
});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

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
