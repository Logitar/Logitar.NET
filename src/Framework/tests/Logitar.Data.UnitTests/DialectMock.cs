namespace Logitar.Data;

public record DialectMock : Dialect
{
  public override string? DefaultSchema => "défaut";
  public override string? IdentifierPrefix => "«";
  public override string? IdentifierSuffix => "»";
  public override string IdentifierSeparator => "·";
  public override string? ParameterPrefix => "Π";
  public override string? ParameterSuffix => "Θ";

  public override string SelectClause => "SÉLECTIONNER";
  public override string AllColumnsClause => "Ω";
  public override string DeleteFromClause => "SUPPRIMER DE";
  public override string InsertIntoClause => "INSÉRER DANS";
  public override string ValuesClause => "VALEURS";
  public override string UpdateClause => "MODIFIER";
  public override string SetClause => "DÉFINIR";

  public override string FromClause => "DEPUIS";

  public override string OnClause => "SUR";

  public override string WhereClause => "OÙ";
  public override string IsClause => "EST";
  public override string NotClause => "NON";
  public override string BetweenClause => "DANS L'INTERVALLE";
  public override string InClause => "DANS";
  public override string LikeClause => "COMME";
  public override string NullClause => "NUL";

  public override string OrderByClause => "ORDONNER PAR";
  public override string AscendingClause => "↑";
  public override string DescendingClause => "↓";
}
