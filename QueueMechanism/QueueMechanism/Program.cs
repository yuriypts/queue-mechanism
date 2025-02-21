using System.Collections.Concurrent;
using System.Diagnostics;

namespace QueueMechanism;

public class Program
{
    private static PricingIntegration _pricingIntegration = new();

    static async Task Main(string[] args)
    {
        ConcurrentBag<Opportunity> _opportunities = Opportunities.GetOpportunities(_pricingIntegration);
        var processor = new OpportunityProcessor<int, bool>();

        Stopwatch sw = new();
        sw.Start();

        // 1 Approach - With Queue Mechanism (Task WhenAll)
        //List<Task> tasks = new();
        //foreach (Opportunity opportunity in _opportunities)
        //{
        //    tasks.Add(processor.Enqueue(opportunity.OpportunityId, async (int opportunityId) => await opportunity.Func(opportunityId)));
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

        sw.Stop();
        Console.WriteLine($"Running time - {sw.Elapsed}");

        foreach (Repository repository in Repositoris.GetRepositories())
        {
            Console.WriteLine($"Result - {repository.GetHashCode()} - {repository.Sum}");
        }
    }
}
