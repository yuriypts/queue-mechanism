using System.Collections.Concurrent;

namespace QueueMechanism;

public class Opportunity
{
    public int OpportunityId { get; set; }
    public Func<int, Task<bool>> Func { get; set; }
}

public static class Opportunities
{
    public static ConcurrentBag<Opportunity> GetOpportunities(PricingIntegration pricingIntegration)
    {
        return new ConcurrentBag<Opportunity>
        {
            new Opportunity
            {
                OpportunityId = 1,
                Func = async (int opportunityId) => await pricingIntegration.ProcessSolaceMessage(opportunityId)
            },
            new Opportunity
            {
                OpportunityId = 1,
                Func = async (int opportunityId) => await pricingIntegration.ProcessSolaceMessage(opportunityId)
            },
            new Opportunity
            {
                OpportunityId = 1,
                Func = async (int opportunityId) => await pricingIntegration.ProcessSolaceMessage(opportunityId)
            },
            new Opportunity
            {
                OpportunityId = 2,
                Func = async (int opportunityId) => await pricingIntegration.ProcessSolaceMessage(opportunityId)
            },
            new Opportunity
            {
                OpportunityId = 3,
                Func = async (int opportunityId) => await pricingIntegration.ProcessSolaceMessage(opportunityId)
            },
            new Opportunity
            {
                OpportunityId = 3,
                Func = async (int opportunityId) => await pricingIntegration.ProcessSolaceMessage(opportunityId)
            }
        };
    }
}