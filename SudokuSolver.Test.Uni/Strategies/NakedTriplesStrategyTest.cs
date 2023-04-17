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
    public class NakedTriplesStrategyTest
    {
        NakedTriplesStrategy _nakedTriplesStrategy = new NakedTriplesStrategy(new SudokuMapper());
        SudokuBoardStateManager _boardStateManager = new SudokuBoardStateManager();

        [TestMethod]
        // 48 24 248
        [DataRow(7, 0, 7)]
        [DataRow(7, 1, 569)]
        [DataRow(7, 2, 48)]
        [DataRow(7, 3, 24)]
        [DataRow(7, 4, 56)]
        [DataRow(7, 5, 248)]
        [DataRow(7, 6, 1)]
        [DataRow(7, 7, 59)]
        [DataRow(7, 8, 3)]
        public void Solve_NakedTripleInRow_SolvesExpectedCell(int row, int col, int expected)
        {
            int[,] sudokuBoard =
            {
                { 5, 38, 148, 7, 43, 6, 9, 14, 2 },
                { 46, 36, 9, 234, 1, 234, 8, 7, 5 },
                { 2, 7, 14, 8, 9, 5, 3, 14, 6 },
                { 3, 4, 7, 6, 8, 9, 25 ,25, 1 },
                { 9, 1, 5, 234, 234, 234, 7, 6, 8 },
                { 8, 2, 6, 5, 7, 1, 4, 3, 9 },
                { 46, 5689, 2, 1, 3456, 348, 56, 589, 3 },
                { 7, 5689, 48, 24, 2456, 248, 1, 2589, 3 },
                { 1, 568, 3, 9, 256, 7, 256, 258, 4 }
            };

        }

        [TestMethod]
        // 158 18 158
        [DataRow(0, 0, 4)]
        [DataRow(0, 1, 79)]
        [DataRow(0, 2, 6)]
        [DataRow(1, 0, 158)]
        [DataRow(1, 1, 3)]
        [DataRow(1, 2, 18)]
        [DataRow(2, 0, 158)]
        [DataRow(2, 1, 279)]
        [DataRow(2, 2, 29)]
        public void Solve_NakedTripleInBlock_SolvesExpectedCell(int row, int col, int expected)
        {
            int[,] sudokuBoard =
            {
                { 4, 79, 6, 389, 5, 1, 2, 78, 37 },
                { 158, 3, 18, 468, 2, 4678, 48, 15678, 9 },
                { 158, 12579, 1289, 34689, 378, 34678, 348, 15678, 3567 },
                { 13, 6, 4, 2, 13, 5, 7, 9, 8 },
                { 7, 19, 139, 1368, 138, 368, 5, 2, 4 },
                { 2, 8, 5, 7, 4, 9, 6, 3, 1 },
                { 13568, 125, 1238, 1348, 1378, 23478, 9, 5678, 3567 },
                { 9, 125, 1238, 138, 6, 2378, 38, 4, 357 },
                { 368, 4, 7, 5, 9, 38, 1, 86, 2 }
            };

            _nakedTriplesStrategy.Solve(sudokuBoard);

            Assert.AreEqual(expected, sudokuBoard[row, col]);
        }

        [TestMethod]
        // 18 38 138
        [DataRow(0, 1, 49)]
        [DataRow(1, 1, 18)]
        [DataRow(2, 1, 59)]
        [DataRow(3, 1, 38)]
        [DataRow(4, 1, 45)]
        [DataRow(5, 1, 7)]
        [DataRow(6, 1, 138)]
        [DataRow(7, 1, 6)]
        [DataRow(8, 1, 2)]
        public void Solve_NakedTripleInCol_SolvesExpectedCell(int row, int col,  int expected)
        {
            int[,] sudokuBoard =
            {
                { 6, 149, 14, 8, 19, 2, 7, 3, 5 },
                { 7, 18, 2, 3, 5, 6, 9, 4, 18 },
                { 3, 1589, 158, 4, 19,7, 18, 6, 2, },
                { 1, 38, 368, 9, 7, 5, 68, 2, 4 },
                { 2, 45, 456, 1, 8, 3, 56, 7, 9 },
                { 58, 7, 9, 6, 2, 4, 158, 18, 3 },
                { 4, 138, 138, 5, 6, 19, 2, 189, 7 },
                { 58, 6, 7, 2, 4, 19, 3, 1589, 18 },
                { 9, 2, 15, 7, 3, 8, 4, 15, 6 }
            };

            _nakedTriplesStrategy.Solve(sudokuBoard);

            Assert.AreEqual(expected, sudokuBoard[row, col]);
        }

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

            _nakedTriplesStrategy.Solve(sudokuBoard);

            Assert.AreEqual(expected, sudokuBoard[row, col]);
        }

        [TestMethod]
        [DataRow(0, 2, 2, 9)]
        [DataRow(0, 2, 2, 9)]
        [DataRow(0, 2, 2, 9)]
        [DataRow(2, 0, 6, 3)]
        [DataRow(4, 3, 4, 7)]
        public void EliminateNakedTripleFromOthersInBlock_GivenNakedTriple_ChangesExpectedCells(int blockIndex, int checkRow, int checkCol, int expectedVal)
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

            _nakedTriplesStrategy.SolveNakedTripleOnBlock(sudokuBoard, blockIndex);
            Assert.AreEqual(expectedVal, sudokuBoard[checkRow, checkCol]);
        }

        [TestMethod]
        [DataRow(0, 4, 6)]
        [DataRow(0, 3, 79)]
        [DataRow(6, 0, 34)]
        public void EliminateNakedTripleFromOthersInCol_GivenTriple_ChangesState(int col, int changeRow, int expected)
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

            _nakedTriplesStrategy.SolveNakedTripleOnCol(sudokuBoard, col);
            Assert.AreEqual(expected, sudokuBoard[changeRow, col]);
        }

        [TestMethod]
        [DataRow(0, 4, 7)]
        [DataRow(0, 2, 89)]
        public void ELiminateNakedTripleFromOthersInRow_GivenTriple_ChangesState(int row, int changedCol, int expected)
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
            _nakedTriplesStrategy.SolveNakedTripleOnRow(sudokuBoard, row);
            Assert.AreEqual(expected, sudokuBoard[row, changedCol]);
        }

        [TestMethod]
        [DataRow("345", 0, 4, 7)]
        [DataRow("123", 8, 8, 4)]
        public void ELiminateNakedTriple_ProperValues_ReturnsExpectedCells(string valuesToEliminate, int row, int col, int expected)
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
            _nakedTriplesStrategy.ELiminateNakedTriple(sudokuBoard, valuesToEliminate, row, col);
            Assert.IsTrue(sudokuBoard[row, col] == expected);

        }

        [TestMethod]
        // First Block
        [DataRow(0, 15, 18, 58)]
        [DataRow(0, 15, 18, 58)]
        [DataRow(0, 15, 18, 58)]
        // Fifth Block
        [DataRow(4, 15, 18, 58)]
        // Seventh Block
        [DataRow(6, 58, 15, 18)]
        // Nineth Block
        [DataRow(8, 15, 18, 58)]
        public void HasNakedTripleInBlock_GivenTriple_ReturnsFirstFoundTriple(int blockIndex, int expectedFirst, int expectedSecond, int expectedThird)
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

            var Triple = _nakedTriplesStrategy.HasNakedTripleOnBlock(sudokuBoard, blockIndex);
            Assert.IsTrue(Triple.IsNakedTriple);
            Assert.AreEqual(expectedFirst, Triple.First);
            Assert.AreEqual(expectedSecond, Triple.Second);
            Assert.AreEqual(expectedThird, Triple.Third); 

        }

        [TestMethod]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(5)]
        [DataRow(7)]
        public void HasNakedTripleInBlock_EmptyBlock_ReturnsNonTripleTuple(int blockIndex, int expectedTripleValue = -1)
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

            var Triple = _nakedTriplesStrategy.HasNakedTripleOnBlock(sudokuBoard, blockIndex);
            Assert.IsFalse(Triple.IsNakedTriple);
            Assert.AreEqual((expectedTripleValue, expectedTripleValue, expectedTripleValue, false), Triple);

        }

        [TestMethod]
        [DataRow(2)]
        [DataRow(2)]
        [DataRow(2)]
        public void HasNakedTripleInBlock_TripleDeosntExist_ReturnsNonTripleTuple(int blockIndex, int expectedTripleValue = -1)
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

            var Triple = _nakedTriplesStrategy.HasNakedTripleOnBlock(sudokuBoard, blockIndex);
            Assert.IsFalse(Triple.IsNakedTriple);
            Assert.AreEqual((expectedTripleValue, expectedTripleValue, expectedTripleValue, false), Triple);

        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(6)]
        public void HasNakedTripleInCol_GivenTriple_ReturnsTrue(int col)
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
            Assert.IsTrue(_nakedTriplesStrategy.HasNakedTripleOnCol(sudokuBoard, col).IsNakedTriple);
        }

        [TestMethod]
        [DataRow(8)]
        [DataRow(1)]
        public void HasNakedTripleInCol_TripleDoesntExist_ReturnsFalse(int col)
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
            Assert.IsFalse(_nakedTriplesStrategy.HasNakedTripleOnCol(sudokuBoard, col).IsNakedTriple);
        }

        [TestMethod]
        [DataRow(3)]
        [DataRow(8)]
        public void HasNakedTripleInCol_EmptyCol_ReturnsFalse(int col)
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
            Assert.IsFalse(_nakedTriplesStrategy.HasNakedTripleOnCol(sudokuBoard, col).IsNakedTriple);
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(0)]
        [DataRow(0)]
        [DataRow(8)]
        public void HasNakedTripleInRow_NakedTripleExists_ReturnsTrue(int row)
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
            Assert.IsTrue(_nakedTriplesStrategy.HasNakedTripleOnRow(sudokuBoard, row).IsNakedTriple);
        }

        [TestMethod]
        [DataRow(2)]
        [DataRow(5)]
        public void HasNakedTripleInRow_EmptyRow_ReturnsFalse(int row)
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
            Assert.IsFalse(_nakedTriplesStrategy.HasNakedTripleOnRow(sudokuBoard, row).IsNakedTriple);
        }

        [TestMethod]
        [DataRow(2)]
        [DataRow(4)]
        public void HasNakedTripleInRow_NakedTripleDoesntExist_ReturnsFalse(int row)
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
            Assert.IsFalse(_nakedTriplesStrategy.HasNakedTripleOnRow(sudokuBoard, row).IsNakedTriple);
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
        public void IsNakedTriple_ProperNakedTriple_ReturnsTrue(int first, int second, int third)
        {
            Assert.IsTrue(_nakedTriplesStrategy.IsNakedTriple(first, second, third));
        }

        [TestMethod]
        [DataRow(18, 15, 57)]
        [DataRow(15, 21, 23)]
        [DataRow(35, 46, 34)]
        [DataRow(157, 157, 1)]
        [DataRow(178, 178, 1)]
        public void IsNakedTriple_NonNakedTriple_ReturnsFalse(int first, int second, int third)
        {
            Assert.IsFalse(_nakedTriplesStrategy.IsNakedTriple(first, second, third));

        }

    }
}
