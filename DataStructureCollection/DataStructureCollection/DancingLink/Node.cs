using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructureCollection.DancingLink
{
    public class Node
    {
        public Node Left { get; set; }

        public Node Right { get; set; }

        public Node Up { get; set; }

        public Node Down { get; set; }

        public int Col { get; set; }

        public int Row { get; set; }

        public Header Head { get; set; }

        public void RemoveFromColumn()
        {
            Up.Down = this.Down;
            Down.Up = this.Up.Down;
        }

        public void RemoveFromRow()
        {
            Left.Right = this.Right;
            Right.Left = this.Left;
        }

        public void Remove()
        {
            RemoveFromRow();
            RemoveFromColumn();
        }

        public void ReturnToRow()
        {
            Left.Right = this;
            Right.Left = this;
        }
        public void ReturnToColume()
        {
            Up.Down = this;
            Down.Up = this;
        }

        public void Return()
        {
            ReturnToRow();
            ReturnToColume();
        }

        public void RemoveThisRow()
        {
            //removement is from left to right
            for (Node current = Right; current != this; current = current.Right)
                current.RemoveFromColumn();

        }

        public void ReturnThisRow()
        {
            //return is from right to left
            for (Node current = Left; current != this; current = current.Left)
                current.ReturnToColume();
        }
    }

    public class Header:Node
    {
        public int ColumnNum { get; set; }

        public int Count { get; set; }

        public Header(int column)
        {
            ColumnNum = column;
            Count = 0;
            Up = this;
            Down = this;
        }

        public Node AppendDown()
        {
            Node child = new Node();
            Up.Down = child;
            child.Down = this;
            child.Up = Up;
            Up = child;
            Count++;
            //child.head = this;
            return child;
        }

        public Node AppendDown(int row,int col)
        {
            Node child = new Node();
            Up.Down = child;
            child.Down = this;
            child.Up = Up;
            Up = child;
            child.Col = col;
            child.Row = row;
            Count++;
            //child.head = this;
            return child;
        }

        public void Mark()
        {

        }

        public void UnMark()
        {

        }
    }

    public class ExactProblemSolver
    {
        int[,] exampleMatrix = new int[6, 7] 
        { 
            { 0, 0, 1, 0, 1, 1, 0 },
            { 1,0,0,1,0,0,1},
            { 0,1,1,0,0,1,0},
            { 1,0,0,1,0,0,0},
            {0,1,0,0,0,0,1 },
            { 0,0,0,1,1,0,1},            
        };
        string matrix = 
            "0,0,1,0,1,1,0\n"+
            "1,0,0,1,0,0,1\n"+
            "0,1,1,0,0,1,0\n" +
            "1,0,0,1,0,0,0\n" +
            "0,1,0,0,0,0,1\n" +
            "0,0,0,1,1,0,1\n";

        Header head;
        private void BuildMatrix()
        {
            head = new Header(-1);
            var indexHead = head;
            for (int i = 0; i < exampleMatrix.GetLength(1); i++)
            {
                var newHead = new Header(i);
                indexHead.Right = newHead;
                indexHead = newHead;
            }
            indexHead.Right = head;
            head.Left = indexHead;

            indexHead = (Header)head.Right;
            for (int j = 0; j < exampleMatrix.GetLength(0); j++)
            {
                indexHead
                for (int i = 0; i < exampleMatrix.GetLength(1); i++)
                {
                    if (exampleMatrix[i, j] == 1)
                    {

                    }
                }
            }
        }

        public bool Solve(Header root)
        {
            if (root.Right == root)
                return true;
            var rhead = root.Right as Header;
            rhead.Mark();
            for (Node i = rhead.Down; i.Down!=rhead;i = i.Down)
            {
                for (Node j = i.Right;  i != j ;j = j.Right)
                {
                    j.Head.Mark();
                }
                if (Solve(root))
                {
                    return true;
                }
                else
                {
                    for (Node j = i.Right; i != j; j = j.Right)
                    {
                        j.Head.UnMark();
                    }
                }
            }
            return false;
        }
    }
}
