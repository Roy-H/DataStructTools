using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataStructureCollection.Greedy
{
    public class HorseGoChess
    {
        private const short Size = 8;
        Cell[,] table = new Cell[Size,Size];

        private void PrintTable()
        {
            for (int i = 0; i < Size; i++)
            {
                string line = string.Empty;
                for (int j = 0; j < Size; j++)
                {
                    line += table[i, j].Value + "_";
                }
                Console.WriteLine(line);
            }
            Console.WriteLine();
        }
        public void Start()
        {
            InitTable();
            if (GoNext(table[2, 2], 1))
            {
                PrintTable();
            }
            else
            {
                Console.WriteLine("There is no solutuon");
            }
        }

        private bool GoNext(Cell cell, int currentStep)
        {
            cell.Value = currentStep;
            //PrintTable();
            if (currentStep >= Size * Size)
                return true;

            var list = GetNextSteps(cell);
            list = list.OrderBy(i => { return GetNextSteps(i).Count; }).ToList();
            foreach (var i in list)
            {
                if (GoNext(i, currentStep+1))
                    return true;
                i.Value = 0;
            }
            return false;
        }

        private void InitTable()
        {
            if (table != null)
            {
                for (int i = 0; i < Size; i++)
                {
                    for (int j = 0; j < Size; j++)
                    {
                        table[i, j] = new Cell();
                        table[i, j].Value = 0;
                        table[i, j].Row = i;
                        table[i, j].Col = j;
                    }
                }
            }
        }

        public List<Cell> GetNextSteps(Cell cell)
        {
            List<Cell> list = new List<Cell>();
            int j = cell.Col;
            int i = cell.Row;

            //1
            if (i - 2 >= 0 && j + 1 < Size && table[i - 2, j + 1].Value == 0)
            {
                list.Add(table[i-2, j+1]);
            }
            //2
            if (i - 1 >= 0 && j + 2 < Size && table[i - 1, j + 2].Value == 0)
            {
                list.Add(table[i - 1, j + 2]);
            }
            //3
            if (i + 1 <Size && j + 2 < Size && table[i+1, j+2].Value == 0)
            {
                list.Add(table[i + 1, j + 2]);
            }
            //4
            if (i + 2 <Size && j + 1 < Size && table[i+2, j+1].Value == 0)
            {
                list.Add(table[i + 2, j + 1]);
            }
            //5
            if (i + 2 <Size && j - 1 >=0 && table[i+2, j-1].Value == 0)
            {
                list.Add(table[i + 2, j - 1]);
            }
            //6
            if (i +1 <Size&& j-2 >=0 && table[i+1, j-2].Value == 0)
            {
                list.Add(table[i +1, j -2]);
            }
            //7
            if (i -1 >= 0 && j -2 >=0 && table[i-1, j-2].Value == 0)
            {
                list.Add(table[i -1, j -2]);
            }
            //8
            if (i - 2 >= 0 && j - 1 >=0 && table[i-2, j-1].Value == 0)
            {
                list.Add(table[i - 2, j - 1]);
            }
            return list;
        }

    }


    public class Cell
    {


        public bool IsPassed { get; set; }

        public int Value { get; set; }

        public int Row { get; set; }

        public int Col { get; set; }

        
    }

    //useless
    public class ChessTable
    {
        public const short SIZE = 5;

        private ulong table;

        private const short CELL_COUNT = SIZE * SIZE;

        public int GetValue(int row, int column)
        {
            if (row < 0 || column < 0 || row >= SIZE || column >= SIZE)
                return -1;
            int index = row * SIZE + column;

            if (((table >> index) & 1UL) == 1UL)
            {
                //return ((table >> index + CellCount) & 1U) == 1U
                //    ? Value.Maximizing
                //    : Value.Minimizing;
                return ((table >> index + CELL_COUNT) & 1UL) == 1UL ? 1 : 2;
            }
            return 0;
        }

        public void SetValue(bool isPassed,int row, int column)
        {
            int index = row * SIZE + column;

            ulong freeCellsMask = 1UL << index;
            ulong valuesMask = 1UL << (index + CELL_COUNT);

            if (isPassed == false)
            {
                table &= ~freeCellsMask;
                table &= ~valuesMask;
            }
            else
            {
                table |= freeCellsMask;
                //if (value == PlayerType.Maximizing)
                //{
                    table |= valuesMask;
                //}
                //else
                //{
                //    table &= ~valuesMask;
                //}
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("  ");
            for (int i = 0; i < ChessTable.SIZE; ++i)
            {
                sb.Append($"{i} ");
            }

            sb.AppendLine();
            for (int i = 0; i < ChessTable.SIZE; ++i)
            {
                sb.Append($"{i}|");
                for (int j = 0; j < ChessTable.SIZE; ++j)
                {
                    int value = GetValue(i, j);
                    if (value == 0)
                    {
                        sb.Append('-');
                    }
                    else if (value == 1)
                    {
                        sb.Append('X');
                    }
                    else
                    {
                        sb.Append('O');
                    }

                    if (j < ChessTable.SIZE - 1)
                    {
                        sb.Append(" ");
                    }
                }

                sb.Append('|');
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }


}
