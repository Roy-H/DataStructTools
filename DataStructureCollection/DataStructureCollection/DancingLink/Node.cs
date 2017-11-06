﻿using System;
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

    public class Head:Node
    {
        public int ColNum { get; set; }
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

        Head head;
        private void Init()
        {
            head = new Head();
            for (int i = 0; i < exampleMatrix.GetLength(0); i++)
            {               
                for (int j = 0; j < exampleMatrix.GetLength(1); j++)
                {

                }
            }
        }
    }
}