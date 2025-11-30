namespace Logitar;

/// <summary>
/// Defines helper methods for environment variables.
/// </summary>
public static class EnvironmentHelper
{
  /// <summary>
  /// Retrieves the boolean value of an environment variable.
  /// </summary>
  /// <param name="variable">The name of the variable.</param>
  /// <param name="defaultValue">The default value.</param>
  /// <returns>The value of the environment value.</returns>
  public static bool GetBoolean(string variable, bool defaultValue = false) => TryGetBoolean(variable) ?? defaultValue;
  /// <summary>
  /// Retrieves the boolean value of an environment variable. Returns null if it is not found.
  /// </summary>
  /// <param name="variable">The name of the variable.</param>
  /// <returns>The value of the environment value, or null if not found.</returns>
  public static bool? TryGetBoolean(string variable)
  {
    string value = Environment.GetEnvironmentVariable(variable);
    return !string.IsNullOrWhiteSpace(value) && bool.TryParse(value.Trim(), out bool boolean) ? boolean : null;
  }

  /// <summary>
  /// Retrieves the enum value of an environment variable.
  /// </summary>
  /// <param name="variable">The name of the variable.</param>
  /// <param name="defaultValue">The default value.</param>
  /// <returns>The value of the environment value.</returns>
  public static T GetEnum<T>(string variable, T defaultValue = default) where T : struct, Enum
  {
    return TryGetEnum<T>(variable) ?? defaultValue;
  }
  /// <summary>
  /// Retrieves the enum value of an environment variable. Returns null if it is not found.
  /// </summary>
  /// <param name="variable">The name of the variable.</param>
  /// <returns>The value of the environment value, or null if not found.</returns>
  public static T? TryGetEnum<T>(string variable) where T : struct, Enum
  {
    string value = Environment.GetEnvironmentVariable(variable);
    return !string.IsNullOrWhiteSpace(value) && Enum.TryParse(value, out T enumValue) && Enum.IsDefined(typeof(T), enumValue) ? enumValue : null;
  }

  /// <summary>
  /// Retrieves the integer value of an environment variable.
  /// </summary>
  /// <param name="variable">The name of the variable.</param>
  /// <param name="defaultValue">The default value.</param>
  /// <returns>The value of the environment value.</returns>
  public static int GetInt32(string variable, int defaultValue = 0) => TryGetInt32(variable) ?? defaultValue;
  /// <summary>
  /// Retrieves the integer value of an environment variable. Returns null if it is not found.
  /// </summary>
  /// <param name="variable">The name of the variable.</param>
  /// <returns>The value of the environment value, or null if not found.</returns>
  public static int? TryGetInt32(string variable)
  {
    string value = Environment.GetEnvironmentVariable(variable);
    return !string.IsNullOrWhiteSpace(value) && int.TryParse(value.Trim(), out int integer) ? integer : null;
  }

  /// <summary>
  /// Retrieves the string value of an environment variable.
  /// </summary>
  /// <param name="variable">The name of the variable.</param>
  /// <param name="defaultValue">The default value.</param>
  /// <returns>The value of the environment value.</returns>
  public static string GetString(string variable, string defaultValue = "") => TryGetString(variable) ?? defaultValue;
  /// <summary>
  /// Retrieves the string value of an environment variable. Returns null if it is not found.
  /// </summary>
  /// <param name="variable">The name of the variable.</param>
  /// <returns>The value of the environment value, or null if not found.</returns>
  public static string? TryGetString(string variable) => Environment.GetEnvironmentVariable(variable)?.CleanTrim();

  /// <summary>
  /// Retrieves the TimeSpan value of an environment variable.
  /// </summary>
  /// <param name="variable">The name of the variable.</param>
  /// <param name="defaultValue">The default value.</param>
  /// <returns>The value of the environment value.</returns>
  public static TimeSpan GetTimeSpan(string variable, TimeSpan defaultValue = default) => TryGetTimeSpan(variable) ?? defaultValue;
  /// <summary>
  /// Retrieves the TimeSpan value of an environment variable. Returns null if it is not found.
  /// </summary>
  /// <param name="variable">The name of the variable.</param>
  /// <returns>The value of the environment value, or null if not found.</returns>
  public static TimeSpan? TryGetTimeSpan(string variable)
  {
    string value = Environment.GetEnvironmentVariable(variable);
    return !string.IsNullOrWhiteSpace(value) && TimeSpan.TryParse(value.Trim(), out TimeSpan timeSpan) ? timeSpan : null;
  }
}
