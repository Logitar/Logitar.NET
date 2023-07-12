namespace Logitar.Data;

internal class InsertBuilderMock : InsertBuilder
{
  public InsertBuilderMock(params ColumnId[] columns) : base(columns)
  {
  }

  protected override string? DefaultSchema => "défaut";
  protected override string? IdentifierPrefix => "«";
  protected override string? IdentifierSuffix => "»";
  protected override string IdentifierSeparator => "·";
  protected override string? ParameterPrefix => "Π";
  protected override string? ParameterSuffix => "Θ";

  protected override string InsertIntoClause => "INSÉRER DANS";
  protected override string ValuesClause => "VALEURS";

  protected override string NullClause => "NUL";

  protected override object CreateParameter(IParameter parameter) => parameter;
}
