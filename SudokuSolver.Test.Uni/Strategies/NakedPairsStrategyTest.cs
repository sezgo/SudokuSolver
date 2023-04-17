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
    public class NakedPairsStrategyTest
    {
        private readonly ISudokuStrategy _nakedPairsStrategy = new NakedPairsStrategy(new SudokuMapper());
        private readonly SudokuBoardStateManager _boardStateManager = new SudokuBoardStateManager();
        [TestMethod]
        public void Solve_NakedPairOnRow_ReturnsExpectedSudoukoBoardState()
        {
            int[,] sudokuBoard =
            {
                { 8, 4, 3567, 2, 357, 357, 36, 9, 1 },
                { 23, 2357, 357, 1, 6, 9, 4, 23, 8 },
                { 1, 236, 9, 4, 38, 38, 236, 5, 7 },
                { 7, 1, 8, 3, 2, 4, 59, 6, 59 },
                { 349, 35, 2, 59, 178, 6, 35789, 1347, 3459 },
                { 3469, 356, 3456, 59, 178, 78, 235789, 12347, 23459 },
                { 234, 237, 1, 6, 9, 235, 2357, 8, 2345 },
                { 2346, 9, 3467, 8, 35, 235, 1, 2347, 23456 },
                { 5, 8, 36, 7, 4, 1, 2369, 23, 2369 }
            };
            int[,] expectedsudokuBoard =
            {
                { 8, 4, 36, 2, 57, 57, 36, 9, 1 },
                { 23, 57, 57, 1, 6, 9, 4, 23, 8 },
                { 1, 26, 9, 4, 38, 38, 26, 5, 7 },
                { 7, 1, 8, 3, 2, 4, 59, 6, 59 },
                { 349, 35, 2, 59, 178, 6, 378, 147, 34 },
                { 3469, 356, 45, 59, 178, 78, 2378, 147, 234 },
                { 234, 237, 1, 6, 9, 235, 2357, 8, 2345 },
                { 236, 9, 47, 8, 35, 235, 1, 47, 2356 },
                { 5, 8, 36, 7, 4, 1, 2369, 23, 2369 }
            };

            string currentState;
            string solvedState;
            do
            {
                currentState = _boardStateManager.GenerateState(sudokuBoard);
                _nakedPairsStrategy.Solve(sudokuBoard);
                solvedState = _boardStateManager.GenerateState(sudokuBoard);

            } while (currentState != solvedState);
            string expectedState = _boardStateManager.GenerateState(expectedsudokuBoard);

            Assert.AreEqual(expectedState, solvedState);
        }

        [TestMethod]
        [DataRow(0, 3, 6)]
        [DataRow(1, 3, 79)]
        [DataRow(1, 5, 4)]
        [DataRow(2, 5, 79)]
        public void Solve_ExampleSudokuBoard_SolvesExpectedCell(int row, int col, int expected)
        {
            int[,] sudoukuBoard =
            {
                { 1367, 13679, 2, 679, 8, 5, 179, 79, 4 },
                { 157, 1789, 15789, 79, 3, 479, 125789, 6, 1579 },
                { 567, 6789, 4, 2, 1, 79, 5789, 3, 579 },
                { 13467, 134678, 13678, 136789, 4679, 34789, 4789, 5, 2},
                { 4567, 24678, 5678, 56789, 245679, 24789, 3, 1, 679 },
                { 9, 1234678, 135678, 135678, 24567, 234578, 478, 478, 67 },
                { 8, 13479, 1379, 3579, 2579, 6, 124579, 2479, 13579 },
                { 2, 5, 13679, 4, 79, 379, 1479, 79, 8 },
                { 347, 3479, 379, 35789, 2579, 1, 6, 247, 3579 }
            };
            _nakedPairsStrategy.Solve(sudoukuBoard);

            Assert.AreEqual(expected, sudoukuBoard[row, col]);

        }

        [TestMethod]
        public void ShouldEliminateNumbersInRowBasedOnNakedPair()
        {
            int[,] sudokuBoard =
            {
                { 1, 2, 34, 5, 34, 6, 7, 348, 9 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            };

            var solvedSudokuBoard = _nakedPairsStrategy.Solve(sudokuBoard);
            Assert.IsTrue(solvedSudokuBoard[0,7] == 8);
        }
        [TestMethod]
        public void ShouldEliminateNumbersInColBasedOnNakedPair()
        {
            int[,] sudokuBoard =
            {
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 1, 56, 3 },
                { 0, 0, 0, 0, 0, 0, 7, 8, 4 },
                { 0, 0, 0, 0, 0, 0, 562, 9, 56 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            };

            var solvedSudokuBoard = _nakedPairsStrategy.Solve(sudokuBoard);
            Assert.IsTrue(solvedSudokuBoard[5, 6] == 2);
        }
        [TestMethod]
        public void ShouldEliminateNumbersInBlockBasedOnNakedPair()
        {
            int[,] sudokuBoard =
            {
                { 1, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 24, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 3, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 5, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 6, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 24, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 7, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 8, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 249, 0, 0, 0, 0, 0, 0, 0, 0 },
            };

            var solvedSudokuBoard = _nakedPairsStrategy.Solve(sudokuBoard);
            Assert.IsTrue(solvedSudokuBoard[8, 0] == 9);
        }
    }
}
