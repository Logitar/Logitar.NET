using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data.Common;
using System.Text;

namespace Logitar.Data.SqlServer;

[Trait(Traits.Category, Categories.Integration)]
public class SqlServerBuildersTests : IAsyncLifetime
{
  private const string ConnectionStringKey = "SQLCONNSTR_IntegrationTests";

  private readonly DbConnection _connection;

  public SqlServerBuildersTests()
  {
    IConfiguration configuration = new ConfigurationBuilder()
      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
      .Build();

    string connectionString = configuration.GetValue<string>(ConnectionStringKey)
      ?? throw new InvalidOperationException($"The configuration '{ConnectionStringKey}' has not been set.");

    _connection = new SqlConnection(connectionString);
  }

  [Fact(DisplayName = "It should delete the correct rows.")]
  public async Task It_should_delete_the_correct_rows()
  {
    ICommand delete = SqlServerDeleteBuilder.From(Db.Artists.Table)
      .Where(new OperatorCondition(Db.Artists.Name, Operators.IsLike("%dream theater%")))
      .Build();

    using DbCommand command = _connection.CreateCommand();
    command.CommandText = delete.Text;
    command.Parameters.AddRange(delete.Parameters.ToArray());
    int rows = await command.ExecuteNonQueryAsync();
    Assert.Equal(1, rows);

    IEnumerable<AlbumEntity> albums = await ReadAllAlbumsAsync();
    Assert.DoesNotContain(albums, album => album.Artist == null || album.Artist.Name == "Dream Theater");
  }

  [Fact(DisplayName = "It should insert the correct rows.")]
  public async Task It_should_insert_the_correct_rows()
  {
    ReviewEntity review = new()
    {
      ReviewId = 503771,
      AlbumId = 1022587,
      IsPublished = false,
      Note = 3,
      Text = "Yeah, I can't agree with myself on where to rank SepticFlesh on the scale of Greek export [...]."
    };
    ICommand insert = SqlServerInsertBuilder.Into(Db.Reviews.ReviewId, Db.Reviews.AlbumId, Db.Reviews.IsPublished, Db.Reviews.Note, Db.Reviews.Text)
      .Value(review.ReviewId, review.AlbumId, review.IsPublished ? 1 : 0, review.Note, review.Text)
      .Build();

    using DbCommand command = _connection.CreateCommand();
    command.CommandText = insert.Text;
    command.Parameters.AddRange(insert.Parameters.ToArray());
    int rows = await command.ExecuteNonQueryAsync();
    Assert.Equal(1, rows);

    IEnumerable<ReviewEntity> reviews = await ReadAllReviewsAsync();
    ReviewEntity newReview = reviews.Single(r => r.ReviewId == review.ReviewId);
    Assert.Equal(review.AlbumId, newReview.AlbumId);
    Assert.Equal(review.IsPublished, newReview.IsPublished);
    Assert.Equal(review.Note, newReview.Note);
    Assert.Equal(review.Text, newReview.Text);
  }

  [Fact(DisplayName = "It should select the correct rows.")]
  public async Task It_should_select_the_correct_rows()
  {
    IQuery query = SqlServerQueryBuilder.From(Db.Artists.Table)
      .Join(Db.Albums.ArtistId, Db.Artists.ArtistId)
      .Join(Db.Reviews.AlbumId, Db.Albums.AlbumId, new AndCondition(
        new OperatorCondition(Db.Reviews.IsPublished, Operators.IsEqualTo(1)),
        new OperatorCondition(Db.Reviews.Text, Operators.IsNotNull())
      ))
      .WhereOr(
        new OperatorCondition(Db.Artists.Name, Operators.IsLike("%drE_m%")),
        new OperatorCondition(Db.Artists.Country, Operators.IsLike("CaNaDa")),
        new OperatorCondition(Db.Albums.Title, Operators.IsLike("%drE_m%"))
      )
      .Where(Db.Albums.Genre, Operators.IsIn("Progressive Metal", "Progressive Deathcore", "Technical Death Metal"))
      .WhereOr(
        new OperatorCondition(Db.Albums.ReleaseYear, Operators.IsLessThanOrEqualTo(2010)),
        new OperatorCondition(Db.Albums.ReleaseYear, Operators.IsGreaterThan(2020))
      )
      .Where(Db.Reviews.Note, Operators.IsBetween(2, 4))
      .Select(Db.Albums.AlbumId, Db.Albums.ArtistId, Db.Albums.ReleaseYear, Db.Albums.Title, Db.Albums.Genre)
      .OrderBy(new OrderBy(Db.Albums.ReleaseYear, isDescending: true),
        new OrderBy(Db.Artists.Name), new OrderBy(Db.Albums.Title)
      )
      .Build();

    using DbCommand command = _connection.CreateCommand();
    command.CommandText = query.Text;
    command.Parameters.AddRange(query.Parameters.ToArray());

    List<AlbumEntity> albums = new();
    using DbDataReader reader = await command.ExecuteReaderAsync();
    if (reader.HasRows)
    {
      while (await reader.ReadAsync())
      {
        albums.Add(new AlbumEntity
        {
          AlbumId = reader.GetInt64(0),
          ArtistId = reader.GetInt64(1),
          ReleaseYear = reader.GetInt32(2),
          Title = reader.GetString(3),
          Genre = reader.GetString(4)
        });
      }
    }

    Assert.Equal(3, albums.Count);
    Assert.Equal("Bleed the Future", albums.ElementAt(0).Title);
    Assert.Equal("In Dreams", albums.ElementAt(1).Title);
    Assert.Equal("Systematic Chaos", albums.ElementAt(2).Title);
  }

