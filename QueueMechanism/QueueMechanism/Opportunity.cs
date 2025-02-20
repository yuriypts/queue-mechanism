namespace QueueMechanism;

public class Opportunity
{
    public int OpportunityId { get; set; }
    public Func<int, Task<bool>> Func { get; set; }
}
