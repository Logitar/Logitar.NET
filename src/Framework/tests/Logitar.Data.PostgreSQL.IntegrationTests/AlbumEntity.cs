namespace Logitar.Data.PostgreSQL;

internal record AlbumEntity
{
  public long AlbumId { get; set; }
  public long ArtistId { get; set; }
  public string Genre { get; set; } = string.Empty;
  public int ReleaseYear { get; set; }
  public string Title { get; set; } = string.Empty;

  public ArtistEntity? Artist { get; set; }
  public List<ReviewEntity> Reviews { get; } = new();
}
