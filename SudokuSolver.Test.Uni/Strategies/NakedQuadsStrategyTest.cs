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
    public class NakedQuadsStrategyTest
    {
        private readonly NakedQuadsStrategy _nakedQuadsStrategy = new(new SudokuMapper());

        [TestMethod]
        // 25, 35, 345, 2345
        public void Solve_NakedQuadOnRow_ReturnsExpectedSudoukoBoardState()
        {
            int[,] sudokuBoard =
            {
                { 1, 79, 357, 2, 47, 379, 6, 45, 8 },
                { 25, 269, 35,  134689, 4689, 13689, 345, 7, 2345 },
                { 4, 267, 8, 367, 67, 5, 1, 9, 23 },
                { 9, 3, 46, 14678, 5, 1678, 2, 1468, 14 },
                { 7, 5, 246, 14689, 4689, 12689, 3489, 1468, 134 },
                { 8, 1, 246, 469, 3, 269, 459, 456, 7 },
                { 25, 24, 1, 678, 678, 678, 45, 3, 9 },
                { 6, 478, 57, 39, 2, 39, 4578, 1458, 145 },
                { 3, 78, 9, 5, 1, 4, 78, 2, 6 }
            };

            int[,] expectedSudokuBoard =
            {
                { 1, 79, 357, 2, 47, 379, 6, 45, 8 },
                { 25, 69, 35,  1689, 689, 1689, 345, 7, 2345 },
                { 4, 267, 8, 367, 67, 5, 1, 9, 23 },
                { 9, 3, 46, 14678, 5, 1678, 2, 1468, 14 },
                { 7, 5, 246, 14689, 4689, 12689, 3489, 1468, 134 },
                { 8, 1, 246, 469, 3, 269, 459, 456, 7 },
                { 25, 24, 1, 678, 678, 678, 45, 3, 9 },
                { 6, 478, 57, 39, 2, 39, 4578, 1458, 145 },
                { 3, 78, 9, 5, 1, 4, 78, 2, 6 }
            };


            SudokuBoardStateManager stateManager = new SudokuBoardStateManager();

            _nakedQuadsStrategy.Solve(sudokuBoard);
            var solvedState = stateManager.GenerateState(sudokuBoard);
            var expectedState = stateManager.GenerateState(expectedSudokuBoard);

            Assert.AreEqual(expectedState, solvedState);
        }

        [TestMethod]
        // 69, 2679, 2679, 2679
        [DataRow(0, 0, 1)]
        [DataRow(0, 1, 69)]
        [DataRow(0, 2, 2679)]
        [DataRow(1, 0, 58)]
        [DataRow(1, 1, 458)]
        [DataRow(1, 2, 2679)]
        [DataRow(2, 0, 358)]
        [DataRow(2, 1, 3458)]
        [DataRow(2, 2, 2679)]
        public void Solve_NakedQuadOnBlock_SolvesExpectedCells(int row, int col, int expected)
        {
            int[,] sudokuBoard =
            {
                { 1, 69, 2679, 4, 679, 27, 8, 5, 3 },
                { 25689, 45689, 2679, 25789, 56789, 3, 2679, 1, 467 },
                { 235689, 345689, 2679, 1, 56789, 2578, 2679, 29, 467 },
                { 7, 36, 1, 28, 4, 9, 5, 23, 68 },
                { 3569, 2, 4, 578, 3578, 1, 679, 39, 678 },
                { 359, 359, 8, 6, 357, 257, 279, 4, 1 },
                { 2689, 689, 269, 89, 1, 4, 3, 7, 5 },
                { 89, 1, 3, 5789, 5789, 578, 4, 6, 2 },
                { 4, 7, 5, 3, 2, 6, 1, 8, 9 }
            };

            _nakedQuadsStrategy.Solve(sudokuBoard);

            Assert.AreEqual(expected, sudokuBoard[row, col]);
        }

        [TestMethod]
        // 26, 56, 245, 246
        [DataRow(0, 4, 26)]
        [DataRow(1, 4, 39)]
        [DataRow(2, 4, 13)]
        [DataRow(3, 4, 56)]
        [DataRow(4, 4, 245)]
        [DataRow(5, 4, 7)]
        [DataRow(6, 4, 189)]
        [DataRow(7, 4, 189)]
        [DataRow(8, 4, 246)]
        public void Solve_NakedQuadOnCol_SolvesExpectedCells(int row, int col, int expected)
        {
            int[,] sudokuBoard =
            {
                { 359, 35, 39, 26, 26, 4, 8, 1, 7 },
                { 6, 17, 17, 59, 359, 8, 35, 2, 4 },
                { 4, 2, 8, 7, 135, 15, 35, 9, 96 },
                { 8, 567, 67, 1, 56, 3, 2, 4, 9 },
                { 125, 9, 14, 8, 245, 25, 7, 6, 3 },
                { 23 ,346, 346, 2469, 7, 269, 1, 5, 8 },
                { 139, 13468, 2, 469, 14689, 169, 46, 7, 5 },
                { 19, 1468, 1469, 4569, 145689, 7, 46, 3, 2 },
                { 7, 46, 5, 3, 246, 26, 9, 8, 1 }
            };

            _nakedQuadsStrategy.Solve(sudokuBoard);

            Assert.AreEqual(expected, sudokuBoard[row, col]);
        }

        [TestMethod]
        // 25, 35, 345, 2345
        [DataRow(1, 0, 25)]
        [DataRow(1, 1, 69)]
        [DataRow(1, 2, 35)]
        [DataRow(1, 3, 1689)]
        [DataRow(1, 4, 689)]
        [DataRow(1, 5, 1689)]
        [DataRow(1, 6, 345)]
        [DataRow(1, 7, 7)]
        [DataRow(1, 8, 2345)]
        public void Solve_NakedQuadOnRow_SolvesExpectedCells(int row, int col, int expected)
        {
            int[,] sudokuBoard =
            {
                { 1, 79, 357, 2, 47, 379, 6, 45, 8 },
                { 25, 269, 35,  134689, 4689, 13689, 345, 7, 2345 },
                { 4, 267, 8, 367, 67, 5, 1, 9, 23 },
                { 9, 3, 46, 14678, 5, 1678, 2, 1468, 14 },
                { 7, 5, 246, 14689, 4689, 12689, 3489, 1468, 134 },
                { 8, 1, 246, 469, 3, 269, 459, 456, 7 },
                { 25, 24, 1, 678, 678, 678, 45, 3, 9 },
                { 6, 478, 57, 39, 2, 39, 4578, 1458, 145 },
                { 3, 78, 9, 5, 1, 4, 78, 2, 6 }
            };

            _nakedQuadsStrategy.Solve(sudokuBoard);

            Assert.AreEqual(expected, sudokuBoard[row, col]);
        }

        [TestMethod]
        // 57, 135, 1357, 37
        [DataRow(4, 0, 57)]
        [DataRow(4, 1, 135)]
        [DataRow(4, 2, 1357)]
        [DataRow(4, 3, 46)]
        [DataRow(4, 4, 24)]
        [DataRow(4, 5, 8)]
        [DataRow(4, 6, 69)]
        [DataRow(4, 7, 29)]
        [DataRow(4, 8, 37)]
        public void Solve_NakedQuadOnRow2_SolvesExpectedCells(int row, int col, int expected)
        {
            int[,] sudokuBoard =
            {
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 57, 135, 1357, 456, 254, 8, 679, 239, 37 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            };

            _nakedQuadsStrategy.Solve(sudokuBoard);

            Assert.AreEqual(expected, sudokuBoard[row, col]);
        }

        [TestMethod]
        // 57, 135, 1357, 37
        [DataRow(0, 0)]
        [DataRow(0, 1)]
        [DataRow(0, 2)]
        [DataRow(0, 8)]
        public void HasNakedQuadInRow_NakedQuadleExists_ReturnsTrue(int row, int col)
        {
            int[,] sudokuBoard =
            {
                { 57, 135, 1357, 456, 254, 8, 679, 239, 37 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            };
            Assert.IsTrue(_nakedQuadsStrategy.HasNakedQuadOnRow(sudokuBoard, row).IsNakedQuad);
        }

        [TestMethod]
        [DataRow(57, 135, 1357, 37)]
        [DataRow(12, 23, 34, 1234)]
        [DataRow(25, 35, 345, 2345)]
        [DataRow(26, 56, 245, 246)]
        [DataRow(69, 2679, 2679, 2679)]
        public void IsNakedQuad_NakedQuad_ReturnsTrue(int first, int second, int third, int fourth)
        {
            Assert.IsTrue(_nakedQuadsStrategy.IsNakedQuad(first, second, third, fourth));
        }

        [TestMethod]
        [DataRow(57, 1235, 1357, 37)]
        [DataRow(12, 2356, 34, 1234)]
        [DataRow(0, 0, 0, 0)]
        public void IsNakedQuad_NonNakedQuad_ReturnsFalse(int first, int second, int third, int fourth)
        {
            Assert.IsFalse(_nakedQuadsStrategy.IsNakedQuad(first, second, third, fourth));
        }
    }
}
