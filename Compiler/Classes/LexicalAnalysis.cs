using System.Collections.Generic;
using System.Text;

namespace Compiler.Classes
{
    public class LexicalAnalysis
    {
        private List<string> keyWord = new List<string>() { "int", "double", "out", "in", "if", "while", "program", "else" };

        private List<string> symbols = new List<string>() { "{", "}", "(", ")", "*", "/", "+", "-", ";", ",", "==", "=", "<=", ">=", "<", ">" };

        public List<Token> Tokens { get; set; } = new List<Token>();

        public void Analize(string[] sourceCode)
        {
            StringBuilder buffer = new StringBuilder();
            int begin = 0;
            string symbol = null;
            for (int i = 0; i < sourceCode.Length; i++)
            {
                for (int j = 0; j < sourceCode[i].Length; j++)
                {
                    if ((char.IsWhiteSpace(sourceCode[i][j]) || this.symbols.Contains(sourceCode[i][j].ToString())) && buffer.Length > 0)
                    {
                        symbol = buffer.ToString();
                        begin = sourceCode[i].IndexOf(symbol);
                        this.InsertToken(symbol, i, begin, false);
                        buffer.Clear();
                    }

                    buffer.Append(sourceCode[i][j]);

                    if (!char.IsWhiteSpace(sourceCode[i][j]) && this.symbols.Contains(sourceCode[i][j].ToString()))
                    {
                        if (j + 1 < sourceCode[i].Length && buffer.Length == 1 && buffer[0] == '=' && sourceCode[i][j + 1] == '=')
                        {
                            symbol = buffer.ToString();
                            buffer.Append(sourceCode[i][j + 1]);
                            begin = sourceCode[i].IndexOf(symbol);
                            this.InsertToken(symbol, i, begin, true);
                            buffer.Clear();
                            j++;
                            continue;
                        }
                        else
                        {
                            symbol = buffer.ToString();
                            begin = sourceCode[i].IndexOf(symbol);
                            this.InsertToken(symbol, i, begin, false);
                            buffer.Clear();
                        }
                    }
                }
            }
        }

        private void InsertToken(string symbol, int i, int j, bool isSymbol)
        {
            bool isKeyword = this.keyWord.Contains(symbol);

            Token token = new Token()
            {
                Line = i,
                Begin = j,
                Length = symbol.Length,
                Symbol = symbol,
                IsSymbol = isSymbol,
                IsKeyWord = isKeyword,
            };

            this.Tokens.Add(token);
        }
    }
}