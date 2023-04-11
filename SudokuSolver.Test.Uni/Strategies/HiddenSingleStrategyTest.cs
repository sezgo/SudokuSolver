using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuSolver.Test.Unit.Strategies;
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
    public class HiddenSingleStrategyTest
    {
        private readonly HiddenSinglesStrategy _hiddenSingleStrategy = new(new SudokuMapper());
        private readonly SudokuBoardStateManager _sudokuBoardStateManager = new SudokuBoardStateManager();

        [TestMethod]
        [DataRow(6, 2, 1)]
        public void Solve_ExampleSudokuBoard_SolvesExpectedCells(int row, int col, int expected)
        {
            int[,] sudokuBoard =
            {
                { 4578, 478, 9, 1456, 3, 2, 678, 14578, 14568 },
                { 3458, 348, 348, 7, 145689, 14568, 23689, 1234589, 1245689 },
                { 1, 6, 2, 45, 4589, 458, 3789, 345789, 4589 },
                { 34789, 1, 3478, 34, 2, 3478, 5, 6, 489 },
                { 234678, 3478, 3478, 9, 14568, 1345678, 238, 2348, 248 },
                { 234689, 5, 348, 346, 468, 3468, 1, 23489, 7 },
                { 789, 789, 178, 1256, 156, 156, 4, 125789, 3 },
                { 3478, 2, 6, 1345, 145, 9, 78, 1578, 158 },
                { 349, 349, 5, 8, 7, 1346, 269, 129, 1269 }
            };

            _hiddenSingleStrategy.Solve(sudokuBoard);

            Assert.AreEqual(expected, sudokuBoard[row, col]);
        }

        [TestMethod]
        [DataRow(1, 2, 1)]
        [DataRow(6, 6, 1)]
        public void CleanHiddenSingle_HiddenSinglesExistInBlock_ChangesState(int row, int col, int expected)
        {
            int[,] sudokuBoard =
            {
                { 23, 235, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 67, 123, 0, 0, 0, 0, 0, 0 },
                { 567, 8, 789, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 123, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 24, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 23, 34 },
            };
            var currentState = _sudokuBoardStateManager.GenerateState(sudokuBoard);

            _hiddenSingleStrategy.CleanHiddenSingle(sudokuBoard, row, col);
            var nextState = _sudokuBoardStateManager.GenerateState(sudokuBoard);

            Assert.AreNotEqual(nextState, currentState);
            Assert.AreEqual(sudokuBoard[row, col], expected);
        }

        [TestMethod]
        [DataRow(0, 2, 3)]
        [DataRow(8, 2, 9)]
        public void CleanHiddenSingle_HiddenSinglesExistInCol_ChangesState(int row, int col, int expected)
        {
            int[,] sudokuBoard =
            {
                { 0, 0, 23, 0, 0, 23, 0, 0, 0 },
                { 0, 0, 25, 0, 0, 32, 0, 0, 0 },
                { 0, 0, 478, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 45, 0, 0, 0 },
                { 0, 0, 145, 0, 0, 54, 0, 0, 0 },
                { 0, 0, 56, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 67, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 78, 0, 0, 78, 0, 0, 0 },
                { 0, 0, 789, 0, 0, 87, 0, 0, 0 },
            };
            var currentState = _sudokuBoardStateManager.GenerateState(sudokuBoard);

            _hiddenSingleStrategy.CleanHiddenSingle(sudokuBoard, row, col);
            var nextState = _sudokuBoardStateManager.GenerateState(sudokuBoard);

            Assert.AreNotEqual(nextState, currentState);
            Assert.AreEqual(expected, sudokuBoard[row, col]);
        }

        [TestMethod]
        [DataRow(0, 0, 1)]
        public void CleanHiddenSingle_HiddenSinglesExistInRow_ChangesState(int row, int col, int expected)
        {
            int[,] sudokuBoard =
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
            var currentState = _sudokuBoardStateManager.GenerateState(sudokuBoard);

            _hiddenSingleStrategy.CleanHiddenSingle(sudokuBoard, row, col);
            var nextState = _sudokuBoardStateManager.GenerateState(sudokuBoard);

            Assert.AreNotEqual(currentState, nextState);
            Assert.AreEqual(expected, sudokuBoard[row, col]);
        }

        [TestMethod]
        [DataRow(8, 0)]
        public void CleanHiddenSingle_NoHiddenSinglesInBlockOrEmptyBlock_DoesntChangeBoardState(int row, int col)
        {
            int[,] sudokuBoard =
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
            var currentState = _sudokuBoardStateManager.GenerateState(sudokuBoard);

            _hiddenSingleStrategy.CleanHiddenSingle(sudokuBoard, 8, 0);
            var nextState = _sudokuBoardStateManager.GenerateState(sudokuBoard);

            Assert.AreEqual(nextState, currentState);
        }

        [TestMethod]
        [DataRow(0, 5)]
        [DataRow(4, 8)]
        public void CleanHiddenSingle_NoHiddenSinglesInColOrEmptyCol_DoesntChangeBoardState(int row, int col)
        {
            int[,] sudokuBoard =
            {
                { 0, 0, 23, 0, 0, 23, 0, 0, 0 },
                { 0, 0, 25, 0, 0, 32, 0, 0, 0 },
                { 0, 0, 478, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 9, 0, 0, 45, 0, 0, 0 },
                { 0, 0, 145, 0, 0, 54, 0, 0, 0 },
                { 0, 0, 56, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 67, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 78, 0, 0, 78, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 87, 0, 0, 0 },
            };
            var currentState = _sudokuBoardStateManager.GenerateState(sudokuBoard);

            _hiddenSingleStrategy.CleanHiddenSingle(sudokuBoard, row, col);
            var nextState = _sudokuBoardStateManager.GenerateState(sudokuBoard);

            Assert.AreEqual(nextState, currentState);
        }

        [TestMethod]
        [DataRow(5, 0)]
        [DataRow(8, 0)]
        public void CleanHiddenSingle_NoHiddenSinglesInRowOrEmptyRow_DoesntChangeBoardState(int row, int col)
        {
            int[,] sudokuBoard =
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
            var currentState = _sudokuBoardStateManager.GenerateState(sudokuBoard);

            _hiddenSingleStrategy.CleanHiddenSingle(sudokuBoard, row, col);
            var nextState = _sudokuBoardStateManager.GenerateState(sudokuBoard);

            Assert.AreEqual(currentState, nextState);
        }

        [TestMethod]
        [DataRow(1, 2, '1')]
        [DataRow(0, 6, '1')]
        public void IsHiddenSingleInBlock_HiddenSingle_ReturnsTrue(int row, int col, char candidate)
        {
            int[,] sudokuBoard =
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
            var result = _hiddenSingleStrategy.IsHiddenSingleForBlock(sudokuBoard, row, col, candidate);
            Assert.IsTrue(result);

        }

        [TestMethod]
        [DataRow(0, 0, '3')]
        [DataRow(2, 8, '4')]
        public void IsHiddenSingleInBlock_NonHiddenSingle_ReturnsFalse(int row, int col, char candidate)
        {
            int[,] sudokuBoard =
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
            var result = _hiddenSingleStrategy.IsHiddenSingleForBlock(sudokuBoard, row, col, candidate);
            Assert.IsFalse(result);
        }

        [TestMethod]
        [DataRow(8, 0, '1')]
        public void IsHiddenSingleInBlock_SolvedCell_ReturnsFalse(int row, int col, char candidate)
        {
            int[,] sudokuBoard =
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
            var result = _hiddenSingleStrategy.IsHiddenSingleForBlock(sudokuBoard, row, col, candidate);
            Assert.IsFalse(result);
        }

        [TestMethod]
        [DataRow(8, 8, '0')]
        public void IsHiddenSingleInBlock_EmptyCell_ReturnsFalse(int row, int col, char candidate)
        {
            int[,] sudokuBoard =
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
            var result = _hiddenSingleStrategy.IsHiddenSingleForBlock(sudokuBoard, row, col, candidate);
            Assert.IsFalse(result);
        }

        [TestMethod]
        [DataRow(4, 2, '1')]
        [DataRow(0, 2, '3')]
        public void IsHiddenSingle_HiddenSingle_ReturnsTrue(int row, int col, char candidate)
        {
            int[,] sudokuBoard =
            {
                { 0, 0, 23, 0, 0, 23, 0, 0, 0 },
                { 0, 0, 25, 0, 0, 32, 0, 0, 0 },
                { 0, 0, 478, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 9, 0, 0, 45, 0, 0, 0 },
                { 0, 0, 145, 0, 0, 54, 0, 0, 0 },
                { 0, 0, 56, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 67, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 78, 0, 0, 78, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 87, 0, 0, 0 },
            };
            var result = _hiddenSingleStrategy.IsHiddenSingleForCol(sudokuBoard, row, col, candidate);

        }

        [TestMethod]
        [DataRow(0, 2, '2')]
        [DataRow(7, 2, '8')]
        public void IsHiddenSingleInCol_NonHiddenSingle_ReturnsFalse(int row, int col, char candidate)
        {
            int[,] sudokuBoard =
            {
                { 0, 0, 23, 0, 0, 23, 0, 0, 0 },
                { 0, 0, 25, 0, 0, 32, 0, 0, 0 },
                { 0, 0, 478, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 9, 0, 0, 45, 0, 0, 0 },
                { 0, 0, 145, 0, 0, 54, 0, 0, 0 },
                { 0, 0, 56, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 67, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 78, 0, 0, 78, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 87, 0, 0, 0 },
            };
            var result = _hiddenSingleStrategy.IsHiddenSingleForCol(sudokuBoard, row, col, candidate);
            Assert.IsFalse(result);
        }

        [TestMethod]
        [DataRow(3, 2, '9')]
        public void IsHiddenSingleInCol_SolvedCell_ReturnsFalse(int row, int col, char candidate)
        {
            int[,] sudokuBoard =
            {
                { 0, 0, 23, 0, 0, 23, 0, 0, 0 },
                { 0, 0, 25, 0, 0, 32, 0, 0, 0 },
                { 0, 0, 478, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 9, 0, 0, 45, 0, 0, 0 },
                { 0, 0, 145, 0, 0, 54, 0, 0, 0 },
                { 0, 0, 56, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 67, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 78, 0, 0, 78, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 87, 0, 0, 0 },
            };
            var result = _hiddenSingleStrategy.IsHiddenSingleForCol(sudokuBoard, row, col, candidate);
            Assert.IsFalse(result);
        }

        [TestMethod]
        [DataRow(0, 0, '1')]
        [DataRow(0, 1, '5')]
        public void IsHiddenSingleInRow_HiddenSingle_ReturnsTrue(int row, int col, char candidate)
        {
            int[,] sudokuBoard =
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
            var result = _hiddenSingleStrategy.IsHiddenSingleForRow(sudokuBoard, row, col, candidate);
            Assert.IsTrue(result);

        }
        [TestMethod]
        [DataRow(0, 1, '3')]
        [DataRow(0, 8, '6')]
        public void IsHiddenSingleInRow_NonHiddenSingle_ReturnsFalse(int row, int col, char candidate)
        {
            int[,] sudokuBoard =
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
            var result = _hiddenSingleStrategy.IsHiddenSingleForRow(sudokuBoard, row, col, candidate);
            Assert.IsFalse(result);
        }
        [TestMethod]
        [DataRow(5, 0, '2')]
        public void IsHiddenSingleInRow_SolvedCell_ReturnsFalse(int row, int col, char candidate)
        {
            int[,] sudokuBoard =
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
            var result = _hiddenSingleStrategy.IsHiddenSingleForRow(sudokuBoard, row, col, candidate);
            Assert.IsFalse(result);
        }

        [TestMethod]
        [DataRow(2, 0, '2')]
        [DataRow(8, 5, '0')]
        public void IsHiddenSingleInRow_EmptyCell_ReturnsFalse(int row, int col, char candidate)
        {
            int[,] sudokuBoard =
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
            var result = _hiddenSingleStrategy.IsHiddenSingleForRow(sudokuBoard, row, col, candidate);
            Assert.IsFalse(result);
        }
    }
}
