using System.Security.Cryptography;
using System.Text;

namespace Onleesbaar;

internal class Program
{
    static void Main(string[] args)
    {
        //Console.WriteLine("Hello, World!");
        //SymmetrischeTest();
        ASymmetrischeTest();
    }

    private static void ASymmetrischeTest()
    {
        // Ontvanger
        RSA rsaInit = RSA.Create();
        string pubKey = rsaInit.ToXmlString(false);
        string privKey = rsaInit.ToXmlString(true);

        // Zender
        RSA rsaZender = RSA.Create();
        rsaZender.FromXmlString(pubKey);
        byte[] data = Encoding.UTF8.GetBytes("Hello World");
        byte[] cipher = rsaZender.Encrypt(data, RSAEncryptionPadding.Pkcs1);


        // Ontvanger
        RSA rsaOntv = RSA.Create();
        rsaOntv.FromXmlString(privKey);
        byte[] data2 = rsaOntv.Decrypt(cipher, RSAEncryptionPadding.Pkcs1);
        Console.WriteLine(Encoding.UTF8.GetString(data2));

    }

    static void SymmetrischeTest()
    {
        // Zender
        string bericht = "Hello World";
        Aes alg = Aes.Create();
        alg.Mode = CipherMode.CBC;
        byte[] key = alg.Key;
        byte[] iv = alg.IV;
        using MemoryStream mem = new MemoryStream();
        using CryptoStream crypt = new CryptoStream(mem, alg.CreateEncryptor(), CryptoStreamMode.Write);
        using (StreamWriter sw = new StreamWriter(crypt))
        {
            sw.WriteLine(bericht);
        }
        byte[] data = mem.ToArray();
        Console.WriteLine(Encoding.UTF8.GetString(data));


        // Ontvanger
        Aes alg2 = Aes.Create();    
        alg2.Mode = CipherMode.CBC;
        alg2.Key = key;
        alg2.IV = iv;
        using MemoryStream mem2 = new MemoryStream(data);
        using CryptoStream crypt2 = new CryptoStream(mem2, alg2.CreateDecryptor(), CryptoStreamMode.Read);
        using (StreamReader sr = new StreamReader(crypt2))
        {
            string res = sr.ReadToEnd();
            Console.WriteLine(res);
        }


    }
}
