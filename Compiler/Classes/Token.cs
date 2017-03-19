using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Classes
{
    public class Token
    {
        public int Line { get; set; }

        public int Begin { get; set; }

        public int Length { get; set; }

        public string Symbol { get; set; }

        public bool IsKeyWord { get; set; }

        public bool IsSymbol { get; set; }        
    }
}
