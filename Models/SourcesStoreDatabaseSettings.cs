namespace SourcesStoreApi.Models;

public class SourcesStoreDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string SourcesCollectionName { get; set; } = null!;
}