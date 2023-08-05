namespace Logitar.Data;

internal class InsertBuilderMock : InsertBuilder
{
  public InsertBuilderMock(params ColumnId[] columns) : base(columns)
  {
  }

  public override Dialect Dialect { get; set; } = new DialectMock();

  protected override object CreateParameter(IParameter parameter) => parameter;
}
