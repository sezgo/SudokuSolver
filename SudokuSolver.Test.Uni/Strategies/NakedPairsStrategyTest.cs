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
