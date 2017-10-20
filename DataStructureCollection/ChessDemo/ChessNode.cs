using DataStructureCollection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                            if ((i + j) % 2 == 0)
                            {
                                if (stateTable.GetValue(i + 1, j + 1) != 0 &&
                                stateTable.GetValue(i + 1, j) != 0 &&
                                stateTable.GetValue(i, j + 1) != 0 &&
                                stateTable.GetValue(i - 1, j + 1) != 0 &&
                                stateTable.GetValue(i - 1, j - 1) != 0 &&
                                stateTable.GetValue(i, j - 1) != 0 &&
                                stateTable.GetValue(i + 1, j - 1) != 0 &&
                                stateTable.GetValue(i - 1, j) != 0)
                                {
                                    return PlayerType.Minimizing;
                                }
                                i = ChessTable.SIZE;
                                j = ChessTable.SIZE;
                            }
                            else
                            {
                                if (
                                stateTable.GetValue(i + 1, j) != 0 &&
                                stateTable.GetValue(i, j + 1) != 0 &&
                                stateTable.GetValue(i, j - 1) != 0 &&                               
                                stateTable.GetValue(i - 1, j) != 0)
                                {
                                    return PlayerType.Minimizing;
                                }
                                i = ChessTable.SIZE;
                                j = ChessTable.SIZE;
                            }
                        }
                        
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

        public INode ResetTable()
        {
            
            for (int i = 0; i < ChessTable.SIZE; i++)
            {
                for (int j = 0; j < ChessTable.SIZE; j++)
                {
                    if (i == 0 ||( i ==( ChessTable.SIZE - 1)) || j == 0 || (j == (ChessTable.SIZE - 1)))
                        stateTable.SetValue(PlayerType.Minimizing, i, j);
                    else
                        stateTable.SetValue(PlayerType.None, i, j);
                }
            }
            stateTable.SetValue(PlayerType.Maximizing, ChessTable.SIZE / 2, ChessTable.SIZE / 2);           
            return this;
        }

        private IEnumerable<Move> GetMovements()
        {
            var a = this;
            for (short i = 0; i < ChessTable.SIZE; ++i)
            {
                for (short j = 0; j < ChessTable.SIZE; ++j)
                {
                    var value = stateTable.GetValue(i, j);

                    //find movement
                    if ((value == 1&& Player == PlayerType.Maximizing)||(Player == PlayerType.Minimizing&& value == 2))
                    {
                        //add deriction movement

                        #region 8 directions
                        if ((i + j) % 2 == 0)
                        {
                            if (stateTable.GetValue(i + 1, j) == 0)
                                yield return new Move() { From = new Position(i, j), To = new Position(i + 1, j) };

                            if (stateTable.GetValue(i + 1, j + 1) == 0)
                                yield return new Move() { From = new Position(i, j), To = new Position(i + 1, j + 1) };

                            if (stateTable.GetValue(i, j + 1) == 0)
                                yield return new Move() { From = new Position(i, j), To = new Position(i, j + 1) };

                            if (stateTable.GetValue(i - 1, j) == 0)
                                yield return new Move() { From = new Position(i, j), To = new Position(i - 1, j) };

                            if (stateTable.GetValue(i - 1, j - 1) == 0)
                                yield return new Move() { From = new Position(i, j), To = new Position(i - 1, j - 1) };

                            if (stateTable.GetValue(i, j - 1) == 0)
                                yield return new Move() { From = new Position(i, j), To = new Position(i, j - 1) };

                            if (stateTable.GetValue(i - 1, j + 1) == 0)
                                yield return new Move() { From = new Position(i, j), To = new Position(i - 1, j + 1) };

                            if (stateTable.GetValue(i + 1, j - 1) == 0)
                                yield return new Move() { From = new Position(i, j), To = new Position(i + 1, j - 1) };
                        }
                        #endregion

                        #region 4 directions
                        else
                        {
                            if (stateTable.GetValue(i + 1, j) == 0)
                                yield return new Move() { From = new Position(i, j), To = new Position(i + 1, j) };

                            if (stateTable.GetValue(i, j + 1) == 0)
                                yield return new Move() { From = new Position(i, j), To = new Position(i, j + 1) };

                            if (stateTable.GetValue(i - 1, j) == 0)
                                yield return new Move() { From = new Position(i, j), To = new Position(i - 1, j) };

                            if (stateTable.GetValue(i, j - 1) == 0)
                                yield return new Move() { From = new Position(i, j), To = new Position(i, j - 1) };
                        }
                        #endregion


                    }
                }
            }
        }

        private ChessTable GetTableForMove(Move move)
        {
            ChessTable newTable = new ChessTable();
            newTable.Table = stateTable.Table;
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

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("  ");
            for (int i = 0; i < ChessTable.SIZE; ++i)
            {
                sb.Append($"{i} ");
            }

            sb.AppendLine();
            for (int i = 0; i < ChessTable.SIZE; ++i)
            {
                sb.Append($"{i}|");
                for (int j = 0; j < ChessTable.SIZE; ++j)
                {
                    int value = stateTable.GetValue(i, j);
                    if (value == 0)
                    {
                        sb.Append('-');
                    }
                    else if (value == 1)
                    {
                        sb.Append('X');
                    }
                    else
                    {
                        sb.Append('O');
                    }

                    if (j < ChessTable.SIZE - 1)
                    {
                        sb.Append(" ");
                    }
                }

                sb.Append('|');
                sb.AppendLine();
            }

            return sb.ToString();
        }

        public INode UpdateTable()
        {
            for (int i = 0; i < ChessTable.SIZE; i++)
            {
                for (int j = 0; j < ChessTable.SIZE; j++)
                {
                    if (stateTable.GetValue(i, j) == 1&& Player == PlayerType.Minimizing)
                    {
                        int LU = stateTable.GetValue(i - 1, j - 1);
                        int L = stateTable.GetValue(i, j - 1);
                        int LD = stateTable.GetValue(i + 1, j-1);
                        int Up = stateTable.GetValue(i-1, j);
                        int Down = stateTable.GetValue(i + 1, j);
                        int R = stateTable.GetValue(i, j + 1);
                        int RU = stateTable.GetValue(i - 1, j + 1);
                        int RD = stateTable.GetValue(i + 1, j + 1);
                        //8 direction
                        if ((i + j) % 2 == 0)
                        {                            
                            if (L == R && L == 2)
                            {
                                stateTable.SetValue(PlayerType.None, i, j - 1);
                                stateTable.SetValue(PlayerType.None, i, j + 1);
                            }
                            if (LU == RD && LU == 2)
                            {
                                stateTable.SetValue(PlayerType.None, i - 1, j - 1);
                                stateTable.SetValue(PlayerType.None, i + 1, j + 1);
                            }

                            if (Up == Down && Up == 2)
                            {
                                stateTable.SetValue(PlayerType.None, i - 1, j);
                                stateTable.SetValue(PlayerType.None, i + 1, j);
                            }

                            if (LD == RU && LD == 2)
                            {
                                stateTable.SetValue(PlayerType.None, i + 1, j-1);
                                stateTable.SetValue(PlayerType.None, i - 1, j + 1);
                            }
                        }
                        else
                        {
                            if (L == R && L == 2)
                            {
                                stateTable.SetValue(PlayerType.None, i, j - 1);
                                stateTable.SetValue(PlayerType.None, i, j + 1);
                            }

                            if (Up == Down && Up == 2)
                            {
                                stateTable.SetValue(PlayerType.None, i - 1, j);
                                stateTable.SetValue(PlayerType.None, i + 1, j);
                            }
                        }
                        i = ChessTable.SIZE;
                        j = ChessTable.SIZE;
                    }     
                }
            }
            return this;
        }
    }
}
