using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    public class SemanticAnalysis
    {
        private string systemVariableName;
        public List<TransformForm> MF;
        private int N = 0;
        private int TetN = 0;
        private int R;
        private SortedSet<int> BoolOprations;
        private SortedSet<int> AritmeticOprationLow;
        private SortedSet<int> AritmeticOprationHight;
        private CompilerClass compiler;
        private SymbolTableItem buffer;
        private int PointToJump = -1;
        private int i;
        private string label = "_S";

        public SemanticAnalysis(CompilerClass Lex)
        {
            BoolOprations = new SortedSet<int>();
            AritmeticOprationLow = new SortedSet<int>();
            AritmeticOprationHight = new SortedSet<int>();
            SetSets();
            MF = new List<TransformForm>();
            compiler = Lex;
            Index = compiler.IndexInSymbolTable;
            TableWithIdexOfOperators = new List<int>();
            MainTable = Lex.MainTable;
            ObjectiveCode = new List<string>();
        }

        public List<int> Errors { get; set; }

        public List<string> ObjectiveCode { get; set; }

        public List<SymbolTableItem> MainTable { get; set; }

        public List<int> Index { get; set; }

        public List<int> TableWithIdexOfOperators { get; set; }

        public List<SymbolTableItem> Variable { get; set; }

        public int GetMaxAddress()
        {
            int maxAdress = 0;
            for (int i = 0; i < MainTable.Count; i++)
            {
                if (maxAdress < MainTable[i].Addrres)
                {
                    maxAdress = MainTable[i].Addrres;
                }
            }
            return maxAdress;
        }

        public void GenerateMemory()
        {
            bool first = false;
            string line = "";

            foreach (var item in MainTable)
            {
                if (item.Code == 24)
                {
                    if (first)
                    {
                        Variable.Add(item);
                        line = item.Name.ToUpper() + "    DW  ";// + item.Addrres.ToString();
                        ObjectiveCode.Add(line);
                    }
                    first = true;
                }
            }


            ObjectiveCode.Add("TABLE	DB	'0123456789ABCDEF'");
            ObjectiveCode.Add("BUFER	DB  DUP(0)");
            ObjectiveCode.Add("BUF1	DB  DUP(0)");
        }

        public bool ConstOrVariableCheck(SymbolTableItem sb)
        {
            bool flag = false;
            if (sb.Code == 23 || sb.Code == 24)
            {
                flag = true;
            }
            return flag;
        }

        public void GenerateOperation(TransformForm operation, int i)
        {

            string line;

            switch (operation.Operation)
            {
                case "*":
                    {
                        line = "    MOV    AX," + MainTable[operation.Operand1].Name.ToUpper();
                        ObjectiveCode.Add(line);
                        line = "    MUL    AX," + MainTable[operation.Operand2].Name.ToUpper();
                        ObjectiveCode.Add(line);
                        line = "    MOV  " + MainTable[operation.Result].Name.ToUpper() + ",AX";
                        ObjectiveCode.Add(line);
                    }
                    break;
                case "/":
                    {
                        line = "    MOV    AX," + MainTable[operation.Operand1].Name.ToUpper();
                        ObjectiveCode.Add(line);
                        line = "    DIV    AX," + MainTable[operation.Operand2].Name.ToUpper();
                        ObjectiveCode.Add(line);
                        line = "    MOV  " + MainTable[operation.Result].Name.ToUpper() + ",AX";
                        ObjectiveCode.Add(line);

                    }
                    break;
                case "+":
                    {
                        line = "    MOV    AX," + MainTable[operation.Operand1].Name.ToUpper();
                        ObjectiveCode.Add(line);
                        line = "    ADD    AX," + MainTable[operation.Operand2].Name.ToUpper();
                        ObjectiveCode.Add(line);
                        line = "    MOV  " + MainTable[operation.Result].Name.ToUpper() + ",AX";
                        ObjectiveCode.Add(line);
                    }
                    break;
                case "-":
                    {
                        line = "    MOV    AX," + MainTable[operation.Operand1].Name.ToUpper();
                        ObjectiveCode.Add(line);
                        line = "    SUB    AX," + MainTable[operation.Operand2].Name.ToUpper();
                        ObjectiveCode.Add(line);
                        line = "    MOV  " + MainTable[operation.Result].Name.ToUpper() + ",AX";
                        ObjectiveCode.Add(line);
                    }
                    break;
                case ">":
                    {
                        line = label + i.ToString() + "    MOV    AX," + MainTable[operation.Operand1].Name.ToUpper();
                        ObjectiveCode.Add(line);
                        line = "    MOV    BX," + MainTable[operation.Operand2].Name.ToUpper();
                        ObjectiveCode.Add(line);
                        line = "    CMP    AX,BX";
                        ObjectiveCode.Add(line);
                        line = "    JG " + label + operation.Operand1;
                        ObjectiveCode.Add(line);
                    }
                    break;
                case "<":
                    {
                        line = label + i.ToString() + "    MOV    AX," + MainTable[operation.Operand1].Name.ToUpper();
                        ObjectiveCode.Add(line);
                        line = "    MOV    BX," + MainTable[operation.Operand2].Name.ToUpper();
                        ObjectiveCode.Add(line);
                        line = "    CMP    AX,BX";
                        ObjectiveCode.Add(line);
                        line = "    JL  " + label + operation.Operand1;
                        ObjectiveCode.Add(line);
                    }
                    break;
                case ">=":
                    {
                        line = label + i.ToString() + "    MOV    AX," + MainTable[operation.Operand1].Name.ToUpper();
                        ObjectiveCode.Add(line);
                        line = "    MOV    BX," + MainTable[operation.Operand2].Name.ToUpper();
                        ObjectiveCode.Add(line);
                        line = "    CMP    AX,BX";
                        ObjectiveCode.Add(line);
                        line = "    JGE  " + label + operation.Operand1;
                        ObjectiveCode.Add(line);
                    }
                    break;
                case "<=":
                    {
                        line = label + i.ToString() + "    MOV    AX," + MainTable[operation.Operand1].Name.ToUpper();
                        ObjectiveCode.Add(line);
                        line = "    MOV    BX," + MainTable[operation.Operand2].Name.ToUpper();
                        ObjectiveCode.Add(line);
                        line = "    CMP    AX,BX";
                        ObjectiveCode.Add(line);
                        line = "    JLE   " + label + operation.Operand1;
                        ObjectiveCode.Add(line);
                    }
                    break;
                case "==":
                    {
                        line = label + i.ToString() + "    MOV    AX," + MainTable[operation.Operand1].Name.ToUpper();
                        ObjectiveCode.Add(line);
                        line = "    MOV    BX," + MainTable[operation.Operand2].Name.ToUpper();
                        ObjectiveCode.Add(line);
                        line = "    CMP    AX,BX";
                        ObjectiveCode.Add(line);
                        line = "    JE   " + label + operation.Operand1;
                        ObjectiveCode.Add(line);
                    }
                    break;
                case "=":
                    {
                        line = "    MOV AX," + MainTable[Index[operation.Operand1]].Name.ToUpper();
                        ObjectiveCode.Add(line);
                        line = "    MOV  " + MainTable[operation.Operand2].Name.ToUpper() + ",AX";
                        ObjectiveCode.Add(line);

                    }
                    break;
                case "IN":
                    {
                        ObjectiveCode.Add("    LEA	SI,BUFER");
                        ObjectiveCode.Add("    LEA DI," + MainTable[operation.Operand1].Name.ToUpper());
                        ObjectiveCode.Add("    CALL IN_CH");

                    }
                    break;
                case "OUT":
                    {
                        ObjectiveCode.Add("    MOV AX," + MainTable[operation.Operand1].Name.ToUpper());
                        ObjectiveCode.Add("    MOV  BUF1,AX");
                        ObjectiveCode.Add("    CALL PRINTB");

                    }
                    break;
                case "JMP":
                    {
                        line = "    JMP " + label + operation.Operand1;
                        ObjectiveCode.Add(line);
                        line = label + (i).ToString();
                        ObjectiveCode.Add(line);
                    }
                    break;

            }

        }

        public void GenerateObejctiveCode()
        {
            for (int i = 0; i < MF.Count; i++)
            {
                GenerateOperation(MF[i], i);
            }

            ObjectiveCode.Add("    MOV	AH,0");
            ObjectiveCode.Add("    INT	16H");
            ObjectiveCode.Add("    INT	20H");

            GenerateMemory();



            string[] temp = File.ReadAllLines("in.asm");

            for (int i = 0; i < temp.Length; i++)
            {
                ObjectiveCode.Add(temp[i]);
            }
            temp = File.ReadAllLines("out.asm");

            for (int i = 0; i < temp.Length; i++)
            {
                ObjectiveCode.Add(temp[i]);
            }
        }

        public void SetSets()
        {
            for (int i = 10; i < 15; i++)
            {
                BoolOprations.Add(i);
            }
            for (int i = 15; i < 17; i++)
            {
                AritmeticOprationLow.Add(i);
            }
            for (int i = 17; i < 19; i++)
            {
                AritmeticOprationHight.Add(i);
            }
        }

        private void Next()
        {
            try
            {
                buffer = MainTable[Index[i]];
                i++;
            }
            catch (ArgumentOutOfRangeException)
            {

            }

        }

        private void Block()
        {
            /*********************************/
            string Prom = "";


            Operation(ref Prom);

            while (buffer.Name != "}")
            {

                Prom = buffer.Name;

                Operation(ref Prom);
                if (i > Index.Count - 1)
                {
                    break;
                }
                //next_();
            }
        }

        private void GenerateJump()
        {
            var temp = MF[MF.Count - 1];
            string comparisonSymbol = temp.Operation;
            switch (comparisonSymbol)
            {
                case "==":
                    {
                        PointToJump = MF.Count;
                        MF.Add(new TransformForm("JNE", -1, temp.Result, -1));

                    }
                    break;
                case ">":
                    {
                        PointToJump = MF.Count;
                        MF.Add(new TransformForm("JNG", -1, temp.Result, -1));
                    }
                    break;
                case "<":
                    {
                        PointToJump = MF.Count;
                        MF.Add(new TransformForm("JNL", -1, temp.Result, -1));
                    }
                    break;
                case ">=":
                    {
                        PointToJump = MF.Count;
                        MF.Add(new TransformForm("JNGE", -1, temp.Result, -1));
                    }
                    break;
                case "<=":
                    {
                        PointToJump = MF.Count;
                        MF.Add(new TransformForm("JNLE", -1, temp.Result, -1));
                    }
                    break;
            }
        }

        private void SetJump()
        {
            MF[PointToJump].Operand1 = MF.Count + 1;
        }

        private void SetJumpForElse()
        {
            MF[PointToJump].Operand1 = MF.Count;
        }

        private void GenerateJumpForIf()
        {
            PointToJump = MF.Count;
            MF.Add(new TransformForm("JMP", -1, -1, -1));
        }

        private void GetJumpForWhile(int jumpUp)
        {
            //PointToJump = MF.Count + 1;
            MF[MF.Count - 1].Operand1 = jumpUp;
        }

        private void Operation(ref string Op1)
        {
            string Op2 = "";
            string op;
            if (buffer.Code == 24)//Promenliva
            {
                Op1 = buffer.Name;
                Next();
                if (buffer.Code != 9) //=
                {
                    Errors.Add(5);
                    return;
                }
                op = buffer.Name;
                Next();

                Expression(ref Op2);
                // R = SysVar();
                AddTransformForm(op, GetAddress(Op1), GetAddress(Op2), -1);
                if (buffer.Code != 21) //;
                {
                    Errors.Add(6);
                    return;
                }
                Next();
            }

            else
            {
                if (buffer.Code == 7)//if
                {
                    Next();
                    if (buffer.Code != 19)//(
                    {
                        Errors.Add(3);
                        return;
                    }

                    Next();
                    Op1 = buffer.Name;

                    BinaryExpression(ref Op1);
                    GenerateJump();
                    if (buffer.Code != 20)//)
                    {
                        Errors.Add(4);
                        return;
                    }

                    Next();
                    if (buffer.Code != 1)//{
                    {
                        Errors.Add(1);
                        return;
                    }

                    Next();
                    Block();
                    SetJump();
                    if (buffer.Code != 2)//}
                    {
                        Errors.Add(2);
                        return;
                    }

                    Next();

                    if (buffer.Code == 26)//else
                    {
                        GenerateJumpForIf();
                        Next();
                        if (buffer.Code != 1)//{
                        {
                            Errors.Add(1);
                            return;
                        }

                        Next();
                        Block();
                        if (buffer.Code != 2)//}
                        {
                            Errors.Add(2);
                            return;
                        }
                        SetJumpForElse();
                        Next();

                    }
                }
                else
                {
                    if (buffer.Code == 8)//while
                    {
                        int jumpUp = -1;

                        Next();
                        if (buffer.Code != 19)//(
                        {
                            Errors.Add(3);
                            return;
                        }

                        Next();
                        Op1 = buffer.Name;
                        jumpUp = MF.Count;
                        BinaryExpression(ref Op1);
                        GenerateJump();

                        if (buffer.Code != 20)//)
                        {
                            Errors.Add(4);
                            return;
                        }

                        Next();
                        if (buffer.Code != 1)//{
                        {
                            Errors.Add(1);
                            return;
                        }


                        Next();
                        Block();
                        SetJump();
                        GenerateJumpForIf();

                        GetJumpForWhile(jumpUp);

                        if (buffer.Code != 2)//}
                        {
                            Errors.Add(2);
                            return;
                        }

                        Next();
                    }
                    else
                    {
                        if (buffer.Code == 5)//out
                        {

                            Next();
                            if (!ConstOrVariableCheck(buffer)) //promenliva ili konstanta
                            {
                                Errors.Add(9);
                                return;
                            }
                            MF.Add(new TransformForm("OUT", GetAddress(buffer.Name), -1, -1));
                            Next();
                            if (buffer.Code != 21)//;
                            {
                                Errors.Add(6);
                                return;
                            }
                            Next();
                        }
                        else
                        {
                            if (buffer.Code == 6)//in
                            {
                                Next();
                                if (buffer.Code != 24)//promenliva ili konstanta
                                {
                                    Errors.Add(9);
                                    return;
                                }
                                MF.Add(new TransformForm("IN", GetAddress(buffer.Name), -1, -1));
                                Next();
                                if (buffer.Code != 21)//;
                                {
                                    Errors.Add(6);
                                    return;
                                }
                                Next();
                            }
                            else
                            {
                                if (buffer.Name == "int" || buffer.Name == "double")
                                {
                                    Next();
                                    if (buffer.Code != 24)
                                    {
                                        Errors.Add(13);
                                        return;
                                    }

                                    Next();
                                    if (buffer.Name != ";")
                                    {
                                        Errors.Add(6);
                                        return;
                                    }
                                    Next();

                                }
                                else
                                {

                                    Errors.Add(10);
                                    Next();
                                    return;

                                }


                            }
                        }
                    }
                }
            }
        }

        private void BinaryExpression(ref string Op1)
        {
            string Op2 = "";
            string operatio;
            Expression(ref Op1);
            if (!BoolOprations.Contains(buffer.Code))
            {
                Errors.Add(11);
                return;
            }
            operatio = buffer.Name;
            Next();
            Expression(ref Op2);
            R = SysVar();
            MainTable[R].Addrres = GetMaxAddress() + 10;
            AddTransformForm(operatio, GetAddress(Op1), GetAddress(Op2), R);

        }

        private void Term(ref string Op1)
        {
            string Op2 = "";
            string op;
            Factor(ref Op1);
            while (AritmeticOprationHight.Contains(buffer.Code))
            {
                op = buffer.Name;
                Next();

                Factor(ref Op2);
                R = SysVar();
                MainTable[R].Addrres = GetMaxAddress() + 10;
                AddTransformForm(op, GetAddress(Op1), GetAddress(Op2), R);
                Op1 = MainTable[R].Name;
            }
        }

        private void Factor(ref string Op1)
        {
            if (buffer.Code == 19) //(
            {
                Next();
                Expression(ref Op1);
                if (buffer.Code != 20) //)
                {
                    Errors.Add(4);
                    return;
                }
                Next();
            }
            else
            {
                if (buffer.Code == 24)
                {
                    Op1 = buffer.Name;
                    Next();

                }
                else
                {
                    if (buffer.Code == 23)
                    {
                        Op1 = buffer.Name;
                        Next();
                    }
                    else
                    {
                        Errors.Add(12);
                        return;
                    }
                }
            }
        }

        private int SysVar()
        {
            systemVariableName = "_a" + N.ToString();
            N++;
            return compiler.FindSymbolInSymbolTable(systemVariableName, 24);
        }

        private void AddTransformForm(string Op, int O1, int O2, int Rez)
        {
            MF.Add(new TransformForm(Op, O1, O2, Rez));
            TetN++;
        }

        private void SemanticError(int NumbError)
        {
            Errors.Add(NumbError);
        }

        private void Expression(ref string Op1)
        {
            string Op2 = "";
            string op;
            Term(ref Op1);
            while (AritmeticOprationLow.Contains(buffer.Code))
            {
                op = buffer.Name;
                Next();
                Term(ref Op2);
                R = SysVar();
                MainTable[R].Addrres = GetMaxAddress() + 10;
                AddTransformForm(op, GetAddress(Op1), GetAddress(Op2), R);
                Op1 = compiler.MainTable[R].Name;
            }

        }

        private int GetAddress(string Op)
        {
            return compiler.SortedTable[Op];
        }

    }
}
