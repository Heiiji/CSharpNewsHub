using System.ServiceModel.Syndication;
using System.Xml;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using NewsStoreApi.Models;
using Quartz;
using SourcesStoreApi.Models;

public class Crawl : IJob
{
    private readonly IMongoCollection<News> _newsCollection;
    private readonly IMongoCollection<Source> _sourcesCollection;

    public Crawl(
        IOptions<NewsStoreDatabaseSettings> NewsStoreDatabaseSettings, IOptions<SourcesStoreDatabaseSettings> sourceStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            NewsStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            NewsStoreDatabaseSettings.Value.DatabaseName);

        _newsCollection = mongoDatabase.GetCollection<News>(
            NewsStoreDatabaseSettings.Value.NewsCollectionName);

        _sourcesCollection = mongoDatabase.GetCollection<Source>(
            sourceStoreDatabaseSettings.Value.SourcesCollectionName);
    }

    public async Task FetchAndStoreAllRssAsync()
    {
        var sources = await _sourcesCollection.Find(new BsonDocument()).ToListAsync();

        foreach (var source in sources)
        {
            await FetchAndStoreRssAsync(source);
        }
    }

    private async Task FetchAndStoreRssAsync(Source source)
    {
        using var httpClient = new HttpClient();
        var rssData = await httpClient.GetStringAsync(source.URL);

        using var stringReader = new StringReader(rssData);
        using var xmlReader = XmlReader.Create(stringReader);
        var feed = SyndicationFeed.Load(xmlReader);

        var latestNewsDate = DateTime.MinValue;
            
            foreach(var item in feed.Items)
            {
                var newsDate = item.PublishDate.DateTime;
                if (newsDate > latestNewsDate)
                {
                    latestNewsDate = newsDate;
                }

                if (source != null && newsDate > source.update)
                {
                    // Ajout de l'actualité à la base de données
                    var news = new News
                    {
                        Title = item.Title.Text,
                        Link = item.Links.FirstOrDefault()?.Uri.ToString(),
                        Description = item.Summary.Text,
                        Source = source.Name,
                        date = item.PublishDate.DateTime.ToString("o") // ISO 8601 format
                    };
                    await _newsCollection.InsertOneAsync(news);
                }
            };

            // Mettre à jour la date de la dernière mise à jour de la source
            var updateDefinition = Builders<Source>.Update.Set(s => s.update, latestNewsDate);
            await _sourcesCollection.UpdateOneAsync(s => s.Name == source.Name, updateDefinition);
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine("Tâche exécutée.");
        await FetchAndStoreAllRssAsync();
    }
}