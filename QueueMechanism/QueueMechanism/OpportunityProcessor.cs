namespace QueueMechanism;

public class OpportunityProcessor<T, T1> where T : new()
{
    private readonly TaskBox _taskBox = new();
    private readonly SemaphoreSlim _processSemaphore = new(1, 1);

    public async Task Enqueue(T uniqueId, Func<int, Task<T1>> func)
    {
        await _taskBox.Enqueue(uniqueId.GetHashCode());
        _ = ProcessQueue(func);
    }

    private async Task ProcessQueue(Func<int, Task<T1>> func)
    {
        await _processSemaphore.WaitAsync();
        try
        {
            while (_taskBox.TryDequeue(out int opportunityId))
            {
                try
                {
                    await func(opportunityId);
                }
                finally
                {
                    await _taskBox.MarkAsProcessed(opportunityId);
                }
            }
        }
        finally
        {
            _processSemaphore.Release();
        }
    }
}

