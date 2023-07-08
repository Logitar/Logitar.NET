namespace Logitar.Data.UnitTests;

internal class QueryBuilderMock : QueryBuilder
{
  public QueryBuilderMock(TableId source) : base(source)
  {
  }

  protected override string? DefaultSchema => "défaut";
  protected override string? IdentifierPrefix => "«";
  protected override string? IdentifierSuffix => "»";
  protected override string IdentifierSeparator => "·";

  protected override string SelectClause => "SÉLECTIONNER";
  protected override string AllColumnsClause => "Ω";

  protected override string FromClause => "DEPUIS";

  protected override string OrderByClause => "ORDONNER PAR";
  protected override string ThenByClause => "PUIS PAR";
  protected override string AscendingClause => "↑";
  protected override string DescendingClause => "↓";
}
