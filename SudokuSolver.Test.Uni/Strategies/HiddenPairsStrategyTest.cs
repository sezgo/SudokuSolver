using SudokuSolver.Strategies;
using SudokuSolver.Workers;
using System;
using System.Collections.Generic;
using System.Data;
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
        [DataRow(4, 0, 26)]
        [DataRow(5, 0, 26)]
        public void Solve_GivenHiddenPairsInBlock_SolvesExpectedCell(int row, int col, int expected)
        {
            int[,] sudokuBoard =
            {
                { 4578, 478, 9, 1456, 3, 2, 678, 14578, 14568 },
                { 3458, 348, 348, 7, 145689, 14568, 23689, 1234589, 1245689 },
                { 1, 6, 2, 45, 4589, 458, 3789, 345789, 4589 },
                { 34789, 1, 3478, 34, 2, 3478, 5, 6, 3489 },
                { 234678, 3478, 3478, 9, 14568, 1345678, 238, 2348, 248 },
                { 234689, 5, 348, 346, 468, 3468, 1, 23489, 7 },
                { 789, 789, 178, 1256, 156, 156, 4, 125789, 3 },
                { 3478, 2, 6, 1345, 145, 9, 78, 1578, 158 },
                { 349, 349, 5, 8, 7, 1346, 269, 129, 1269 }
            };

            _hiddenPairStrategy.Solve(sudokuBoard);

            Assert.AreEqual(expected, sudokuBoard[row, col]);
            
        }

        [TestMethod]
        public void Solve_NoHiddenPairsInBlock_DoesntChangeState()
        {
            int[,] sudokuBoard =
            {
                { 4578, 23458, 1, 34789, 234678, 234689, 79, 3478, 49 },
                { 0, 0, 0, 0, 0, 26, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 26, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 34789, 16, 3478, 234678, 3478, 3478,234689, 5, 348 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            };
            var currentState = _boardStateManager.GenerateState(sudokuBoard);

            _hiddenPairStrategy.Solve(sudokuBoard);
            var nextState = _boardStateManager.GenerateState(sudokuBoard);

            Assert.AreEqual(currentState, nextState);

        }

        [TestMethod]
        public void Solve_NoHiddenPairInCol_DoesntChangeState()
        {
            int[,] sudokuBoard =
            {
                { 4578, 0, 1, 0, 0, 0, 0, 0, 34789 },
                { 23458, 0, 0, 0, 0, 0, 0, 0, 16 },
                { 1, 0, 0, 0, 0, 0, 0, 0, 3478 },
                { 34789, 0, 0, 0, 0, 0, 0, 0, 234678 },
                { 234678, 0, 0, 0, 0, 0, 0, 0, 3478 },
                { 234689, 26, 26, 0, 0, 0, 0, 0, 3478 },
                { 79, 0, 0, 0, 0, 0, 0, 0, 234689 },
                { 3478, 0, 0, 0, 0, 0, 0, 0, 5 },
                { 49, 0, 0, 0, 0, 0, 0, 0, 348 },
            };
            var currentState = _boardStateManager.GenerateState(sudokuBoard);

            _hiddenPairStrategy.Solve(sudokuBoard);
            var nextState = _boardStateManager.GenerateState(sudokuBoard);

            Assert.AreEqual(currentState, nextState);
        }

        [TestMethod]
        [DataRow(4, 5, 0, 26)]
        [DataRow(5, 4, 0, 26)]
        [DataRow(3, 6, 8, 26)]
        [DataRow(6, 3, 8, 26)]
        public void Solve_GivenHiddenPairInCol_SolvesExpectedCell(int row, int row2, int col, int expected)
        {
            int[,] sudokuBoard =
            {
                { 4578, 0, 1, 0, 0, 0, 0, 0, 34789 },
                { 3458, 0, 0, 0, 0, 0, 0, 0, 1 },
                { 1, 0, 0, 0, 0, 0, 0, 0, 3478 },
                { 34789, 0, 0, 0, 0, 0, 0, 0, 234678 },
                { 234678, 0, 0, 0, 0, 0, 0, 0, 3478 },
                { 234689, 0, 0, 0, 0, 0, 0, 0, 3478 },
                { 789, 0, 0, 0, 0, 0, 0, 0, 234689 },
                { 3478, 0, 0, 0, 0, 0, 0, 0, 5 },
                { 349, 0, 0, 0, 0, 0, 0, 0, 348 },
            };

            _hiddenPairStrategy.Solve(sudokuBoard);
            Assert.AreEqual(expected, sudokuBoard[row, col]);
            Assert.AreEqual(expected, sudokuBoard[row2, col]);
        }

        [TestMethod]
        public void Solve_NoHiddenPairInRow_DoesntChangeState()
        {
            int[,] sudokuBoard =
            {
                { 4578, 23458, 1, 234789, 234678, 234689, 79, 3478, 49 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 34789, 16, 3478, 234678, 3478, 3478,234689, 5, 348 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            };
            var currentState = _boardStateManager.GenerateState(sudokuBoard);

            _hiddenPairStrategy.Solve(sudokuBoard);
            var nextState = _boardStateManager.GenerateState(sudokuBoard);

            Assert.AreEqual(currentState, nextState);
        }

        [TestMethod]
        //[DataRow(0, 4, 5, 26)]
        //[DataRow(0, 5, 4, 26)]
        [DataRow(6, 3, 6, 26)]
        [DataRow(6, 6, 3, 26)]
        public void Solve_GivenHiddenPairInRow_SolvesExpectedCell(int row, int col, int col2, int expected)
        {
            int[,] sudokuBoard =
            {
                //{ 4578, 3458, 1, 34789, 234678, 234689, 789, 3478, 349 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 34789, 1, 3478, 234678, 3478, 3478,234689, 5, 348 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            };

            _hiddenPairStrategy.Solve(sudokuBoard);
            Assert.AreEqual(expected, sudokuBoard[row, col]);
            Assert.AreEqual(expected, sudokuBoard[row, col2]);
        }

    }
}
