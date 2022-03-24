using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCL_Project
{
    public static class GenerateAsset
    {
        interface IVisitor
        {

        }
        public static void DefineVisitor(TextWriter writer, string basename, List<string> types)
        {
            writer.Write(" interface Visitor<R> {");

            foreach (string type in types)
            {
                string typeName = type.Split(':')[0].Trim();
                writer.WriteLine($"    R visit{typeName}{basename}({typeName} {basename.ToLower()});");
            }
            writer.WriteLine("  }");
        }
    }
}
