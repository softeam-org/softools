using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Tasks.v1;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Softools.ServiceDefaults;

public class GoogleAuthService
{
    private readonly string _applicationName;
    private readonly string _credentialFilePath;
    private readonly string _tokenFolderPath;
    private readonly ILogger<GoogleAuthService> _logger;
    private UserCredential _userCredential;
    
    public GoogleAuthService(IConfiguration config, ILogger<GoogleAuthService> logger)
    {
        _applicationName = config["Google:Tasks:ApplicationName"] ?? "Pulse";
        _credentialFilePath = config["Google:Tasks:CredentialFilePath"] ?? ".google.credentials";
        _tokenFolderPath = config["Google:Tasks:TokenFolderPath"] 
                           ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".config", "pulse", "google.token");

        _logger = logger;
    } 
    
    public async Task<UserCredential> AuthorizeAsync(string[] scopes, CancellationToken cancellationToken = default)
    {
        using var stream = new FileStream(_credentialFilePath, FileMode.Open, FileAccess.Read);
        _userCredential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
            GoogleClientSecrets.FromStream(stream).Secrets,
            [CalendarService.Scope.Calendar, TasksService.Scope.Tasks],
            "user",
            cancellationToken,
            new Google.Apis.Util.Store.FileDataStore(_tokenFolderPath, true));

        _logger.LogInformation("Google authorization completed.");
        return _userCredential;
    }
    
    public UserCredential GetUserCredential()
    {
        if (_userCredential == null)
        {
            throw new InvalidOperationException("User is not authorized. Call AuthorizeAsync first.");
        }
        return _userCredential;
    }
    
    
}