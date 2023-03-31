using SudokuSolver.Workers;
using System;
using System.Collections.Generic;
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
        public void CleanHiddenSingleInBlock_HiddenSinglesExist_SolvesFirstHiddenSingle()
        {
            var currentState = _sudokuBoardStateManager.GenerateState(sudokuBoard);

            _hiddenSingleStrategy.CleanHiddenSingleInBlock(sudokuBoard, 1,2);
            var nextState = _sudokuBoardStateManager.GenerateState(sudokuBoard);

            Assert.AreNotEqual(nextState, currentState);
            Assert.AreEqual(sudokuBoard[1, 2], 1);

            currentState = _sudokuBoardStateManager.GenerateState(sudokuBoard);

            _hiddenSingleStrategy.CleanHiddenSingleInBlock(sudokuBoard, 2,2);
            nextState = _sudokuBoardStateManager.GenerateState(sudokuBoard);

            Assert.AreNotEqual(nextState, currentState);
            Assert.AreEqual(sudokuBoard[2, 2], 9);

            currentState = _sudokuBoardStateManager.GenerateState(sudokuBoard);

            _hiddenSingleStrategy.CleanHiddenSingleInBlock(sudokuBoard, 0, 6);
            nextState = _sudokuBoardStateManager.GenerateState(sudokuBoard);

            Assert.AreNotEqual(nextState, currentState);
            Assert.AreEqual(sudokuBoard[0, 6], 1);
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
        public void HasHiddenSingleInBlock_HiddenSingleExists_ReturnsFirstHiddenSingle()
        {
            var hiddenSingle = _hiddenSingleStrategy.HasHiddenSingleInBlock(sudokuBoard, 0, 0);
            Assert.IsTrue(hiddenSingle.Single != -1);
            Assert.IsTrue(hiddenSingle.Single == 1);
            Assert.IsTrue(hiddenSingle.Row == 1);
            Assert.IsTrue(hiddenSingle.Col == 2);

            hiddenSingle = _hiddenSingleStrategy.HasHiddenSingleInBlock(sudokuBoard, 0, 8);
            Assert.IsTrue(hiddenSingle.Single != -1);
            Assert.IsTrue(hiddenSingle.Single == 1);
            Assert.IsTrue(hiddenSingle.Row == 0);
            Assert.IsTrue(hiddenSingle.Col == 6);
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
        public void IsHiddenSingleInRow_HiddenSingle_ReturnsTrue()
        {
            var result = _hiddenSingleStrategy.IsHiddenSingleInBlock(sudokuBoard, 1, 2, '1');
            Assert.IsTrue(result);

            result = _hiddenSingleStrategy.IsHiddenSingleInBlock(sudokuBoard, 0, 6, '1');
            Assert.IsTrue(result);

        }
        [TestMethod]
        public void IsHiddenSingleInRow_NonHiddenSingle_ReturnsFalse()
        {
            var result = _hiddenSingleStrategy.IsHiddenSingleInBlock(sudokuBoard, 0, 0, '3');
            Assert.IsFalse(result);

            result = _hiddenSingleStrategy.IsHiddenSingleInBlock(sudokuBoard, 2, 8, '4');
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
