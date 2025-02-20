using System.Collections.Concurrent;

namespace QueueMechanism;

public class TaskBox
{
    private readonly ConcurrentQueue<int> _queue = new();
    private readonly HashSet<int> _processingSet = new();
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public async Task Enqueue(int taskId)
    {
        await _semaphore.WaitAsync();
        try
        {
            _queue.Enqueue(taskId);
            _processingSet.Add(taskId);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public bool TryDequeue(out int taskId)
    {
        return _queue.TryDequeue(out taskId);
    }

    public async Task MarkAsProcessed(int taskId)
    {
        await _semaphore.WaitAsync();
        try
        {
            _processingSet.Remove(taskId);
        }
        finally
        {
            _semaphore.Release();
        }
    }
}
