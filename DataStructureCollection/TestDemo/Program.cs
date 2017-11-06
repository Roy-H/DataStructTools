using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStructureCollection;
using DataStructureCollection.BinaryIndexTree;
using DataStructureCollection.Greedy;
using System.Diagnostics;

namespace TestDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //int[] array = new int[]{ 1, 3, 5 };
            //NumArray a = new NumArray(array);
            //Console.WriteLine(a.SumRange(1, 2));
            //a.Update(1, 8);
            //Console.WriteLine(a.SumRange(1, 2));
            //Console.ReadKey();
            HorseGoChess a = new HorseGoChess();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            a.Start();

            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
            Console.ReadKey();
        }
    }
}
