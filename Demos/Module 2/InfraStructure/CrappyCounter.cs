namespace InfraStructure;

public class CrappyCounter : ICounter
{
    private int _counter = 0;
    public void Increment()
    {
        _counter--;
    }

    public void Show()
    {
        Console.WriteLine($"Counter value is {_counter}");
    }
}
