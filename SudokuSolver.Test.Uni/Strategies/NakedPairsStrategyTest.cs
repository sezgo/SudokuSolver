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
