//using Logitar.Data;
//using Logitar.Data.PostgreSQL;
//using Logitar.EventSourcing.Infrastructure;
//using Microsoft.Extensions.Configuration;
//using Npgsql;
//using System.Data.Common;

//namespace Logitar.EventSourcing.PostgreSQL;

//[Trait(Traits.Category, Categories.Integration)]
//public class AggregateRepositoryTests : Relational.AggregateRepositoryTests
//{
//  protected override DbConnection InitializeDbConnection()
//  {
//    IConfiguration configuration = new ConfigurationBuilder()
//      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
//      .Build();

//    string connectionString = configuration.GetValue<string>("POSTGRESQLCONNSTR_IntegrationTests")
//      ?? throw new InvalidOperationException("The configuration 'POSTGRESQLCONNSTR_IntegrationTests' has not been set.");

//    return new NpgsqlConnection(connectionString);
//  }

//  protected override IQueryBuilder CreateQueryBuilder(TableId source)
//  {
//    return new PostgresQueryBuilder(source);
//  }

//  protected override Relational.AggregateRepository CreateRepository(IEventBus eventBus)
//  {
//    return new AggregateRepository(Connection, eventBus);
//  }
//}
