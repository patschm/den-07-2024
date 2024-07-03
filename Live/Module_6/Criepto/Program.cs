
using System.Security.Cryptography;
using System.Text;

namespace Criepto;

internal class Program
{
    static void Main(string[] args)
    {
        //TestHash();
        //TestKeyedHash();
        TestAsymHash();
    }

    private static void TestHash()
    {
        // Zender
        string bericht = "Hello World";
        SHA512 alg = SHA512.Create();   
        byte[] hash = alg.ComputeHash(Encoding.UTF8.GetBytes(bericht));
        Console.WriteLine(Convert.ToBase64String(hash));

        //bericht += ".";


        // Ontvanger
        SHA512 alg2 = SHA512.Create();
        byte[] hash2 = alg2.ComputeHash(Encoding.UTF8.GetBytes(bericht));
        Console.WriteLine(Convert.ToBase64String(hash2));
    }

    private static void TestKeyedHash()
    {
        // Zender
        string bericht = "Hello World";
        HMACSHA512 alg = new HMACSHA512();
        alg.Key = Encoding.UTF8.GetBytes("Geheim123");
        byte[] hash = alg.ComputeHash(Encoding.UTF8.GetBytes(bericht));
        Console.WriteLine(Convert.ToBase64String(hash));

        //bericht += ".";


        // Ontvanger
        HMACSHA512 alg2 = new HMACSHA512();
        alg2.Key = Encoding.UTF8.GetBytes("Geheim123");
        byte[] hash2 = alg2.ComputeHash(Encoding.UTF8.GetBytes(bericht));
        Console.WriteLine(Convert.ToBase64String(hash2));
    }

    private static void TestAsymHash()
    {
        // Zender
        string bericht = "Hello World";
        SHA512 alg = SHA512.Create();
        byte[] hash = alg.ComputeHash(Encoding.UTF8.GetBytes(bericht));
        DSA dsa = DSA.Create();
        string pubKey = dsa.ToXmlString(false);
        byte[] signature = dsa.SignData(hash, HashAlgorithmName.SHA512);

        //Console.WriteLine(Convert.ToBase64String(hash));

        //bericht += ".";


        // Ontvanger
        SHA512 alg2 = SHA512.Create();
        byte[] hash2 = alg2.ComputeHash(Encoding.UTF8.GetBytes(bericht));
        DSA dsa2 = DSA.Create();
        dsa2.FromXmlString(pubKey);
        bool isOk = dsa2.VerifyData(hash2, signature, HashAlgorithmName.SHA512);
        Console.WriteLine(isOk ? "Ok": "NOk");
        //Console.WriteLine(Convert.ToBase64String(hash2));
    }
}
