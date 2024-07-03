namespace Vullis;

internal class Program
{
    static UnmanagedResource? um1 = new UnmanagedResource();
    static UnmanagedResource? um2 = new UnmanagedResource();

    static void Main(string[] args)
    {
        try
        {
            um1.Open();
        }
        finally
        {
            um1.Dispose();
            um1 = null;
        }

        using (um2)
        {
            um2.Open();
            
        }
        um2 = null;

        using (var um3 = new UnmanagedResource())
        {
            string x= "hoi";
            
            um3.Open();
            
        }


        GC.Collect();
        GC.WaitForPendingFinalizers();

        Console.ReadLine();
    }
}
