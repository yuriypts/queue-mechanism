using System.Collections.Concurrent;

namespace QueueMechanism;

public class Program
{
    private static PricingIntegration _pricingIntegration = new();
    private static ConcurrentBag<Opportunity> _opportunities = new()
    {
        new Opportunity
        {
            OpportunityId = 1,
            Func = (int opportunityId) => _pricingIntegration.ProcessSolaceMessage(opportunityId)
        },
        new Opportunity
        {
            OpportunityId = 1,
            Func = (int opportunityId) => _pricingIntegration.ProcessSolaceMessage(opportunityId)
        },
        new Opportunity
        {
            OpportunityId = 2,
            Func = (int opportunityId) => _pricingIntegration.ProcessSolaceMessage(opportunityId)
        },
        new Opportunity
        {
            OpportunityId = 3,
            Func = (int opportunityId) => _pricingIntegration.ProcessSolaceMessage(opportunityId)
        },
        new Opportunity
        {
            OpportunityId = 3,
            Func = (int opportunityId) => _pricingIntegration.ProcessSolaceMessage(opportunityId)
        }
    };

    static async Task Main(string[] args)
    {
        var processor = new OpportunityProcessor<int, bool>();

        // 1 Approach - With Queue Mechanism (Task WhenAll)
        //List<Task> tasks = new();
        //foreach (Opportunity opportunity in _opportunities)
        //{
        //    tasks.Add(processor.Enqueue(opportunity.OpportunityId, async () => await opportunity.Func()));
        //}
        //await Task.WhenAll(tasks);

        // 2 - Approach - With Queue Mechanism (Parallel ForEach)
        //await Parallel.ForEachAsync(_opportunities, async (Opportunity opportunity, CancellationToken cancellationToken) =>
        //{
        //    await processor.Enqueue(opportunity.OpportunityId, async (int opportunityId) => await opportunity.Func(opportunityId));
        //});

        // 3 - Approach - No Queue Mechanism
        await Parallel.ForEachAsync(_opportunities, async (Opportunity opportunity, CancellationToken cancellationToken) =>
        {
            await _pricingIntegration.ProcessSolaceMessage(opportunity.OpportunityId);
        });

        await Task.Delay(11000); // Ensure all tasks are completed

        foreach (Repository repository in Repositoris.GetRepositories())
        {
            Console.WriteLine($"Result - {repository.GetHashCode()} - {repository.Sum}");
        }
    }
}
