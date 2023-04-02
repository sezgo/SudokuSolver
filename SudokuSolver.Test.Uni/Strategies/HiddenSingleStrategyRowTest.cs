using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuSolver.Test.Unit.Strategies;
using SudokuSolver.Strategies;
using SudokuSolver.Workers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Test.Unit.Strategies.Tests
{
    [TestClass]
    public class HiddenSingleStrategyRowTest
    {
        private readonly HiddenSingleStrategy _hiddenSingleStrategy = new(new SudokuMapper());
        private readonly SudokuBoardStateManager _sudokuBoardStateManager = new SudokuBoardStateManager();

        internal int[,] sudokuBoard = 
            {
                { 123, 2345, 34, 23, 344, 62, 72, 348, 96 }, // 1 is a hidden single
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 2, 2345, 345, 23, 344, 62, 2, 34, 6 }, // no hidden singles
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            };

        [TestMethod]
        [DataRow(0, 0, 0, 1)]
        public void CleanHiddenSingleInRow_HiddenSinglesExist_SolvesFirstHiddenSingle(int row, int checkRow, int checkCol, int expected)
        {
            var currentState = _sudokuBoardStateManager.GenerateState(sudokuBoard);
            _hiddenSingleStrategy.CleanHiddenSingleInRow(sudokuBoard, row);
            var nextState = _sudokuBoardStateManager.GenerateState(sudokuBoard);
            Assert.AreNotEqual(currentState, nextState);
            Assert.AreEqual(expected, sudokuBoard[checkRow, checkCol]);
        }

        [TestMethod]
        public void CleanHiddenSingleInRow_NoHiddenSingles_DoesntChangeBoardState()
        {
            var currentState = _sudokuBoardStateManager.GenerateState(sudokuBoard);
            _hiddenSingleStrategy.CleanHiddenSingleInRow(sudokuBoard, 5);
            var nextState = _sudokuBoardStateManager.GenerateState(sudokuBoard);
            Assert.AreEqual(currentState, nextState);
        }

        [TestMethod()]
        public void HasHiddenSingleInRow_HiddenSingleExists_ReturnsFirstHiddenSingle()
        {
            var hiddenSingle = _hiddenSingleStrategy.HasHiddenSingleInRow(sudokuBoard, 0);
            Assert.IsTrue(hiddenSingle.Single != -1);
            Assert.IsTrue(hiddenSingle.Single == 1);
        }


        // NonHiddenSingle: (-1, -1, -1)
        [TestMethod()]
        public void HasHiddenSingleInRow_HiddenSingleDoesntExists_ReturnsNonHiddenSingle()
        {
            var hiddenSingle = _hiddenSingleStrategy.HasHiddenSingleInRow(sudokuBoard, 5);
            Assert.IsTrue(hiddenSingle.Single == -1);
        }
        [TestMethod()]
        public void HasHiddenSingleInRow_EmptyRow_ReturnsNonHiddenSingle()
        {
            var hiddenSingle = _hiddenSingleStrategy.HasHiddenSingleInRow(sudokuBoard, 1);
            Assert.IsTrue(hiddenSingle.Single == -1);
        }

        [TestMethod]
        [DataRow(0, 0, '1')]
        [DataRow(0, 1, '5')]
        public void IsHiddenSingleInRow_HiddenSingle_ReturnsTrue(int row, int col, char candidate)
        {
            var result = _hiddenSingleStrategy.IsHiddenSingleInRow(sudokuBoard, row, col, candidate);
            Assert.IsTrue(result);

        }
        [TestMethod]
        [DataRow(0, 1, '3')]
        [DataRow(0, 8, '6')]
        public void IsHiddenSingleInRow_NonHiddenSingle_ReturnsFalse(int row, int col, char candidate)
        {
            var result = _hiddenSingleStrategy.IsHiddenSingleInRow(sudokuBoard, row, col, candidate);
            Assert.IsFalse(result);
        }
        [TestMethod]
        [DataRow(5, 0, '2')]
        public void IsHiddenSingleInRow_SolvedCell_ReturnsFalse(int row, int col, char candidate)
        {
            var result = _hiddenSingleStrategy.IsHiddenSingleInRow(sudokuBoard, row, col, candidate);
            Assert.IsFalse(result);
        }
    }
}
