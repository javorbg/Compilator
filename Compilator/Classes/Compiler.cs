using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilator
{
    public class Compiler
    {
        private string Symbols = "{ } ( ) * / + - ; , == = <= >= < >";
        private char[] spaces = { ' ', '\n', '\r', '\t' };
        private StringBuilder buffer;
        private List<string> lexicalAnalyze;

        // 24 variable
        // 23 const

        public Compiler()
        {
            MainTable = new List<SymbolTableItem>();
            SortedTable = new SortedList<string, int>();
            buffer = new StringBuilder();
            IndexInSymbolTable = new List<int>();
            lexicalAnalyze = new List<string>();
            Codes = new SortedList<string, int>();
        }

        public List<SymbolTableItem> MainTable { get; set; }
        public SortedList<string, int> SortedTable { get; set; }

        public List<int> IndexInSymbolTable { get; set; }

        public SortedList<string, int> Codes { get; set; }


        public bool CharCheck(char ch)
        {
            if ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z'))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DigitCheck(char ch)
        {
            if (ch >= '0' && ch <= '9')
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool VariableCheck(string checkedElement)
        {
            bool flagError = true;
            for (int i = 0; i < checkedElement.Length; i++)
            {
                if (CharCheck(checkedElement[i]) || DigitCheck(checkedElement[i]))
                {

                }
                else
                {
                    flagError = false;
                }
            }
            if (checkedElement.Contains('.'))
            {
                return false;
            }
            return flagError;
        }

        public bool NumberCheck(string checkedElement)
        {
            int brDot = 0;
            bool flagError = true;
            for (int i = 0; i < checkedElement.Length; i++)
            {
                if (DigitCheck(checkedElement[i]) || checkedElement[i] == '.')
                {
                    if (checkedElement[i] == '.')
                    {
                        brDot++;
                    }
                }
                else
                {
                    flagError = false;
                }
            }

            if (brDot > 1)
            {
                flagError = false;
            }
            return flagError;

        }

        public int VariableNumberCheck(string checkedElement)
        {
            if (checkedElement.Length > 0)
            {
                if (CharCheck(checkedElement[0]))
                {
                    if (!VariableCheck(checkedElement))
                    {
                        lexicalAnalyze.Clear();
                        return 3;
                    }
                }

                if (DigitCheck(checkedElement[0]))
                {
                    if (!NumberCheck(checkedElement))
                    {
                        lexicalAnalyze.Clear();
                        return 4;
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// leksi4en analiz
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public int LexicalAnalysis(string input)
        {
            if (Codes.Count == 0)
            {
                return 2;
            }
            int i = 0;

            for (i = 0; i < input.Length;)
            {
                if (!FindSpace(input[i]))
                {
                    if (CharCheck(input[i]))
                    {
                        buffer.Append(input[i]);
                        i++;
                    }
                    else
                    {
                        if ((input[i] >= '0' && input[i] <= '9') || input[i] == '.')
                        {
                            buffer.Append(input[i]);
                            i++;
                        }
                        else
                        {
                            string buffer1 = buffer.ToString();
                            int n = VariableNumberCheck(buffer1);
                            if (n > 0)
                            {
                                lexicalAnalyze.Clear();
                                return n;
                            }

                            if (2 == FindSymbol(input, i))
                            {
                                addToLs(ref buffer);
                                buffer.Append(input[i]);
                                buffer.Append(input[i + 1]);
                                addToLs(ref buffer);
                                i += 2;
                            }
                            else
                            {
                                if (1 == FindSymbol(input, i))
                                {
                                    addToLs(ref buffer);
                                    buffer.Append(input[i]);
                                    addToLs(ref buffer);
                                    i++;
                                }
                                else
                                {
                                    lexicalAnalyze.Clear();
                                    return 1;
                                }
                            }
                        }
                    }
                }
                else
                {
                    string buffer1 = buffer.ToString();
                    int n = VariableNumberCheck(buffer1);
                    if (n > 0)
                    {
                        lexicalAnalyze.Clear();
                        return n;
                    }
                    addToLs(ref buffer);
                    i++;
                }
            }
            addToLs(ref buffer);
            ListStringToIndexSymbol();
            SetCodesInSymbolTable();
            SetValueAndAddress();
            return 0;
        }

        public void SetValueAndAddress()
        {
            int adress = 100;

            foreach (var item in MainTable)
            {
                if (item.Code == 24)
                {
                    item.Addrres = adress;
                    adress += 10;
                    item.Value = 0;
                }
                if (item.Code == 23)
                {
                    item.Addrres = adress;
                    adress += 10;
                    item.Value = Convert.ToInt32(item.Name);

                }
            }
        }

        /// <summary>
        /// Find Symbol in Symbol table if exist return Symbol code or add in Symbol table
        /// </summary>
        /// <param name="SymbolName"></param>
        /// <param name="SymbolCode"></param>
        /// <returns></returns>
        public int FindSymbolInSymbolTable(string SymbolName, int SymbolCode)
        {
            if (MainTable.Count != 0)
            {
                if (!SortedTable.ContainsKey(SymbolName))
                {
                    return Add(SymbolName, SymbolCode);
                }
                else
                {
                    return SortedTable[SymbolName];
                }
            }
            else
            {
                return Add(SymbolName, SymbolCode);
            }
        }


        /// <summary>
        /// Sets Codes in Symbol table
        /// </summary>
        private void SetCodesInSymbolTable()
        {
            foreach (SymbolTableItem item in MainTable)
            {
                if (Codes.ContainsKey(item.Name))
                {
                    item.Code = Codes[item.Name];
                }
                else
                {
                    try
                    {
                        Convert.ToDouble(item.Name);
                        item.Code = 23;   //const
                    }
                    catch (FormatException)
                    {

                        item.Code = 24;  // variable
                    }
                }
            }
        }

        /// <summary>
        /// Convert list of Symbol to List of Symbol table indexes
        /// </summary>
        private void ListStringToIndexSymbol()
        {
            int symbolCode = 0;
            foreach (string symbol in lexicalAnalyze)
            {
                symbolCode = FindSymbolInSymbolTable(symbol, 0);
                IndexInSymbolTable.Add(symbolCode);
            }
        }
       
        private void AddToList(StringBuilder str)
        {
            if (buffer.Length != 0)
            {
                lexicalAnalyze.Add(str.ToString());
                str = new StringBuilder();
            }
        }

        private int Add(string NameC, int Code)
        {
            int i = MainTable.Count;
            MainTable.Add(new SymbolTableItem(NameC, Code));
            SortedTable.Add(NameC, i);
            return i;
        }

        private bool FindSpace(char ch)
        {
            bool find = false;
            for (int i = 0; i < spaces.Length; i++)
            {
                if (ch == spaces[i])
                {
                    find = true;
                }
            }
            return find;
        }

        private int FindSymbol(string ch, int j)
        {
            char[] separator = { ' ' };
            string[] split = Symbols.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < split.Length; i++)
            {
                try
                {
                    if (split[i].Length == 2)
                    {
                        if (ch[j] == split[i][0] && split[i][1] == ch[j + 1])
                        {
                            return 2;
                        }
                    }
                    else
                    {
                        if (split[i].Length == 1)
                        {
                            if (ch[j] == split[i][0])
                            {
                                return 1;
                            }
                        }
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    return 0;
                }
            }
            return 0;
        }
    }
}
