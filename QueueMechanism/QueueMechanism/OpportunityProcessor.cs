using System.Collections.Concurrent;

namespace QueueMechanism;

public class OpportunityProcessor
{
    private readonly ConcurrentQueue<int> _queue = new();
    private readonly HashSet<int> _processingSet = new();
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private readonly Opportunity _opportunity = new();

    public async Task Enqueue(int opportunityId)
    {
        await _semaphore.WaitAsync();
        try
        {
            //if (!_processingSet.Contains(opportunityId))
            //{
                _queue.Enqueue(opportunityId);
                _processingSet.Add(opportunityId);
            //}
        }
        finally
        {
            _semaphore.Release();
        }
        _ = ProcessQueue();
    }

    private async Task ProcessQueue()
    {
        while (_queue.TryDequeue(out int opportunityId))
        {
            try
            {
                await _opportunity.Process(opportunityId);
            }
            finally
            {
                await _semaphore.WaitAsync();
                try
                {
                    _processingSet.Remove(opportunityId);
                }
                finally
                {
                    _semaphore.Release();
                }
            }
        }
    }
}
