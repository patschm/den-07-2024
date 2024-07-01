
using System.IO.Compression;
using System.Text;

namespace Stromingen;

internal class Program
{
    static void Main(string[] args)
    {
        //SimpelStreamen();
        //SimpleLezen();
        //ModernSchrijven();
        //ModernLezen();
        //ModernSchrijvenCompressed();
        ModernLezenCompressed();
      
    }
    private static void ModernLezenCompressed()
    {
        FileInfo file = new FileInfo("modern.zip");
        FileStream fs = file.OpenRead();
        GZipStream zip = new GZipStream(fs, CompressionMode.Decompress);
        StreamReader sr = new StreamReader(zip);
        string? line;
        while ((line = sr.ReadLine()) != null)
        {
            Console.WriteLine(line);
        }
    }
    private static void ModernSchrijvenCompressed()
    {
        string s = "Hello World";
        FileStream fs = File.Create("modern.zip");
        GZipStream zip = new GZipStream(fs, CompressionMode.Compress);

        StreamWriter writer = new StreamWriter(zip);
        for (int i = 0; i < 1000; i++)
        {
            writer.WriteLine($"{s} {i}");
        }
        writer.Flush();
        fs.Close();
    }
    private static void ModernLezen()
    {
        FileInfo file = new FileInfo("modern.txt");
        FileStream fs = file.OpenRead();
        StreamReader sr = new StreamReader(fs);
        string? line;
       while((line = sr.ReadLine()) != null)
        {
            Console.WriteLine(line);
        }
    }

    private static void ModernSchrijven()
    {
        string s = "Hello World";
        FileStream fs = File.Create("modern.txt");
        StreamWriter writer = new StreamWriter(fs);
        for (int i = 0; i < 1000; i++)
        {
            writer.WriteLine($"{s} {i}");
        }
        writer.Flush();
        fs.Close();
    }
    private static void SimpleLezen()
    {
        FileInfo file = new FileInfo("bla.txt");
        FileStream fs = file.OpenRead();
        byte[] buffer = new byte[4];
        int nrRead = 0;
        while ((nrRead = fs.Read(buffer)) > 0)
        {
            string s = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            Console.Write(s);
            Array.Clear(buffer, 0, buffer.Length);
        }

    }

    private static void SimpelStreamen()
    {
        string s = "Hello World";
        FileStream fs = File.Create("bla.txt");

        for (int i = 0; i < 1000; i++)
        {
            byte[] data = Encoding.UTF8.GetBytes($"{s} {i}\r\n");
            fs.Write(data);
        }
        fs.Close();
    }
}
