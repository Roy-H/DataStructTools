using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinder
{
    class Program
    {
        
        static void Main(string[] args)
        {
            var finder = new PathFinder();
            //finder.Doit(1, 2);
            //finder.Doit2(1, 3);
            finder.Folyd(1, 2);
            Console.ReadKey();
            
        }

        
    }

    class PathFinder
    {
        const int maxn = 256;
        List<KeyValuePair<int, int>>[] list = new List<KeyValuePair<int, int>>[maxn];
        
        int[] distance = new int[maxn];
        int[] inq = new int[maxn];
        int[,] matrix = new int[256, 256];

        void init()
        {
            for (int i = 0; i < maxn; i++)
            {
                distance[i] = short.MaxValue;
                inq[i] = 0;

            }
            for (int i = 0; i < maxn; i++)
            {
                for (int j = 0; j < maxn; j++)
                {
                    matrix[i, j] = short.MaxValue;
                }
            }
            matrix[1, 2] = 3;
            matrix[1, 3] = 1;
            matrix[2, 3] = 1;
            matrix[2, 1] = 3;
            matrix[3, 2] = 1;
            matrix[3, 1] = 1;

            list[1] = new List<KeyValuePair<int, int>>() { new KeyValuePair<int, int>(2, 3), new KeyValuePair<int, int>(3, 1) };
            list[2] = new List<KeyValuePair<int, int>>() { new KeyValuePair<int, int>(1, 3), new KeyValuePair<int, int>(3, 1) };
            list[3] = new List<KeyValuePair<int, int>>() { new KeyValuePair<int, int>(1, 1), new KeyValuePair<int, int>(2, 1) };
        }

        Queue<int> Q = new Queue<int>();
        SortedList<int, int> sortedList = new SortedList<int, int>();

        public void Doit2(int start, int target)
        {
            init();
            sortedList.Add(0, start);
            distance[start] = 0;
            while (sortedList.Count > 0)
            {
                int now = sortedList.First().Value;
                sortedList.RemoveAt(0);
                for (int i = 0; i < list[now].Count; i++)
                {
                    if (distance[list[now][i].Key] > distance[now] + list[now][i].Value)
                    {
                        distance[list[now][i].Key] = distance[now] + list[now][i].Value;
                        if (sortedList.Values.Contains(list[now][i].Key))
                            continue;
                        sortedList.Add(distance[list[now][i].Key], list[now][i].Key);
                    }
                }
            }
            Console.WriteLine(distance[target]);

        }

        public void Doit(int start, int target)
        {
            init();

            Q.Enqueue(start);
            inq[start] = 1;
            distance[start] = 0;
            while (Q.Count > 0)
            {
                int now = Q.Peek();
                Q.Dequeue();
                inq[now] = 0;
                for (int i = 0; i < list[now].Count; i++)
                {
                    if (distance[list[now][i].Key] > distance[now] + list[now][i].Value)
                    {
                        distance[list[now][i].Key] = distance[now] + list[now][i].Value;
                        if (inq[list[now][i].Key] == 1)
                            continue;
                        Q.Enqueue(list[now][i].Key);
                        inq[list[now][i].Key] = 1;
                    }
                }
            }
            Console.WriteLine(distance[target]);
        }

        public void Folyd(int start, int target)
        {
            init();
            for (int k = 0; k < 256; k++)
            {
                for (int i = 0; i < 256; i++)
                {
                    for (int j = 0; j < 256; j++)
                    {
                        matrix[i, j] = Math.Min(matrix[i, j],matrix[i, k] + matrix[k, j]);
                    }
                }
            }
            Console.WriteLine(matrix[start, target]);
        }
    }

    
}
