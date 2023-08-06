namespace Logitar.Data.PostgreSQL;

internal record ArtistEntity
{
  public long ArtistId { get; set; }
  public string Name { get; set; } = string.Empty;
  public string Country { get; set; } = string.Empty;

  public List<AlbumEntity> Albums { get; } = new();
}
