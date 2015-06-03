using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTimer
{
    class ConsoleOutput : IOutput
    {
        public ConsoleColor ForegroundColor
        {
            get
            {
                return Console.ForegroundColor;
            }
            set
            {
                Console.ForegroundColor = value;
            }
        }

        public void Write(string str)
        {
            Console.Write(str);
        }

        public void WriteLine()
        {
            Console.WriteLine();
        }

        public void WriteLine(string str)
        {
            Console.WriteLine(str);
        }
    }
}
