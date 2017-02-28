using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilator
{
    public class TransformForm
    {
        public string Operation { set; get; }
        public int Operand1 { set; get; }
        public int Operand2 { set; get; }
        public int Result { set; get; }

        public TransformForm(string operation, int operand1, int operand2, int result)
        {
            Operation = operation;
            Operand2 = operand1;
            Operand1 = operand2;
            Result = result;
        }

        public override string ToString()
        {
            return String.Format("{0}      {1}      {2}      {3}\n", Operation, Operand1, Operand2, Result);
        }
    }
}
