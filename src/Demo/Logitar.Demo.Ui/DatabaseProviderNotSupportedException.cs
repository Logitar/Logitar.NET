namespace Logitar.Demo.Ui;

internal class DatabaseProviderNotSupportedException : Exception
{
  public DatabaseProviderNotSupportedException(DatabaseProvider databaseProvider)
    : base($"The database provider '{databaseProvider}' is not supported.")
  {

  }

  public DatabaseProvider DatabaseProvider
  {
    get => (DatabaseProvider)Data[nameof(DatabaseProvider)]!;
    set => Data[nameof(DatabaseProvider)] = value;
  }
}
