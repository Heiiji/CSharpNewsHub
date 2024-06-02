using NewsStoreApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace NewsStoreApi.Services;

public class NewsService
{
    private readonly IMongoCollection<News> _newsCollection;

    public NewsService(
        IOptions<NewsStoreDatabaseSettings> NewsStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            NewsStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            NewsStoreDatabaseSettings.Value.DatabaseName);

        _newsCollection = mongoDatabase.GetCollection<News>(
            NewsStoreDatabaseSettings.Value.NewsCollectionName);
    }

    public async Task<List<News>> GetAsync() =>
        await _newsCollection.Find(_ => true).ToListAsync();

    public async Task<News?> GetAsync(string id) =>
        await _newsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(News newNews) =>
        await _newsCollection.InsertOneAsync(newNews);

    public async Task UpdateAsync(string id, News updatedNews) =>
        await _newsCollection.ReplaceOneAsync(x => x.Id == id, updatedNews);

    public async Task RemoveAsync(string id) =>
        await _newsCollection.DeleteOneAsync(x => x.Id == id);
}