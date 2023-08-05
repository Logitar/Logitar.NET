namespace Logitar.EventSourcing.MongoDB;

/// <summary>
/// Represents the connection settings to MongoDB.
/// </summary>
public record MongoDBSettings
{
  /// <summary>
  /// Gets or sets the connection string, or MongoDB URI, to the MongoDB server.
  /// </summary>
  public string ConnectionString { get; set; } = string.Empty;
  /// <summary>
  /// Gets or sets the name of the MongoDB database.
  /// </summary>
  public string DatabaseName { get; set; } = string.Empty;
}
