using Google.Apis.Services;
using Google.Apis.Tasks.v1;
using Google.Apis.Tasks.v1.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Task = System.Threading.Tasks.Task;

namespace Softools.ServiceDefaults;

public class GoogleTasksService
{
    private TasksService _tasksService;
    private readonly string _applicationName;
    private readonly string _credentialFilePath;
    private readonly string _tokenFolderPath;
    private readonly ILogger<GoogleTasksService> _logger;
    private readonly GoogleAuthService _googleAuthService;

    public GoogleTasksService(IConfiguration config, ILogger<GoogleTasksService> logger, GoogleAuthService googleAuthService)
    {
        _logger = logger;
        _googleAuthService = googleAuthService;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        var credential = _googleAuthService.GetUserCredential();

        _tasksService = new TasksService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = _applicationName
        });

        _logger.LogInformation("Google Tasks service initialized.");
    }

    public async Task<Google.Apis.Tasks.v1.Data.Task> InsertTaskAsync(
        string taskListId,
        string title,
        string? notes = null,
        DateTime? due = null,
        CancellationToken cancellationToken = default)
    {
        var newTask = new Google.Apis.Tasks.v1.Data.Task
        {
            Title = title,
            Notes = notes,
            Due = due.HasValue ? due.Value.ToString("yyyy-MM-dd'T'HH:mm:ss.fff'Z'") : null
        };

        _logger.LogInformation(
            "Inserting task '{TaskTitle}' into task list '{TaskListId}'",
            title,
            "@default");

        var request = _tasksService.Tasks.Insert(newTask, "@default");
        return await request.ExecuteAsync(cancellationToken);
    }

    public async Task<IList<TaskList>> GetTaskListsAsync(CancellationToken cancellationToken = default)
    {
        var request = _tasksService.Tasklists.List();
        var result = await request.ExecuteAsync(cancellationToken);
        
        _logger.LogInformation(
            "Retrieved {TaskListCount} task lists.",
            result.Items?.Count ?? 0);

        return result.Items ?? new List<TaskList>();
    }

    public async Task<IEnumerable<Google.Apis.Tasks.v1.Data.Task>> GetTasksAsync(
        string taskListId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Listing tasks from task list '{TaskListId}'", taskListId);

        var request = _tasksService.Tasks.List(taskListId);
        var result = await request.ExecuteAsync(cancellationToken);

        _logger.LogInformation(
            "Retrieved {TaskCount} tasks from task list '{TaskListId}'",
            result.Items?.Count ?? 0,
            taskListId);

        return result.Items ?? new List<Google.Apis.Tasks.v1.Data.Task>();
    }
    
    public async Task<IEnumerable<Google.Apis.Tasks.v1.Data.Task>> GetCompletedTasksAsync(
        string taskListId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Listing completed tasks from task list '{TaskListId}'", taskListId);

        var request = _tasksService.Tasks.List(taskListId);
        request.ShowHidden = true;
        request.ShowCompleted = true; // Show completed tasks
        var result = await request.ExecuteAsync(cancellationToken);

        _logger.LogInformation(
            "Retrieved {TaskCount} completed tasks from task list '{TaskListId}'",
            result.Items?.Count ?? 0,
            taskListId);

        return result.Items ?? new List<Google.Apis.Tasks.v1.Data.Task>();
    }

    public async Task<Google.Apis.Tasks.v1.Data.Task?> GetTaskAsync(
        string taskListId,
        string taskId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation(
                "Retrieving task '{TaskId}' from task list '{TaskListId}'",
                taskId,
                taskListId);

            return await _tasksService.Tasks.Get(taskListId, taskId).ExecuteAsync(cancellationToken);
        }
        catch (Google.GoogleApiException e) when (e.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
        {
            _logger.LogWarning(
                "Task '{TaskId}' not found in task list '{TaskListId}'",
                taskId,
                taskListId);

            return null;
        }
    }

    public async Task<Google.Apis.Tasks.v1.Data.Task> UpdateTaskAsync(
        string taskListId,
        Google.Apis.Tasks.v1.Data.Task taskToUpdate,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Updating task '{TaskId}' in task list '{TaskListId}'",
            taskToUpdate.Id,
            taskListId);

        var request = _tasksService.Tasks.Update(taskToUpdate, taskListId, taskToUpdate.Id);
        return await request.ExecuteAsync(cancellationToken);
    }

    public async Task DeleteTaskAsync(
        string taskListId,
        string taskId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Deleting task '{TaskId}' from task list '{TaskListId}'",
            taskId,
            taskListId);

        var request = _tasksService.Tasks.Delete(taskListId, taskId);
        await request.ExecuteAsync(cancellationToken);
    }
}
