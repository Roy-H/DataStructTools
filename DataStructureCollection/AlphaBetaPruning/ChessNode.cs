using DataStructureCollection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Position = System.Tuple<int, int>;

namespace AlphaBetaPruning
{
    public struct Move
    {
        public Position From;
        public Position To;
        public Move(Position from, Position to)
        {
            From = from;
            To = to;
        }
    }
    class ChessNode : INode
    {

        #region Fields
        private readonly ChessTable stateTable;

        private readonly Lazy<PlayerType> winner;

        private readonly Lazy<IReadOnlyList<ChessNode>> children;

        private readonly Lazy<int> heuristics;
        #endregion

        public ChessNode():this(new ChessTable(), PlayerType.Maximizing)
        {

        }

        public ChessNode(ChessTable table,PlayerType playerType)
        {
            stateTable = table;

            winner = new Lazy<PlayerType>(
                () => IsFinished(),
                LazyThreadSafetyMode.ExecutionAndPublication);

            children = new Lazy<IReadOnlyList<ChessNode>>(() => GetChildren(), LazyThreadSafetyMode.ExecutionAndPublication);

            Player = playerType;

            heuristics = new Lazy<int>(
                () => GetHeuristics(),
                LazyThreadSafetyMode.ExecutionAndPublication);

            Opponent = playerType == PlayerType.Maximizing
                ? PlayerType.Minimizing
                : PlayerType.Maximizing;
        }

        private PlayerType IsFinished()
        {
            if (Player == PlayerType.Maximizing)
            {
                for (int i = 0; i < ChessTable.SIZE; i++)
                {
                    for (int j = 0; j < ChessTable.SIZE; j++)
                    {
                        var value = stateTable.GetValue(i, j);
                        if (value == 1)
                        {

                            if (stateTable.GetValue(i + 1, j + 1) != 0 &&
                                stateTable.GetValue(i + 1, j) != 0 &&
                                stateTable.GetValue(i, j + 1) != 0 &&
                                stateTable.GetValue(i - 1, j+1) != 0 &&
                                stateTable.GetValue(i - 1, j-1) != 0 &&
                                stateTable.GetValue(i, j-1) != 0 &&
                                stateTable.GetValue(i + 1, j-1) != 0 &&
                                stateTable.GetValue(i -1, j) != 0)
                            {
                                return PlayerType.Minimizing;
                            }
                            i = ChessTable.SIZE;
                            j = ChessTable.SIZE;
                            /*
                            if (stateTable.GetValue(i + 1, j + 1) == 0)
                            {
                                i = ChessTable.SIZE;
                                break;
                            }
                            else if (stateTable.GetValue(i + 1, j) == 0)
                            {
                                i = ChessTable.SIZE;
                                break;
                            }
                            else if (stateTable.GetValue(i, j + 1) == 0)
                            {
                                i = ChessTable.SIZE;
                                break;
                            }
                            else if (stateTable.GetValue(i + 1, j - 1) == 0)
                            {
                                i = ChessTable.SIZE;
                                break;
                            }
                            else if (stateTable.GetValue(i - 1, j + 1) == 0)
                            {
                                i = ChessTable.SIZE;
                                break;
                            }
                            else if (stateTable.GetValue(i - 1, j - 1) == 0)
                            {
                                i = ChessTable.SIZE;
                                break;
                            }
                            else if (stateTable.GetValue(i, j - 1) == 0)
                            {
                                i = ChessTable.SIZE;
                                break;
                            }
                            else if (stateTable.GetValue(i - 1, j) == 0)
                            {
                                i = ChessTable.SIZE;
                                break;
                            }
                            else
                            {
                                return PlayerType.Minimizing;
                            }
                            */

                        }
                    }
                }
            }
            else if (Player == PlayerType.Maximizing)
            {
                int count = 0;
                for (int i = 0; i < ChessTable.SIZE; i++)
                {
                    for (int j = 0; j < ChessTable.SIZE; j++)
                    {
                        if (stateTable.GetValue(i, j) == 2)
                            count++;
                    }
                }
                if (count < 4)
                {
                    return PlayerType.Maximizing;
                }
            }

            return PlayerType.None;
        }

        private int GetHeuristics()
        {
            return 1;
        }

        private IReadOnlyList<ChessNode> GetChildren()
        {
            if (winner.Value != PlayerType.None)
            {
                return new List<ChessNode>();
            }
            return GetMovements().Select(move =>
            {
                return new ChessNode(GetTableForMove(move), Opponent);
            }).ToList();
        }

        public void ResetTable()
        {
            int[] mins = new int[] { };
            for (int i = 0; i < ChessTable.SIZE; i++)
            {
                for (int j = 0; j < ChessTable.SIZE; j++)
                {
                    if (i == 0 || i == ChessTable.SIZE - 1 || j == 0 || j == ChessTable.SIZE - 1)
                        stateTable.SetValue(PlayerType.Minimizing, i, j);
                    else
                        stateTable.SetValue(PlayerType.None, i, j);
                }
            }
            stateTable.SetValue(PlayerType.Maximizing, ChessTable.SIZE/2, ChessTable.SIZE / 2);
        }

        private IEnumerable<Move> GetMovements()
        {     
            for (short i = 0; i < ChessTable.SIZE; ++i)
            {
                for (short j = 0; j < ChessTable.SIZE; ++j)
                {
                    var value = stateTable.GetValue(i, j);

                    //find movement
                    if ((value == 1&& Player == PlayerType.Maximizing)||(Player == PlayerType.Minimizing&& value == 2))
                    {
                        //eight deriction movement
                        if(stateTable.GetValue(i+1, j) == 0)
                            yield return new Move() { From = new Position(i,j),To = new Position(i + 1,j)};

                        if (stateTable.GetValue(i + 1, j + 1) == 0)
                            yield return new Move() { From = new Position(i, j), To = new Position(i + 1, j + 1) };

                        if (stateTable.GetValue(i , j + 1) == 0)
                            yield return new Move() { From = new Position(i, j), To = new Position(i, j + 1) };

                        if (stateTable.GetValue(i-1, j) == 0)
                            yield return new Move() { From = new Position(i, j), To = new Position(i-1, j) };

                        if (stateTable.GetValue(i - 1, j -1) == 0)
                            yield return new Move() { From = new Position(i, j), To = new Position(i - 1, j-1) };

                        if (stateTable.GetValue(i, j - 1) == 0)
                            yield return new Move() { From = new Position(i, j), To = new Position(i, j - 1) };

                        if (stateTable.GetValue(i - 1, j+1) == 0)
                            yield return new Move() { From = new Position(i, j), To = new Position(i - 1, j+1) };

                        if (stateTable.GetValue(i + 1, j - 1) == 0)
                            yield return new Move() { From = new Position(i, j), To = new Position(i + 1, j - 1) };

                    }
                }
            }
        }

        private ChessTable GetTableForMove(Move move)
        {
            ChessTable newTable = stateTable;
            newTable.SetValue(PlayerType.None, move.From.Item1, move.From.Item2);
            newTable.SetValue(Player, move.To.Item1, move.To.Item2);
            return newTable;
        }

        public PlayerType Player { get; private set; }

        public PlayerType Opponent { get; private set; }

        public IReadOnlyList<INode> Children
        {
            get
            {
                return children.Value;
            }
        }

        public int Heuristics
        {
            get
            {
                return heuristics.Value;
            }
        }
    }
}
