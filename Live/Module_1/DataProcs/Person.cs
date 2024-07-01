using System.Xml.Serialization;

namespace DataProcs;

[XmlRoot("person")]
public class Person
{
    [XmlElement("first-name")]
    public string? FirstName { get; set; }
    [XmlElement("last-name")]
    public string? LastName { get; set; }
    [XmlAttribute("age")]
    public int Age { get; set; }
}
