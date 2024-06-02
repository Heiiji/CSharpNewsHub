using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace NewsStoreApi.Models;

public class News
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("Title")]
    [JsonPropertyName("Title")]
    public string Title { get; set; } = null!;

    public string Link { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Source { get; set; } = null!;

    public string date { get; set; } = null!;
}