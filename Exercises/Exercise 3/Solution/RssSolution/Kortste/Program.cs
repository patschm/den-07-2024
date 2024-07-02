using Feeds;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Kortste;

internal class Program
{
    static void Main(string[] args)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Item));
        var reader = XmlReader.Create("https://nu.nl/rss");
        while (reader.ReadToFollowing("item"))
        {
            var item = serializer.Deserialize(reader.ReadSubtree()) as Item;
            if (item != null) ShowItem(item);
        }
    }
    static void ShowItem(Item item)
    {
        Console.BackgroundColor = ConsoleColor.Yellow;
        Console.WriteLine(item.Category);
        Console.ResetColor();
        Console.BackgroundColor = ConsoleColor.Red;
        Console.WriteLine(item.Title);
        Console.ResetColor();
        Console.WriteLine(item.Description);
        Console.WriteLine();
    }
}
