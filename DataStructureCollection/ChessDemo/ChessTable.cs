using DataStructureCollection;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlphaBetaPruning
{
    public class ChessTable
    {
        #region fields
        public const short SIZE = 3;

        private const short CELL_COUNT = SIZE * SIZE;

        private uint table;
        #endregion

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

            if (((table >> index) & 1U) == 1U)
            {
                //return ((table >> index + CellCount) & 1U) == 1U
                //    ? Value.Maximizing
                //    : Value.Minimizing;
                return ((table >> index + CELL_COUNT) & 1U) == 1U ? 1: 2;
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

            uint freeCellsMask = 1U << index;
            uint valuesMask = 1U << (index + CELL_COUNT);

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
    }
}
