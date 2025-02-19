namespace QueueMechanism;

public class Opportunity
{
    public async Task Process(int opportunityId)
    {
        Console.WriteLine($"Processing opportunity {opportunityId}");
        await Task.Delay(1000); // Simulating async work
    }
}
