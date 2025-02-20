namespace QueueMechanism;

public class Repository
{
    public int Sum { get; private set; }

    public Repository(int value)
    {
        Sum = value;
    }

    public int Get()
    {
        return Sum;
    }

    public void Add(int value)
    {
        Sum = value;
    }
}

public static class Repositoris
{
    private static readonly Repository _repository = new(0);
    private static readonly Repository _repository1 = new(0);
    private static readonly Repository _repository2 = new(0);

    public static Repository GetRepository(int opportunityId)
    {
        return opportunityId switch
        {
            1 => _repository,
            2 => _repository1,
            3 => _repository2,
            _ => throw new ArgumentException("Invalid opportunityId"),
        };
    }

    public static List<Repository> GetRepositories()
    {
        return new List<Repository> { _repository, _repository1, _repository2 };
    }
}
