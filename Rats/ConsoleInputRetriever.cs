using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rats
{
    public class ConsoleInputRetriever : IConsoleInputRepository
    {
        public string GetNextConsoleInput()
        {
            return Console.ReadLine();
        }
    }
}
