using SudokuSolver.Strategies;
using SudokuSolver.Workers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Test.Unit.Strategies
{
    [TestClass]
    public class HiddenPairsStrategyTest
    {
        private readonly HiddenPairsStrategy _hiddenPairStrategy = new HiddenPairsStrategy(new SudokuMapper());
        private readonly SudokuBoardStateManager _boardStateManager = new SudokuBoardStateManager();

        [TestMethod]
        [DataRow(0, 4, 5, 26)]
        [DataRow(0, 5, 4, 26)]
        public void FindHiddenPairInRowTest(int row, int col, int col2, int hiddenPair)
        {
            int[,] sudokuBoard =
            {
                { 4578, 3458, 1, 34789, 234678, 234689, 789, 3478, 349 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            };
            var currentState = _boardStateManager.GenerateState(sudokuBoard);

            _hiddenPairStrategy.SolveForHiddenPairInRow(sudokuBoard, row, col);
            var nextState = _boardStateManager.GenerateState(sudokuBoard);
            
            Assert.AreNotEqual(currentState, nextState);
            Assert.AreEqual(hiddenPair, sudokuBoard[row, col]);
            Assert.AreEqual(hiddenPair, sudokuBoard[row, col2]);
        }
    }
}
