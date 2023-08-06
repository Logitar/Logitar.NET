namespace Logitar.Data.PostgreSQL;

internal record ReviewEntity
{
  public long ReviewId { get; set; }
  public long AlbumId { get; set; }
  public bool IsPublished { get; set; }
  public int Note { get; set; }
  public string? Text { get; set; }

  public AlbumEntity? Album { get; set; }
}
