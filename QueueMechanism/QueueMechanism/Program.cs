namespace QueueMechanism;

public class Program
{
    static async Task Main(string[] args)
    {
        var processor = new OpportunityProcessor();
        await processor.Enqueue(1);
        await processor.Enqueue(2);
        await processor.Enqueue(1); // Duplicate, will be ignored
        await Task.Delay(5000); // Give time for processing
    }
}
