using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeTimer;

namespace CodeTimer.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            CodeTimer.Evaluate("A", () => { Console.WriteLine("a"); }, 1, true);

            Console.ReadKey(true);
        }
    }
}
