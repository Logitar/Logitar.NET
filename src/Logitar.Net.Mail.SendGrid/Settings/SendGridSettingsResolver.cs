using Microsoft.Extensions.Configuration;

namespace Logitar.Net.Mail.SendGrid.Settings;

/// <summary>
/// An implementation of a SendGrid API settings resolver using the application configuration.
/// </summary>
public class SendGridSettingsResolver : ISendGridSettingsResolver
{
  /// <summary>
  /// Gets or sets the configuration of the application.
  /// </summary>
  protected virtual IConfiguration Configuration { get; }
  /// <summary>
  /// Gets or sets the cached SendGrid API settings.
  /// </summary>
  protected virtual ISendGridSettings? SendGridSettings { get; set; }

  /// <summary>
  /// Initializes a new instance of the <see cref="SendGridSettingsResolver"/> class.
  /// </summary>
  /// <param name="configuration">The configuration of the application.</param>
  public SendGridSettingsResolver(IConfiguration configuration)
  {
    Configuration = configuration;
  }

  /// <summary>
  /// Resolves the SendGrid API settings.
  /// </summary>
  /// <returns>The SendGrid API settings.</returns>
  public ISendGridSettings Resolve()
  {
    SendGridSettings ??= Configuration.GetSection("SendGrid").Get<SendGridSettings>() ?? new();
    return SendGridSettings;
  }
}
