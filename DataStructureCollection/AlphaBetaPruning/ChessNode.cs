using DataStructureCollection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace AlphaBetaPruning
{
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

            children = new Lazy<IReadOnlyList<ChessNode>>(() => GetChildren(), LazyThreadSafetyMode.ExecutionAndPublication);
                
        }

        private IReadOnlyList<ChessNode> GetChildren()
        {
            return null;
        }

        public PlayerType Player => throw new NotImplementedException();

        public PlayerType Opponent => throw new NotImplementedException();

        public IReadOnlyList<INode> Children => throw new NotImplementedException();

        public int Heuristics => throw new NotImplementedException();
    }
}
