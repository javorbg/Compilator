using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Compiler.Classes
{
    public class Syntaxis
    {
        public Dictionary<int, Tuple<string, Color?>> KeyWords { get; set; }
        public Syntaxis()
        {
            KeyWords = new Dictionary<int, Tuple<string, Color?>>();
            KeyWords.Add(1, new Tuple<string, Color?>("{", null));
            KeyWords.Add(2, new Tuple<string, Color?>("}", null));
            KeyWords.Add(3, new Tuple<string, Color?>("int", Colors.Blue));
            KeyWords.Add(4, new Tuple<string, Color?>("double", Colors.Blue));
            KeyWords.Add(5, new Tuple<string, Color?>("out", Colors.Blue));
            KeyWords.Add(6, new Tuple<string, Color?>("in", Colors.Blue));
            KeyWords.Add(7, new Tuple<string, Color?>("if", Colors.Blue));
            KeyWords.Add(8, new Tuple<string, Color?>("while", Colors.Blue));
            KeyWords.Add(9, new Tuple<string, Color?>("=", null));
            KeyWords.Add(10, new Tuple<string, Color?>("==", null));
            KeyWords.Add(11, new Tuple<string, Color?>(">", null));
            KeyWords.Add(12, new Tuple<string, Color?>("<", null));
            KeyWords.Add(13, new Tuple<string, Color?>("<=", null));
            KeyWords.Add(14, new Tuple<string, Color?>(">=", null));
            KeyWords.Add(15, new Tuple<string, Color?>("+", null));
            KeyWords.Add(16, new Tuple<string, Color?>("-", null));
            KeyWords.Add(17, new Tuple<string, Color?>("*", null));
            KeyWords.Add(18, new Tuple<string, Color?>("/", null));
            KeyWords.Add(19, new Tuple<string, Color?>("(", null));
            KeyWords.Add(20, new Tuple<string, Color?>(")", null));
            KeyWords.Add(21, new Tuple<string, Color?>(";", null));
            KeyWords.Add(22, new Tuple<string, Color?>(",", null));
            KeyWords.Add(23, new Tuple<string, Color?>("program", Colors.Blue));
            KeyWords.Add(24, new Tuple<string, Color?>("else", Colors.Blue));
        }
    }
}
