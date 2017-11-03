using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStructureCollection;
using DataStructureCollection.BinaryIndexTree;

namespace TestDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] array = new int[]{ 1, 3, 5 };
            NumArray a = new NumArray(array);
            Console.WriteLine(a.SumRange(1, 2));
            a.Update(1, 8);
            Console.WriteLine(a.SumRange(1, 2));
            Console.ReadKey();
        }
    }
}
