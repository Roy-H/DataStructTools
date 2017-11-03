using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructureCollection.BinaryIndexTree
{
    public class NumMatrix
    {
        int[,] bit, vals;
        int n, m;

        public NumMatrix(int[,] matrix)
        {
            m = matrix.GetLength(0);
            n = matrix.GetLength(1);

            bit = new int[m + 1, n + 1];
            vals = new int[m, n];
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Update(i, j, matrix[i, j]);
                }
            }
        }

        public void Update(int row, int col, int val)
        {
            int d = val - vals[row, col];
            vals[row, col] = val;
            for (int i = row + 1; i <= m; i += (i & -i))
            {
                for (int j = col + 1; j <= n; j += (j & -j))
                {
                    bit[i, j] += d;
                }
            }
        }

        public int SumRegion(int row1, int col1, int row2, int col2)
        {
            return OriginSum(row2, col2) + OriginSum(row1 - 1, col1 - 1) - OriginSum(row2, col1 - 1) - OriginSum(row1 - 1, col2);
        }

        public int OriginSum(int row, int col)
        {
            int sum = 0;
            for (int i = row + 1; i > 0; i -= (i & -i))
            {
                for (int j = col + 1; j > 0; j -= (j & -j))
                {
                    sum += bit[i, j];
                }
            }

            return sum;
        }
    }

    public class NumArray
    {
        int[] bit, vals;
        int length;

        public NumArray(int[] array)
        {
            length = array.Length;

            bit = new int[length+1];
            vals = new int[length];
            for (int i = 0; i < length; i++)
            {
                Update(i, array[i]);
            }
        }

        public void Update(int index, int val)
        {
            int diff = val - vals[index];
            vals[index] = val;
            for (int i = index + 1; i <= length; i += (i & -i))
            {
                bit[i] += diff;
            }
        }

        public int SumRange(int i, int j)
        {
            return OriginSum(j) - OriginSum(i-1);
        }

        private int OriginSum(int i)
        {
            int sum = 0;
            for (int k = i + 1; k > 0; k -= (k & -k))
            {
                sum += bit[k];
            }
            return sum;
        }

    }
}
