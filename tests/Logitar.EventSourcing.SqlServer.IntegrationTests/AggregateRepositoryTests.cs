//using Logitar.Data;
//using Logitar.Data.SqlServer;
//using Logitar.EventSourcing.Infrastructure;
//using Microsoft.Data.SqlClient;
//using Microsoft.Extensions.Configuration;
//using System.Data.Common;

//namespace Logitar.EventSourcing.SqlServer;

//[Trait(Traits.Category, Categories.Integration)]
//public class AggregateRepositoryTests : Relational.AggregateRepositoryTests
//{
//  protected override DbConnection InitializeDbConnection()
//  {
//    IConfiguration configuration = new ConfigurationBuilder()
//      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
//      .Build();

//    string connectionString = configuration.GetValue<string>("SQLCONNSTR_IntegrationTests")
//      ?? throw new InvalidOperationException("The configuration 'SQLCONNSTR_IntegrationTests' has not been set.");

//    return new SqlConnection(connectionString);
//  }

//  protected override IQueryBuilder CreateQueryBuilder(TableId source)
//  {
//    return new SqlServerQueryBuilder(source);
//  }

//  protected override Relational.AggregateRepository CreateRepository(IEventBus eventBus)
//  {
//    return new AggregateRepository(Connection, eventBus);
//  }
//}
