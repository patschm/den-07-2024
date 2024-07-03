namespace SomeLibrary;

[Obsolete]
public class Person
{
	private int _age;

	public int Age
	{
		get { return _age; }
		set 
		{ 
			if (value >= 0)
				_age = value; 
		}
	}
	public string? FirstName { get; set; }
    public string? LastName { get; set; }

	public void Introduce()
	{
        Console.WriteLine($"{FirstName} {LastName} ({Age})");
    }
}
