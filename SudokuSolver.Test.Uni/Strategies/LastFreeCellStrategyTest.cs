using SudokuSolver.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Test.Unit.Strategies
{
    [TestClass]
    public class LastFreeCellStrategyTest
    {
        private readonly LastFreeCellStrategy _lastFreeCellStrategy = new LastFreeCellStrategy();

        [TestMethod]
        [DataRow(0, 7, 8)]
        public void LastFreeCellStrategy_ReturnsSolvedBoard(int row, int col, int expected)
        {
            int[,] sudokuBoard =
            {
                { 1, 2, 3, 4, 5, 6, 7, 0, 9 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            };

            _lastFreeCellStrategy.Solve(sudokuBoard);
            Assert.AreEqual(expected, sudokuBoard[row, col]);
        }

        [TestMethod]
        [DataRow(1, 0, 23456789)]
        [DataRow(1, 1, 23456789)]
        [DataRow(1, 2, 23456789)]
        [DataRow(2, 0, 23456789)]
        [DataRow(2, 1, 23456789)]
        [DataRow(2, 2, 23456789)]
        [DataRow(5, 4, 123456789)]
        public void LastFreeCellStrategy_ReturnsPossibilitesForUnsolvedCells(int row, int col, int expected)
        {
            int[,] sudokuBoard =
            {
                { 1, 245, 368, 456, 78, 67, 79, 789, 49 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            };

            _lastFreeCellStrategy.Solve(sudokuBoard);
            Assert.AreEqual(expected, sudokuBoard[row, col]);
        }
    }
}
