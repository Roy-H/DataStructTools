using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DataStructureCollection
{
    public class AlphaBetaPruning
    {

    }

    public class AlphaBeta<Node> where Node : INode
    {
        public uint Depth { get; set; }


        public async Task<Node> BestAsync(Node root)
        {
            if (root == null)
            {
                throw new ArgumentNullException(nameof(root), "Null node.");
            }
            else if (root.Player == PlayerType.None)
            {
                throw new ArgumentException(nameof(root.Player), "The player is not specified.");
            }

            bool maximing = root.Player == PlayerType.Maximizing;

            List<Task<Tuple<int, Node>>> tasks = new List<Task<Tuple<int, Node>>>();
            foreach (Node child in root.Children)
            {
                tasks.Add(Task.Run(() => new Tuple<int, Node>
                (
                    Search(
                        child,
                        Depth-1,
                        int.MinValue,
                        int.MaxValue,
                        !maximing
                        ),
                    child
                    )));
            }

            Tuple<int, Node>[] results =await Task.WhenAll(tasks);
            Node bestNode = default(Node);
            int bestValue;
            if (maximing)
            {
                bestValue = int.MinValue;
                foreach (var result in results)
                {
                    if (result.Item1 > bestValue)
                    {
                        bestNode = result.Item2;
                        bestValue = result.Item1;
                    }
                }
            }
            else
            {
                bestValue = int.MaxValue;
                foreach (var result in results)
                {
                    if (result.Item1 < bestValue)
                    {
                        bestNode = result.Item2;
                        bestValue = result.Item1;
                    }
                }

            }

            //if bestNode is null, it is exception.
            return bestNode;
        }

        public Node Best(Node root)
        {
            if (root == null)
            {
                throw new ArgumentNullException(nameof(root), "Null node.");
            }
            else if (root.Player == PlayerType.None)
            {
                throw new ArgumentException(nameof(root.Player), "The player is not specified.");
            }

            bool maximing = root.Player == PlayerType.Maximizing;
            //List<Tuple<int, Node>> result = new List<Tuple<int, Node>>();

            Node bestNode = default(Node);
            int bestValue;
            foreach (Node child in root.Children)
            {
                var value = Search(child, Depth - 1, int.MinValue, int.MaxValue, !maximing);
                if (maximing)
                {
                    bestValue = int.MinValue;
                    if (value > bestValue)
                    {
                        bestNode = child;
                        bestValue = value;
                    }
                }
                else
                {
                    bestValue = int.MaxValue;
                    if (value < bestValue)
                    {
                        bestNode = child;
                        bestValue = value;
                    }
                }
            }
            //if bestNode is null, it is exception.
            return bestNode;
        }

        private int Search(Node node, uint depth, int alpha, int beta, bool maximizing)
        {
            if (depth == 0 || node.Children.Count == 0)
            {
                return node.Heuristics;
            }

            if (maximizing)
            {
                int value = int.MinValue;
                
                foreach (Node child in node.Children)
                {
                    value = Math.Max(value, Search(child, depth - 1, alpha, beta, false));
                    alpha = Math.Max(alpha, value);
                    if (beta <= alpha)
                    {
                        break;
                    }
                }
                return value;
            }
            else
            {
                int value = int.MaxValue;

                foreach (Node child in node.Children)
                {
                    value = Math.Min(value, Search(child, depth - 1, alpha, beta, true));
                    beta = Math.Min(beta, value);
                    if (beta <= alpha)
                    {
                        break;
                    }
                }
                return value;
            }
        }
    }

    public class AlphaBetaHelper<Node> where Node:INode
    {
        //private uint searchDepth;
        public uint Depth { get; set; }

        private void InitTree()
        {
            
        }

        public AlphaBetaHelper()
        {
            Depth = 3;
        }

        public async Task<Node> Best(Node root)
        {
            if (root == null)
            {
                throw new Exception();
            }
            else if (root.Player == PlayerType.None)
            {
                throw new Exception();
            }
            bool maximizing = root.Player == PlayerType.Maximizing;
            for (int i = 0; i < root.Children.Count; i++)
            {

            }
            await Task.Factory.StartNew(() => 
            {
                
            });
            return root;
        }

       


    }

    public enum PlayerType
    {
        /// <summary>
        /// Not specified player or empty cell.
        /// </summary>
        None,

        /// <summary>
        /// The maximizing player which starts the game.
        /// </summary>
        Maximizing,

        /// <summary>
        /// The minimizing player.
        /// </summary>
        Minimizing
    }

    public class Node:INode
    {
        
        public Node()
        {
            Children = new List<Node>();
        }

        public bool AddChild(Node node)
        {
            Debug.Assert(object.Equals(null, node),"node is null error");
            if(Children == null)
                Children = new List<Node>();
            if (Children.Contains(node))
                return false;
            Children.Add(node);
            return true;
        }

        public bool RemoveChildNode(Node node)
        {
            if (node == null)
                return false;
            if (!Children.Contains(node))
                return false;
            Children.Remove(node);
            return true;
        }
       
        public int Value { get; set; }

        public int Alpha { get; set; }

        public int Beta { get; set; }

        public IList<Node> Children { get; set; }      

        public PlayerType LType { get; set; }

        public PlayerType Player => throw new NotImplementedException();

        public PlayerType Opponent => throw new NotImplementedException();

        IReadOnlyList<INode> INode.Children => throw new NotImplementedException();

        public int Heuristics => throw new NotImplementedException();
    }

    public interface INode
    {
        /// <summary>
        /// Gets the current player.
        /// </summary>
        PlayerType Player { get; }

        /// <summary>
        /// Gets the opponent of current player.
        /// </summary>
        PlayerType Opponent { get; }

        /// <summary>
        /// Gets the list of children nodes.
        /// </summary>
        IReadOnlyList<INode> Children { get; }

        /// <summary>
        /// Gets the heuristics value of current position
        /// from maximizing player's perspective.
        /// </summary>
        int Heuristics { get; }
    }
}
