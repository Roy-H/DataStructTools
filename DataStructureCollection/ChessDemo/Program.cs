﻿using AlphaBeta;
using AlphaBetaPruning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStructureCollection;

namespace ChessDemo
{
    class Program
    {
        /// <summary>
        /// Entry point.
        /// </summary>
        /// <param name="args">The console arguments.</param>
        public static void Main(string[] args)
        {
            //AlphaBetaDemo(new ReversiNode(), 5U);
            //AlphaBetaDemo(new TicTacToeNode(), 2U);
            AlphaBetaDemo2((new ChessNode().ResetTable()), 3U);
        }

        private static void AlphaBetaDemo2<Node>(Node state, uint depth) where Node: DataStructureCollection.INode
        {
            DataStructureCollection.AlphaBeta<Node> search = new DataStructureCollection.AlphaBeta<Node>() { Depth = depth};
            //state.ResetTable();
            //Console.WriteLine(state);
            while (state.Children.Any())
            {
                
                state = search.BestAsync(state).Result;
                Console.WriteLine(state);
                //update state
                if (state.Opponent == PlayerType.Maximizing)
                {
                    //only when max remove can delete node of min
                    state.UpdateTable();
                }               
                Console.WriteLine(state);
            }

            Console.WriteLine(state);

            Value winner = state.Heuristics > 0 ? Value.Maximizing
                : state.Heuristics < 0 ? Value.Minimizing
                : Value.None;

            Console.WriteLine($"Game over. Winner: {winner}.");
            Console.WriteLine();
        }

        /// <summary>
        /// Demo run of the game in computer against computer mode.
        /// </summary>
        /// <typeparam name="Node">The type of the node.</typeparam>
        /// <param name="state">The initial state.</param>
        /// <param name="depth">The search depth.</param>
        private static void AlphaBetaDemo<Node>(Node state, uint depth) where Node : AlphaBeta.INode
        {
            AlphaBeta.AlphaBeta<Node> search = new AlphaBeta.AlphaBeta<Node>(depth);

            while (state.Children.Any())
            {
                Console.WriteLine(state);
                state = search.Best(state).Result;
            }

            Console.WriteLine(state);

            Value winner = state.Heuristics > 0 ? Value.Maximizing
                : state.Heuristics < 0 ? Value.Minimizing
                : Value.None;

            Console.WriteLine($"Game over. Winner: {winner}.");
            Console.WriteLine();
        }
    }
}