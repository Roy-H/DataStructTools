using DataStructureCollection;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlphaBetaPruning
{
    public class ChessTable
    {
        #region fields
        public const short SIZE = 5;

        private const short CELL_COUNT = SIZE * SIZE;

        private ulong table;
        #endregion

        public ulong Table
        {
            get
            {                
                return table;
            }
            set
            {
                table = value;
            }
        }

        /// <summary>
        /// Get Value of 2D Table's value by position 
        /// 1 is maximizing, 2 is minimizing, 0 is None -1 is out of bound
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns>1 is maximizing, 2 is minimizing, 0 is None -1 is out of bound</returns>
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
                return ((table >> index + CELL_COUNT) & 1UL) == 1UL ? 1: 2;
            }
            return 0;
        }


        /// <summary>
        /// set value of the table by position
        /// </summary>
        /// <param name="value">
        /// 1 is maximizing 2 is minimizing 0 is default
        /// </param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        public void SetValue(PlayerType value, int row, int column)
        {
            int index = row * SIZE + column;

            ulong freeCellsMask = 1UL << index;
            ulong valuesMask = 1UL << (index + CELL_COUNT);

            if (value == PlayerType.None)
            {
                table &= ~freeCellsMask;
                table &= ~valuesMask;
            }
            else
            {
                table |= freeCellsMask;
                if (value == PlayerType.Maximizing)
                {
                    table |= valuesMask;
                }
                else
                {
                    table &= ~valuesMask;
                }
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
