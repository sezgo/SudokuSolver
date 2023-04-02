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
        [DataRow(1, 2, 1)]
        [DataRow(0, 6, 1)]
        public void CleanHiddenSingleInBlock_HiddenSinglesExist_SolvesFirstHiddenSingle(int row, int col, int expected)
        {
            var currentState = _sudokuBoardStateManager.GenerateState(sudokuBoard);

            _hiddenSingleStrategy.CleanHiddenSingleInBlock(sudokuBoard, row,col);
            var nextState = _sudokuBoardStateManager.GenerateState(sudokuBoard);

            Assert.AreNotEqual(nextState, currentState);
            Assert.AreEqual(sudokuBoard[row, col], expected);
        }

        [TestMethod]
        public void CleanHiddenSingleInBlock_HiddenSingleDoesntExist_DoesntChangeBoardState()
        {
            var currentState = _sudokuBoardStateManager.GenerateState(sudokuBoard);
            _hiddenSingleStrategy.CleanHiddenSingleInBlock(sudokuBoard, 8,0);
            var nextState = _sudokuBoardStateManager.GenerateState(sudokuBoard);
            Assert.AreEqual(nextState, currentState);
        }


        [TestMethod()]
        [DataRow(0, 0, 1, 1, 2)]
        [DataRow(0, 8, 1, 0, 6)]
        public void HasHiddenSingleInBlock_HiddenSingleExists_ReturnsFirstHiddenSingle(int row, int col, int expectedSingle, int expectedRow, int expectedCol)
        {
            var hiddenSingle = _hiddenSingleStrategy.HasHiddenSingleInBlock(sudokuBoard, row, col);
            Assert.IsTrue(hiddenSingle.Single != -1);
            Assert.IsTrue(hiddenSingle.Single == expectedSingle);
            Assert.IsTrue(hiddenSingle.Row == expectedRow);
            Assert.IsTrue(hiddenSingle.Col == expectedCol);
        }


        // NonHiddenSingle: (-1, -1, -1)
        [TestMethod()]
        public void HasHiddenSingleInBlock_HiddenSingleDoesntExists_ReturnsNonHiddenSingle()
        {
            var hiddenSingle = _hiddenSingleStrategy.HasHiddenSingleInBlock(sudokuBoard, 8, 0);
            Assert.IsTrue(hiddenSingle.Single == -1);
        }
        [TestMethod()]
        public void HasHiddenSingleInBlock_EmptyBlock_ReturnsNonHiddenSingle()
        {
            var hiddenSingle = _hiddenSingleStrategy.HasHiddenSingleInBlock(sudokuBoard, 8, 8);
            Assert.IsTrue(hiddenSingle.Single == -1);
        }

        [TestMethod]
        [DataRow(1, 2, '1')]
        [DataRow(0, 6, '1')]
        public void IsHiddenSingleInRow_HiddenSingle_ReturnsTrue(int row, int col, char candidate)
        {
            var result = _hiddenSingleStrategy.IsHiddenSingleInBlock(sudokuBoard, row, col, candidate);
            Assert.IsTrue(result);

        }
        [TestMethod]
        [DataRow(0, 0, '3')]
        [DataRow(2, 8, '4')]
        public void IsHiddenSingleInRow_NonHiddenSingle_ReturnsFalse(int row, int col, char candidate)
        {
            var result = _hiddenSingleStrategy.IsHiddenSingleInBlock(sudokuBoard, row, col, candidate);
            Assert.IsFalse(result);
        }
        [TestMethod]
        public void IsHiddenSingleInRow_SolvedCell_ReturnsFalse()
        {
            var result = _hiddenSingleStrategy.IsHiddenSingleInBlock(sudokuBoard, 8, 0, '1');
            Assert.IsFalse(result);
        }
        [TestMethod]
        public void IsHiddenSingleInBlock_EmptyCell_ReturnsFalse()
        {
            var result = _hiddenSingleStrategy.IsHiddenSingleInBlock(sudokuBoard, 8, 0, '0');
            Assert.IsFalse(result);
        }
    }
}
