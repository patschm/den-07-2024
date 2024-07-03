
namespace Vullis;

internal class UnmanagedResource : IDisposable
{
    private static bool isOpen = false;
    private FileStream? _stream;

    public void Open()
    {
        Console.WriteLine("Openen....");
        if (isOpen)
        {
            Console.WriteLine("Helaas. In gebruik");
            return;
        }
        isOpen = true;
        _stream = File.Open("bla.txt", FileMode.OpenOrCreate);
        Console.WriteLine("Is Open");
    }
    public void Close()
    {
        Console.WriteLine("Closing...");
        isOpen = false;
        Console.WriteLine("Closed");
    }

    protected void RuimOp(bool fromFinalizer)
    {
        Close();
        if (!fromFinalizer)
        {
            _stream?.Dispose();
        }
    }

    public void Dispose()
    {
        RuimOp(false);
        GC.SuppressFinalize(this);
    }

    ~UnmanagedResource()
    {
        RuimOp(true);
    }
}
