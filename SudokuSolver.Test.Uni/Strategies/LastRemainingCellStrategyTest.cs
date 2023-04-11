using NUnit.Framework.Constraints;
using SudokuSolver.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Test.Unit.Strategies
{
    [TestClass]
    public class LastRemainingCellStrategyTest
    {
        private LastRemainingCellStrategy _lastRemainingCellStrategy = new LastRemainingCellStrategy();

        [TestMethod]
        [DataRow(7, 0, 8)]
        public void LastRemainingCellTest(int row, int col, int expected)
        {
            int[,] sudokuBoard =
            {
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 8, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 8, 0, 0, 0, 0 },
                { 0, 6, 0, 0, 0, 0, 0, 0, 0 },
                { 9, 1, 0, 0, 0, 0, 0, 0, 0 },
            };

            var solved = _lastRemainingCellStrategy.Solve(sudokuBoard);

            Assert.AreEqual(expected, solved[row, col]);
        
        }
    }
}
