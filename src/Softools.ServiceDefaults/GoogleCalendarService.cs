using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Softools.ServiceDefaults;

public class GoogleCalendarService
{
    private CalendarService _calendarService;
    private readonly string _applicationName;
    private readonly string _credentialFilePath;
    private readonly string _tokenFolderPath;
    private readonly string _timeZone;
    private readonly ILogger<GoogleCalendarService> _logger;
    private readonly GoogleAuthService _googleAuthService;

    public GoogleCalendarService(IConfiguration config, GoogleAuthService googleAuthService)
    {
        _timeZone = config["Google:Calendar:TimeZone"] ?? "America/Sao_Paulo";
        _googleAuthService = googleAuthService;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        var credential = _googleAuthService.GetUserCredential();
        _calendarService = new CalendarService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = _applicationName
        });
        
        _logger?.LogInformation("Google Calendar service initialized.");
    }

    /// <summary>
    /// Adds an event to the specified calendar.
    /// </summary>
    public async Task<Event> AddEventAsync(string calendarId, string summary, DateTime start, DateTime end, IList<EventAttendee>? attendees = null, IList<EventReminder>? reminders = null, CancellationToken cancellationToken = default)
    {
        var newEvent = new Event
        {
            Summary = summary,
            Start = new EventDateTime { DateTime = start, TimeZone = _timeZone },
            End = new EventDateTime { DateTime = end, TimeZone = _timeZone },
            Attendees = attendees,
            Reminders = reminders is null
                ? new Event.RemindersData { UseDefault = true }
                : new Event.RemindersData { UseDefault = false, Overrides = reminders }
        };

        var request = _calendarService.Events.Insert(newEvent, calendarId);
        request.SendUpdates = EventsResource.InsertRequest.SendUpdatesEnum.All;
        
        _logger?.LogInformation("Adding event '{EventSummary}' to calendar '{CalendarId}'", summary, calendarId);

        return await request.ExecuteAsync(cancellationToken);
    }

    /// <summary>
    /// Gets all upcoming events from the calendar.
    /// </summary>
    public async Task<IList<Event>> GetAllEventsAsync(string calendarId, CancellationToken cancellationToken = default)
    {
        var request = _calendarService.Events.List(calendarId);
        request.TimeMin = DateTime.UtcNow;
        request.ShowDeleted = false;
        request.SingleEvents = true;
        request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

        var result = await request.ExecuteAsync(cancellationToken);
        _logger?.LogInformation("Retrieved {EventCount} upcoming events from calendar '{CalendarId}'", result.Items?.Count ?? 0, calendarId);
        return result.Items ?? new List<Event>();
    }

    /// <summary>
    /// Gets an event by ID.
    /// </summary>
    public async Task<Event?> GetEventAsync(string calendarId, string eventId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogInformation("Retrieving event '{EventId}' from calendar '{CalendarId}'", eventId, calendarId);
            return await _calendarService.Events.Get(calendarId, eventId).ExecuteAsync(cancellationToken);
        }
        catch (Google.GoogleApiException e) when (e.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    /// <summary>
    /// Updates an existing event.
    /// </summary>
    public async Task<Event> UpdateEventAsync(string calendarId, Event eventToUpdate, CancellationToken cancellationToken = default)
    {
        var request = _calendarService.Events.Update(eventToUpdate, calendarId, eventToUpdate.Id);
        request.SendUpdates = EventsResource.UpdateRequest.SendUpdatesEnum.All;
        
        _logger?.LogInformation("Updating event '{EventId}' in calendar '{CalendarId}'", eventToUpdate.Id, calendarId);

        return await request.ExecuteAsync(cancellationToken);
    }

    /// <summary>
    /// Deletes an event.
    /// </summary>
    public async Task DeleteEventAsync(string calendarId, string eventId, CancellationToken cancellationToken = default)
    {
        var request = _calendarService.Events.Delete(calendarId, eventId);
        request.SendUpdates = EventsResource.DeleteRequest.SendUpdatesEnum.All;
        
        _logger?.LogInformation("Deleting event '{EventId}' from calendar '{CalendarId}'", eventId, calendarId);

        await request.ExecuteAsync(cancellationToken);
    }
}
