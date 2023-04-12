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
    public class NakedTriosStrategyTest
    {
        NakedTriplesStrategy _nakedTriosStrategy = new NakedTriplesStrategy(new SudokuMapper());
        SudokuBoardStateManager _boardStateManager = new SudokuBoardStateManager();

        [TestMethod]
        [DataRow(0, 2, 24)]
        [DataRow(1, 1, 246)]
        [DataRow(1, 2, 24)]
        [DataRow(2, 5, 39)]
        [DataRow(2, 6, 37)]
        [DataRow(2, 7, 7)]
        public void Solve_ExampleSudokuBoard_SolvesExpectedCell(int row, int col, int expected)
        {
            int[,] sudokuBoard =
            {
                { 3, 7, 2458, 158, 156, 1568, 158, 9, 12458 },
                { 9, 12468, 2458, 1358, 7, 13568, 1358, 248, 123458 },
                { 15, 18, 58, 4, 2, 13589, 13578, 78, 6 },
                { 567, 369, 1, 3579, 8, 4, 2, 67, 37 },
                { 24567, 23469, 234579, 123579, 1359, 13579, 136789, 4678, 13478 },
                { 8, 2349, 23479, 6, 139, 1379, 1379, 5, 1347 },
                { 47, 3489, 6, 35789, 3459, 2, 578, 1, 578 },
                { 1247, 1248, 2478, 1578, 1456, 15678, 5678, 3, 9 },
                { 127, 5, 23789, 13789, 1369, 136789, 4, 2678, 278 }
            };

            _nakedTriosStrategy.Solve(sudokuBoard);

            Assert.AreEqual(expected, sudokuBoard[row, col]);
        }

        [TestMethod]
        [DataRow(0, 0, 2, 2, 9)]
        [DataRow(1, 0, 2, 2, 9)]
        [DataRow(1, 1, 2, 2, 9)]
        [DataRow(0, 7, 0, 6, 3)]
        [DataRow(3, 3, 3, 4, 7)]
        public void EliminateNakedTrioFromOthersInBlock_GivenNakedTrio_ChangesExpectedCells(int row, int col, int checkRow, int checkCol, int expectedVal)
        {
            int[,] sudokuBoard = {
                { 15, 0, 0, 0, 0, 0, 1234, 12, 14 },
                { 18, 58, 0, 0, 0, 0, 24, 0, 0 },
                { 0, 0, 1589, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 25, 237, 0, 0, 0, 0 },
                { 0, 0, 0, 35, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 23, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            };

            _nakedTriosStrategy.EliminateNakedTrioFromOthersInBlock(sudokuBoard, row, col);
            Assert.AreEqual(expectedVal, sudokuBoard[checkRow, checkCol]);
        }

        [TestMethod]
        [DataRow(0, 3)]
        [DataRow(3, 6)]
        [DataRow(6, 0)]
        [DataRow(6, 6)]
        [DataRow(0, 1)]
        public void EliminateNakedTrioFromOthersInBlock_EmptyCellAndCellWithoutTrio_DoesntChangeState(int row, int col)
        {
            int[,] sudokuBoard = {
                { 15, 0, 0, 0, 0, 0, 1234, 12, 14 },
                { 18, 58, 0, 0, 0, 0, 24, 0, 0 },
                { 0, 0, 1589, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 25, 237, 0, 0, 0, 0 },
                { 0, 0, 0, 35, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 23, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 12, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 58, 0 },
                { 0, 0, 0, 0, 0, 0, 61, 24, 0 },
            };

            var initialState = _boardStateManager.GenerateState(sudokuBoard);
            

            _nakedTriosStrategy.EliminateNakedTrioFromOthersInBlock(sudokuBoard, row, col);

            var nextState = _boardStateManager.GenerateState(sudokuBoard);
            Assert.AreEqual(initialState, nextState);
        }

        [TestMethod]
        [DataRow(0, 0, 4, 6)]
        [DataRow(0, 0, 3, 79)]
        [DataRow(8, 6, 0, 34)]
        public void EliminateNakedTrioFromOthersInCol_GivenTrio_ChangesState(int row, int col, int changeRow, int expected)
        {
            int[,] sudokuBoard =
            {
                { 35, 0, 0, 0, 0, 0, 12345, 0, 0 },
                { 7, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 8, 0, 0, 0, 0, 0, 12, 0, 0 },
                { 3579, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 3456, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 1, 0, 0, 0, 0, 0, 15, 0, 0 },
                { 2, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 45, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 34, 0, 0, 0, 0, 0, 25, 0, 0 },
            };

            _nakedTriosStrategy.EliminateNakedTrioFromOthersInCol(sudokuBoard, row, col);
            Assert.AreEqual(expected, sudokuBoard[changeRow, col]);
        }

        [TestMethod]
        [DataRow(0, 0, 4, 7)]
        [DataRow(0, 0, 2, 89)]
        public void ELiminateNakedTrioFromOthersInRow_GivenTrio_ChangesState(int row, int col, int changedCol, int expected)
        {
            int[,] sudokuBoard =
            {
                { 35, 0, 34589, 0, 3457, 0, 0, 45, 34 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            };
            _nakedTriosStrategy.ELiminateNakedTrioFromOthersInRow(sudokuBoard, row, col);
            Assert.AreEqual(expected, sudokuBoard[row, changedCol]);
        }

        [TestMethod]
        [DataRow("345", 0, 4, 7)]
        [DataRow("123", 8, 8, 4)]
        public void ELiminateNakedTrio_ProperValues_ReturnsSingle(string valuesToEliminate, int row, int col, int expected)
        {
            int[,] sudokuBoard =
            {
                { 35, 0, 0, 0, 3457, 0, 0, 45, 34 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 12, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 23, 0 },
                { 0, 0, 0, 0, 0, 0, 13, 0, 1234 },
            };
            _nakedTriosStrategy.ELiminateNakedTrio(sudokuBoard, valuesToEliminate, row, col);
            Assert.IsTrue(sudokuBoard[row, col] == expected);

        }

        [TestMethod]
        // First Block
        [DataRow(0, 0, 15, 18, 58)]
        [DataRow(0, 1, 18, 15, 58)]
        [DataRow(1, 0, 58, 15, 18)]
        // Fifth Block
        [DataRow(3, 3, 15, 18, 58)]
        // Seventh Block
        [DataRow(7, 0, 58, 15, 18)]
        // Nineth Block
        [DataRow(8, 8, 58, 15, 18)]
        public void HasNakedTrioInBlock_GivenTrio_ReturnsFirstFoundTrio(int row, int col, int expectedFirst, int expectedSecond, int expectedThird)
        {
            int[,] sudokuBoard =
            {
                { 15, 18, 0, 0, 0, 0, 0, 0, 0 },
                { 58, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 15, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 18, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 58, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 58, 15, 18, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 15, 18, 58 },
            };

            var trio = _nakedTriosStrategy.HasNakedTrioInBlock(sudokuBoard, row, col);
            Assert.IsTrue(trio.IsNakedTrio);
            Assert.AreEqual(expectedFirst, trio.First);
            Assert.AreEqual(expectedSecond, trio.Second);
            Assert.AreEqual(expectedThird, trio.Third); 

        }

        [TestMethod]
        [DataRow(0, 6)]
        [DataRow(3, 0)]
        [DataRow(3, 6)]
        [DataRow(6, 3)]
        public void HasNakedTrioInBlock_EmptyBlock_ReturnsNonTrioTuple(int row, int col, int expectedTrioValue = -1)
        {
            int[,] sudokuBoard =
            {
                { 15, 18, 0, 0, 0, 0, 0, 0, 0 },
                { 58, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 15, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 18, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 58, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 58, 15, 18, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 15, 18, 58 },
            };

            var trio = _nakedTriosStrategy.HasNakedTrioInBlock(sudokuBoard, row, col);
            Assert.IsFalse(trio.IsNakedTrio);
            Assert.AreEqual((expectedTrioValue, expectedTrioValue, expectedTrioValue, false), trio);

        }

        [TestMethod]
        [DataRow(0, 6)]
        [DataRow(0, 7)]
        [DataRow(2, 8)]
        public void HasNakedTrioInBlock_TrioDeosntExist_ReturnsNonTrioTuple(int row, int col, int expectedTrioValue = -1)
        {
            int[,] sudokuBoard =
            {
                { 15, 18, 0, 0, 0, 0, 12, 34, 0 },
                { 58, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 62 },
                { 0, 0, 0, 15, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 18, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 58, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 58, 15, 18, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 15, 18, 58 },
            };

            var trio = _nakedTriosStrategy.HasNakedTrioInBlock(sudokuBoard, row, col);
            Assert.IsFalse(trio.IsNakedTrio);
            Assert.AreEqual((expectedTrioValue, expectedTrioValue, expectedTrioValue, false), trio);

        }

        [TestMethod]
        [DataRow(0, 0)]
        [DataRow(8, 6)]
        public void HasNakedTrioInCol_GivenTrio_ReturnsTrue(int row, int col)
        {
            int[,] sudokuBoard =
            {
                { 35, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 7, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 8, 0, 0, 0, 0, 0, 12, 0, 0 },
                { 9, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 6, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 1, 0, 0, 0, 0, 0, 15, 0, 0 },
                { 2, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 45, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 34, 0, 0, 0, 0, 0, 25, 0, 0 },
            };
            Assert.IsTrue(_nakedTriosStrategy.HasNakedTrioInCol(sudokuBoard, row, col).IsNakedTrio);
        }

        [TestMethod]
        [DataRow(0, 8)]
        [DataRow(0, 1)]
        public void HasNakedTrioInCol_TrioDoesntExist_ReturnsFalse(int row, int col)
        {
            int[,] sudokuBoard =
            {
                { 35, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 7, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 8, 0, 0, 0, 0, 0, 12, 0, 0 },
                { 9, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 6, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 1, 0, 0, 0, 0, 0, 15, 0, 0 },
                { 2, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 45, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 34, 0, 0, 0, 0, 0, 25, 0, 0 },
            };
            Assert.IsFalse(_nakedTriosStrategy.HasNakedTrioInCol(sudokuBoard, row, col).IsNakedTrio);
        }

        [TestMethod]
        [DataRow(0, 3)]
        [DataRow(0, 8)]
        public void HasNakedTrioInCol_EmptyCol_ReturnsFalse(int row, int col)
        {
            int[,] sudokuBoard =
            {
                { 35, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 7, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 8, 0, 0, 0, 0, 0, 12, 0, 0 },
                { 9, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 6, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 1, 0, 0, 0, 0, 0, 15, 0, 0 },
                { 2, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 45, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 34, 0, 0, 0, 0, 0, 25, 0, 0 },
            };
            Assert.IsFalse(_nakedTriosStrategy.HasNakedTrioInCol(sudokuBoard, row, col).IsNakedTrio);
        }

        [TestMethod]
        [DataRow(0, 0)]
        [DataRow(0, 7)]
        [DataRow(0, 8)]
        [DataRow(8, 1)]
        public void HasNakedTrioInRow_NakedTrioExists_ReturnsTrue(int row, int col)
        {
            int[,] sudokuBoard =
            {
                { 35, 7, 8, 9, 6, 1, 2, 45, 34 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 23, 34, 0, 24, 0, 1234, 0, 0 },
            };
            Assert.IsTrue(_nakedTriosStrategy.HasNakedTrioInRow(sudokuBoard, row, col).IsNakedTrio);
        }

        [TestMethod]
        [DataRow(2, 0)]
        [DataRow(5, 7)]
        public void HasNakedTrioInRow_EmptyRow_ReturnsFalse(int row, int col)
        {
            int[,] sudokuBoard =
            {
                { 35, 7, 8, 9, 6, 1, 2, 45, 34 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 123, 0, 456, 0, 789, 0, 0, 34, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 45, 12, 0, 65, 0, 89, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            };
            Assert.IsFalse(_nakedTriosStrategy.HasNakedTrioInRow(sudokuBoard, row, col).IsNakedTrio);
        }

        [TestMethod]
        [DataRow(2, 0)]
        [DataRow(4, 7)]
        public void HasNakedTrioInRow_NakedTrioDoesntExist_ReturnsFalse(int row, int col)
        {
            int[,] sudokuBoard =
            {
                { 35, 7, 8, 9, 6, 1, 2, 45, 34 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 123, 0, 456, 0, 789, 0, 0, 34, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 45, 12, 0, 65, 0, 89, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            };
            Assert.IsFalse(_nakedTriosStrategy.HasNakedTrioInRow(sudokuBoard, row, col).IsNakedTrio);
        }

        [TestMethod]
        [DataRow(15, 18, 58)]
        [DataRow(18, 15, 58)]
        [DataRow(18, 15, 18)]
        [DataRow(18, 15, 15)]
        [DataRow(15, 21, 25)]
        [DataRow(15, 25, 21)]
        [DataRow(34, 45, 35)]
        [DataRow(17, 18, 78)]
        [DataRow(157, 157, 57)]
        [DataRow(178, 178, 178)]
        public void IsNakedTrio_ProperNakedTrio_ReturnsTrue(int first, int second, int third)
        {
            Assert.IsTrue(_nakedTriosStrategy.IsNakedTriple(first, second, third));
        }

        [TestMethod]
        [DataRow(18, 15, 57)]
        [DataRow(15, 21, 23)]
        [DataRow(35, 46, 34)]
        [DataRow(157, 157, 1)]
        [DataRow(178, 178, 1)]
        public void IsNakedTrio_NonNakedTrio_ReturnsFalse(int first, int second, int third)
        {
            Assert.IsFalse(_nakedTriosStrategy.IsNakedTriple(first, second, third));

        }

    }
}
