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
    public class HiddenSingleStrategyBlockTest
    {
        private readonly HiddenSingleStrategy _hiddenSingleStrategy = new(new SudokuMapper());
        private readonly SudokuBoardStateManager _sudokuBoardStateManager = new();
        internal int[,] sudokuBoard =
            {
                { 23, 235, 0, 0, 0, 0, 123, 0, 0 },
                { 0, 67, 123, 0, 0, 0, 24, 0, 0 },
                { 567, 8, 789, 0, 0, 0, 0, 23, 34 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 1, 23, 23, 0, 0, 0, 0, 0, 0 },
            };

        
        [TestMethod]
        [DataRow(1, 2, '1')]
        [DataRow(0, 6, '1')]
        public void IsHiddenSingleInRow_HiddenSingle_ReturnsTrue(int row, int col, char candidate)
        {
            var result = _hiddenSingleStrategy.IsHiddenSingleForBlock(sudokuBoard, row, col, candidate);
            Assert.IsTrue(result);

        }
        [TestMethod]
        [DataRow(0, 0, '3')]
        [DataRow(2, 8, '4')]
        public void IsHiddenSingleInRow_NonHiddenSingle_ReturnsFalse(int row, int col, char candidate)
        {
            var result = _hiddenSingleStrategy.IsHiddenSingleForBlock(sudokuBoard, row, col, candidate);
            Assert.IsFalse(result);
        }
        [TestMethod]
        public void IsHiddenSingleInRow_SolvedCell_ReturnsFalse()
        {
            var result = _hiddenSingleStrategy.IsHiddenSingleForBlock(sudokuBoard, 8, 0, '1');
            Assert.IsFalse(result);
        }
        [TestMethod]
        public void IsHiddenSingleInBlock_EmptyCell_ReturnsFalse()
        {
            var result = _hiddenSingleStrategy.IsHiddenSingleForBlock(sudokuBoard, 8, 0, '0');
            Assert.IsFalse(result);
        }
    }
}
