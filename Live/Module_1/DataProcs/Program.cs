
using System.Xml;
using System.Xml.Serialization;

namespace DataProcs;

internal class Program
{
    static Person[] people = [
        new Person { FirstName = "AAAA", LastName = "ZZZZ", Age = 23},
        new Person { FirstName = "BBBB", LastName = "YYYY", Age = 14},
        new Person { FirstName = "CCCC", LastName = "XXXX", Age = 67},
        new Person { FirstName = "DDDD", LastName = "WWWW", Age = 45}
    ];
    static void Main(string[] args)
    {
        // NaarXml();
        //NaarXmlModern();
        VanXmlModern();
    }

    private static void VanXmlModern()
    {
        XmlSerializer ser = new XmlSerializer(typeof(Person));
        FileStream fs = File.OpenRead("persons.xml");
        XmlReader reader = XmlReader.Create(fs);
        while(reader.ReadToFollowing("person"))
        {
            Person? p = ser.Deserialize(reader.ReadSubtree()) as Person;
            Console.WriteLine($"{p.FirstName} {p.LastName}");
        }
    }

    private static void NaarXmlModern()
    {
        XmlSerializer ser = new XmlSerializer(typeof(Person));
        FileStream fs = File.OpenWrite("persons.xml");
        XmlWriter writer = XmlWriter.Create(fs);
        writer.WriteStartElement("people");
        foreach(Person person in people) 
        {
            ser.Serialize(writer, person);
        }
        writer.WriteEndElement();
        writer.Flush();
        fs.Close();
    }

    private static void NaarXml()
    {
        FileStream fs = File.OpenWrite("people.xml");
        XmlWriter writer = XmlWriter.Create(fs);
        writer.WriteProcessingInstruction("xml", "utf-8");

        writer.WriteStartElement("people");
        foreach (Person p in people)
        {
            writer.WriteStartElement("person");
            writer.WriteStartElement("first-name");
            writer.WriteString(p.FirstName);
            writer.WriteEndElement();
            writer.WriteStartElement("last-name");
            writer.WriteString(p.LastName);
            writer.WriteEndElement();
            writer.WriteStartElement("age");
            writer.WriteString(p.Age.ToString());
            writer.WriteEndElement();
           writer.WriteEndElement();
        }
        writer.WriteEndElement();
        writer.Flush();
        fs.Close();
    }
   
}
