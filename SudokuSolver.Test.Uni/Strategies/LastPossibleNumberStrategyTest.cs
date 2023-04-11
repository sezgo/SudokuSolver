using SudokuSolver.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Test.Unit.Strategies
{
    [TestClass]
    public class LastPossibleNumberStrategyTest
    {
        private readonly LastPossibleNumberStrategy _lastFreeCellStrategy = new LastPossibleNumberStrategy();

        [TestMethod]
        [DataRow(1, 0, 5)]
        public void LastPossibleNumberStrategy_ReturnsSolvedBoard(int row, int col, int expected)
        {
            int[,] sudokuBoard =
            {
                { 2, 4, 6, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 4, 6, 0, 0, 7, 4 },
                { 3, 7, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 1, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 8, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 9, 0, 0, 0, 0, 0, 0, 0, 0 },
            };

            _lastFreeCellStrategy.Solve(sudokuBoard);
            Assert.AreEqual(expected, sudokuBoard[row, col]);
        }
    }
}
