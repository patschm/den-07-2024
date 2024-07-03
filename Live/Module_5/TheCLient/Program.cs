//using SomeLibrary;

using System.Reflection;

namespace TheCLient;

internal class Program
{
    static void Main(string[] args)
    {
        //Person p1 = new Person { FirstName = "Jan", LastName = "Peters", Age = 42 };
        //p1.Introduce();
        Assembly assembly = Assembly.LoadFile("E:\\DistOut\\SomeLibrary.dll");
        Console.WriteLine(assembly.FullName);
        //Examine(assembly);
        // DoeErIetsMee(assembly);

        var rndclass = new RandomClass();
        DoeIets(rndclass);
    }

    private static void DoeIets(RandomClass rndclass)
    {
        var res = rndclass.GetType().GetCustomAttributes();
        var attr = rndclass.GetType().GetCustomAttribute<MyAttribute>(false);
        Console.WriteLine(attr.Age);
    }

    private static void DoeErIetsMee(Assembly assembly)
    {
        Type? t = assembly.GetType("SomeLibrary.Person");
        object? p1 =Activator.CreateInstance(t);

        Console.WriteLine(p1);

        var pFirst = t?.GetProperty("FirstName");
        pFirst?.SetValue(p1, "Jan");

        var pLast = t?.GetProperty("LastName");
        pLast?.SetValue(p1, "Peters");

        var pAge = t?.GetProperty("Age");
        pAge?.SetValue(p1, 42);

        var pField = t?.GetField("_age", BindingFlags.Instance | BindingFlags.NonPublic);
        pField?.SetValue(p1, -42);

        var mIntro = t?.GetMethod("Introduce");
        mIntro?.Invoke(p1, []);

        dynamic? p2 = Activator.CreateInstance(t);
        p2.FirstName = "Kees";
        p2.LastName = "de Vries";
        p2.Age = 34;

        p2.Introduce();

    }

    private static void Examine(Assembly assembly)
    {
       foreach(Type t in assembly.GetTypes())
        {
            Console.WriteLine(t.FullName);
            //Console.WriteLine(t);
            Console.WriteLine("{");
            foreach (var field in t.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                Console.WriteLine("\t" + (field.IsPrivate ? "private" : "public") + $" {field.FieldType.Name} {field.Name};");
            }
            Console.WriteLine(  );
            foreach(var prop in t.GetProperties())
            {
                Console.WriteLine("\tpublic " + $"{prop.PropertyType.Name} {prop.Name}");
            }
            Console.WriteLine();
            foreach(var mi in t.GetMethods())
            {
                Console.WriteLine($"\t{mi.Name}()");
            }
            Console.WriteLine();
            foreach (var mi in t.GetConstructors())
            {
                Console.WriteLine($"\t{mi.Name}()");
            }
            Console.WriteLine("}");
        }
    }
}
