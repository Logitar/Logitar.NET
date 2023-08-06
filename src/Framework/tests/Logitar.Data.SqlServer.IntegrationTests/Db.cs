namespace Logitar.Data.SqlServer;

internal static class Db
{
  internal static class Artists
  {
    public static readonly TableId Table = new("Artists");

    public static readonly ColumnId ArtistId = new(nameof(ArtistEntity.ArtistId), Table);
    public static readonly ColumnId Country = new(nameof(ArtistEntity.Country), Table);
    public static readonly ColumnId Name = new(nameof(ArtistEntity.Name), Table);
  }

  internal static class Albums
  {
    public static readonly TableId Table = new("Albums");

    public static readonly ColumnId AlbumId = new(nameof(AlbumEntity.AlbumId), Table);
    public static readonly ColumnId ArtistId = new(nameof(AlbumEntity.ArtistId), Table);
    public static readonly ColumnId Genre = new(nameof(AlbumEntity.Genre), Table);
    public static readonly ColumnId ReleaseYear = new(nameof(AlbumEntity.ReleaseYear), Table);
    public static readonly ColumnId Title = new(nameof(AlbumEntity.Title), Table);
  }

  internal static class Reviews
  {
    public static readonly TableId Table = new("Reviews");

    public static readonly ColumnId AlbumId = new(nameof(ReviewEntity.AlbumId), Table);
    public static readonly ColumnId IsPublished = new(nameof(ReviewEntity.IsPublished), Table);
    public static readonly ColumnId Note = new(nameof(ReviewEntity.Note), Table);
    public static readonly ColumnId ReviewId = new(nameof(ReviewEntity.ReviewId), Table);
    public static readonly ColumnId Text = new(nameof(ReviewEntity.Text), Table);
  }
}
