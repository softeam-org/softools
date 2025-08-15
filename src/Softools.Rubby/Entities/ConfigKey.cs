namespace Softools.Rubby.Entities;

public class ConfigKey
{
    public string Key { get; set; }
    public string? Value { get; set; }
    
    public const string ServerMetricsTrackerMessageId = "server_metrics_tracker_message_id";
    public const string ServerMetricsTrackerChannelId = "server_metrics_tracker_channel_id";
    public const string ServerMetricsUpdateDelay = "server_metrics_update_delay";
}