using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTimer
{
    interface IOutput
    {
        ConsoleColor ForegroundColor { get; set; }

        void Write(string str);
        void WriteLine();
        void WriteLine(string str);
    }
}