  [Fact(DisplayName = "It should update the correct rows.")]
  public async Task It_should_update_the_correct_rows()
  {
    ICommand update = new SqlServerUpdateBuilder()
      .Set(new Update(Db.Reviews.IsPublished, 1))
      .Where(new OperatorCondition(Db.Reviews.IsPublished, Operators.IsNotEqualTo(1)))
      .Where(new OperatorCondition(Db.Reviews.Note, Operators.IsGreaterThanOrEqualTo(3)))
      .Build();

    using DbCommand command = _connection.CreateCommand();
    command.CommandText = update.Text;
    command.Parameters.AddRange(update.Parameters.ToArray());
    int rows = await command.ExecuteNonQueryAsync();
    Assert.Equal(1, rows);

    IEnumerable<ReviewEntity> reviews = await ReadAllReviewsAsync();
    foreach (ReviewEntity review in reviews)
    {
      if (review.Note >= 3)
      {
        Assert.True(review.IsPublished);
      }
    }
    Assert.Contains(reviews, review => !review.IsPublished);
  }

  public async Task InitializeAsync()
  {
    await _connection.OpenAsync();

    string script = await File.ReadAllTextAsync("Init.sql", Encoding.UTF8);
    string[] statements = script.Split("GO");

    using DbCommand command = _connection.CreateCommand();
    foreach (string statement in statements)
    {
      command.CommandText = statement;
      await command.ExecuteNonQueryAsync();
    }
  }

  public async Task DisposeAsync()
  {
    await _connection.DisposeAsync();
  }

  private async Task<IEnumerable<AlbumEntity>> ReadAllAlbumsAsync()
  {
    using DbCommand command = _connection.CreateCommand();
    command.CommandText = @"SELECT al.[AlbumId],
  al.[ReleaseYear],
  al.[Title],
  al.[Genre],
  ar.[ArtistId],
  ar.[Name],
  ar.[Country]
FROM [Albums] al
JOIN [Artists] ar ON ar.[ArtistId] = al.[ArtistId]";

    Dictionary<long, ArtistEntity> artists = new();
    Dictionary<long, AlbumEntity> albums = new();
    using DbDataReader reader = await command.ExecuteReaderAsync();
    if (reader.HasRows)
    {
      while (await reader.ReadAsync())
      {
        long artistId = reader.GetInt64(4);
        if (!artists.TryGetValue(artistId, out ArtistEntity? artist))
        {
          artist = new()
          {
            ArtistId = artistId,
            Name = reader.GetString(5),
            Country = reader.GetString(6)
          };
          artists.Add(artistId, artist);
        }

        long albumId = reader.GetInt64(0);
        if (!albums.TryGetValue(albumId, out AlbumEntity? album))
        {
          album = new()
          {
            AlbumId = albumId,
            Artist = artist,
            ArtistId = artist.ArtistId,
            ReleaseYear = reader.GetInt32(1),
            Title = reader.GetString(2),
            Genre = reader.GetString(3)
          };
          albums.Add(albumId, album);

          artist.Albums.Add(album);
        }
      }
    }

    return albums.Values;
  }

  private async Task<IEnumerable<ReviewEntity>> ReadAllReviewsAsync()
  {
    using DbCommand command = _connection.CreateCommand();
    command.CommandText = @"SELECT r.[ReviewId],
  r.[IsPublished],
  r.[Note],
  r.[Text],
  al.[AlbumId],
  al.[ReleaseYear],
  al.[Title],
  al.[Genre],
  ar.[ArtistId],
  ar.[Name],
  ar.[Country]
FROM [Reviews] r
JOIN [Albums] al ON al.[AlbumId] = r.[AlbumId]
JOIN [Artists] ar ON ar.[ArtistId] = al.[ArtistId]";

    Dictionary<long, ArtistEntity> artists = new();
    Dictionary<long, AlbumEntity> albums = new();
    Dictionary<long, ReviewEntity> reviews = new();
    using DbDataReader reader = await command.ExecuteReaderAsync();
    if (reader.HasRows)
    {
      while (await reader.ReadAsync())
      {
        long artistId = reader.GetInt64(8);
        if (!artists.TryGetValue(artistId, out ArtistEntity? artist))
        {
          artist = new()
          {
            ArtistId = artistId,
            Name = reader.GetString(9),
            Country = reader.GetString(10)
          };
          artists.Add(artistId, artist);
        }

        long albumId = reader.GetInt64(4);
        if (!albums.TryGetValue(albumId, out AlbumEntity? album))
        {
          album = new()
          {
            AlbumId = albumId,
            Artist = artist,
            ArtistId = artist.ArtistId,
            ReleaseYear = reader.GetInt32(5),
            Title = reader.GetString(6),
            Genre = reader.GetString(7)
          };
          albums.Add(albumId, album);

          artist.Albums.Add(album);
        }

        long reviewId = reader.GetInt64(0);
        if (!reviews.TryGetValue(reviewId, out ReviewEntity? review))
        {
          review = new()
          {
            ReviewId = reviewId,
            Album = album,
            AlbumId = album.AlbumId,
            IsPublished = reader.GetBoolean(1),
            Note = reader.GetInt32(2),
            Text = reader.IsDBNull(3) ? null : reader.GetString(3)
          };
          reviews.Add(reviewId, review);

          album.Reviews.Add(review);
        }
      }
    }

    return reviews.Values;
  }
}
