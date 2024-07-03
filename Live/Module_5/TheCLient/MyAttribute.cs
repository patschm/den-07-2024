using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheCLient;

[AttributeUsage(AttributeTargets.Class)]
public class MyAttribute : Attribute
{
    public int Age { get; set; }
}
