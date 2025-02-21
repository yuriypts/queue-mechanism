namespace QueueMechanism;


public class PricingIntegration
{
    public async Task<bool> ProcessSolaceMessage(int opportunityId)
    {
        Repository repository = Repositoris.GetRepository(opportunityId);

        Console.WriteLine($"Has unique code for Repositoy - {repository.GetHashCode()}");

        Console.WriteLine($"Started for opportunityId - {opportunityId}");

        int number = repository.Get();

        Console.WriteLine($"Number before update - {number} - for opportunityId - {opportunityId} ");

        int result = await Calculate(number);

        Console.WriteLine($"Number after update - {result} - for opportunityId - {opportunityId} ");

        repository.Add(result);

        Console.WriteLine(new string('-', 50));

        return true;
    }

    private async Task<int> Calculate(int number)
    {
        number++;

        await Task.Delay(500);

        return number;
    }
}
