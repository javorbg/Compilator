using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilator
{
    public class SymbolTableItem
    {
        public string Name { get; set; }
        public int Code { get; set; }

        public int Value { get; set; }
        public int Addrres { get; set; }


        public SymbolTableItem(string name, int code)
        {
            Name = name;
            Code = code;
        }

        public override string ToString()
        {
            return String.Format("{0}  {1}  {2}  {3}\n", Name, Code, Value, Addrres);
        }
    }
}
