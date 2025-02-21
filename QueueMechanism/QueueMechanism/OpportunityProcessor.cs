using System.Collections.Concurrent;

namespace QueueMechanism;

public class OpportunityProcessor<T, T1> where T : new()
{
    // _locks keeps a unique semaphore for each unique object (by HashCode) to ensure that only one thread processes a queue at a time
    private readonly ConcurrentDictionary<int, SemaphoreSlim> _locks = new();

    // _queues maintains a queue for each unique object (by HashCode), ensuring tasks are executed in order
    private readonly ConcurrentDictionary<int, ConcurrentQueue<Func<Task<T1>>>> _queues = new();

    public async Task<T1> Enqueue(T value, Func<T, Task<T1>> func)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        int uniqueHashCode = value.GetHashCode();

        ConcurrentQueue<Func<Task<T1>>> queue = _queues.GetOrAdd(uniqueHashCode, _ => new ConcurrentQueue<Func<Task<T1>>>());
        SemaphoreSlim semaphore = _locks.GetOrAdd(uniqueHashCode, _ => new SemaphoreSlim(1, 1));

        TaskCompletionSource<T1> tcs = new();
        queue.Enqueue(async () =>
        {
            try
            {
                T1 result = await func(value);
                tcs.SetResult(result);
                return result;
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
                throw;
            }
        });

        await ProcessQueue(semaphore, queue);

        return await tcs.Task;
    }

    // ProcessQueue method attempts to acquire the semaphore (ensuring that only one thread processes the queue at a time).
    private async Task ProcessQueue(SemaphoreSlim semaphore, ConcurrentQueue<Func<Task<T1>>> queue)
    {
        // if another thread is already processing, the new thread exits immediately
        if (!await semaphore.WaitAsync(0))
        {
            return;
        }

        try
        {
            // the loop ensures that all tasks are processed sequentially
            while (queue.TryDequeue(out var taskFunc))
            {
                await taskFunc();
            }
        }
        finally
        {
            // Once the queue is empty, the semaphore is released.
            semaphore.Release();
        }
    }
}
