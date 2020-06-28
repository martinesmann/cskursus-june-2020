using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System.Threading.Tasks;

namespace ReflectionDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {

            var script = string.Empty
                .AddLine("System.Console.WriteLine(42)")
                .AddLine("var calc = new MyCalc()")
                .AddLine("var res = calc.Add(2, 3)")
                .AddLine("Console.WriteLine(res)");

            var options = ScriptOptions.Default
                .AddReferences(typeof(MyCalc).Assembly)
                .AddImports("ReflectionDemo", "System");

            await CSharpScript.EvaluateAsync(script, options);

        }
    }

    public class MyCalc
    {
        public int Add(int c, int y)
        {
            return c + y;
        }
    }

    public static class Extensions
    {
        public static string AddLine(this string str, string line)
        {
            return str + (line.EndsWith(";") ? line : line + ";");
        }
    }
}
