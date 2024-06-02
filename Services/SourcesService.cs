using SourcesStoreApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace SourcesStoreApi.Services;

public class SourcesService
{
    private readonly IMongoCollection<Source> _sourcesCollection;

    public SourcesService(
        IOptions<SourcesStoreDatabaseSettings> sourceStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            sourceStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            sourceStoreDatabaseSettings.Value.DatabaseName);

        _sourcesCollection = mongoDatabase.GetCollection<Source>(
            sourceStoreDatabaseSettings.Value.SourcesCollectionName);
    }

    public async Task<List<Source>> GetAsync() =>
        await _sourcesCollection.Find(_ => true).ToListAsync();

    public async Task<Source?> GetAsync(string id) =>
        await _sourcesCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Source newBook) =>
        await _sourcesCollection.InsertOneAsync(newBook);

    public async Task UpdateAsync(string id, Source updatedBook) =>
        await _sourcesCollection.ReplaceOneAsync(x => x.Id == id, updatedBook);

    public async Task RemoveAsync(string id) =>
        await _sourcesCollection.DeleteOneAsync(x => x.Id == id);
}