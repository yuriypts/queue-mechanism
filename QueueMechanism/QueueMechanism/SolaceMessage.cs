namespace QueueMechanism;

public class SolaceMessage
{
    public Guid CorrelationId { get; set; }
    public string MessageType { get; set; }
    public string? Message { get; set; }
    public string? ResponseUrl { get; set; }
}
