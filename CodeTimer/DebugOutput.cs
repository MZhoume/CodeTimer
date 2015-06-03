using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTimer
{
    class DebugOutput : IOutput
    {
        public ConsoleColor ForegroundColor { get; set; }

        public void Write(string str)
        {
            Debug.Write(str);
        }

        public void WriteLine()
        {
            Debug.WriteLine("");
        }

        public void WriteLine(string str)
        {
            Debug.WriteLine(str);
        }
    }
}
