using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    public class HiddenTripleStrategyTest
    {
        private readonly HiddenTriplesStrategy _strategy = new HiddenTriplesStrategy();

        List<int[,]> exampleBoards = new List<int[,]>();
        List<int[,]> solvedBoards = new List<int[,]>();

        // https://www.sudokuoftheday.com/techniques/hidden-pairs-triples
        [TestMethod]
        public void SolveTestColExample1()
        {
            SudokuBoardStateManager manager = new SudokuBoardStateManager();
            int[,] sudokuBoard =
            {
                { 5, 2, 8, 6, 137, 137, 17, 4, 9 },
                { 1, 3, 6, 4, 9, 78, 78, 2, 5 },
                { 7, 9, 4, 2, 18, 5, 6, 3, 18 },
                { 3469, 568, 35, 1, 347, 37, 2, 59, 478 },
                { 49, 15, 7, 8, 2, 6, 3, 59, 14 },
                { 34, 18, 2, 5, 347, 9, 18, 6, 47 },
                { 2, 4, 15, 3, 158, 18, 9, 7, 6 },
                { 8, 56, 9, 7, 56, 2, 4, 1, 3 },
                { 36, 7, 13, 9, 16, 4, 5, 8, 2 }
            };
            int[,] solvedSudokuBoard =
            {
                { 5, 2, 8, 6, 37, 137, 17, 4, 9 },
                { 1, 3, 6, 4, 9, 78, 78, 2, 5 },
                { 7, 9, 4, 2, 18, 5, 6, 3, 18 },
                { 3469, 568, 35, 1, 347, 37, 2, 59, 478 },
                { 49, 15, 7, 8, 2, 6, 3, 59, 14 },
                { 34, 18, 2, 5, 347, 9, 18, 6, 47 },
                { 2, 4, 15, 3, 158, 18, 9, 7, 6 },
                { 8, 56, 9, 7, 56, 2, 4, 1, 3 },
                { 36, 7, 13, 9, 16, 4, 5, 8, 2 }
            };

           
            var expectedSolvedState = manager.GenerateState(solvedSudokuBoard);

            _strategy.Solve(sudokuBoard);

            var solvedState = manager.GenerateState(sudokuBoard);
            Assert.AreEqual(expectedSolvedState, solvedState);
        }
        // https://www.sudokuoftheday.com/techniques/hidden-pairs-triples
        [TestMethod]
        public void SolveTestColExample2()
        {
            // 569 on first row should not be a hidden pair cuz 9 exists outside of the three cells.
            int[,] sudokuBoard =
            {
                { 3, 7, 256, 4, 256, 8, 1, 2569, 29 },
                { 126, 568, 12568, 9, 256, 3, 7, 256, 4 },
                { 9, 4, 256, 1, 2567, 2567, 56, 8, 3 },
                { 4, 2, 17, 378, 136789, 167, 38, 179, 5 },
                { 167, 369, 1367, 5, 28, 4, 28, 179, 79 },
                { 8, 59, 157, 37, 12379, 127, 23, 4, 6 },
                { 27, 1, 2378, 378, 4, 9, 56, 56, 78 },
                { 5, 38, 9, 6, 1378, 17, 4, 27, 278 },
                { 67, 68, 4, 2, 578, 57, 9, 3, 1 }
            };
            // 139, 139, 13
            // 3,4 - 5,4 - 7-4
            int[,] solvedSudokuBoard =
            {
                { 3, 7, 256, 4, 256, 8, 1, 2569, 29 },
                { 126, 568, 12568, 9, 256, 3, 7, 256, 4 },
                { 9, 4, 256, 1, 2567, 2567, 56, 8, 3 },
                { 4, 2, 17, 378, 139, 167, 38, 179, 5 },
                { 167, 369, 1367, 5, 28, 4, 28, 179, 79 },
                { 8, 59, 157, 37, 139, 127, 23, 4, 6 },
                { 27, 1, 2378, 378, 4, 9, 56, 56, 78 },
                { 5, 38, 9, 6, 13, 17, 4, 27, 278 },
                { 67, 68, 4, 2, 578, 57, 9, 3, 1 }
            };

            _strategy.Solve(sudokuBoard);

            CollectionAssert.AreEqual(solvedSudokuBoard, sudokuBoard);
        }
        // https://www.sudokuoftheday.com/techniques/hidden-pairs-triples
        [TestMethod]
        public void SolveTestRowExample1()
        {
            int[,] sudokuBoard =
            {
                { 9, 58, 3, 4, 12, 12, 6, 7, 58 },
                { 7, 1, 2, 6, 5, 8, 9, 4, 3 },
                { 58, 6, 4, 7, 3, 9, 18, 15, 2 },
                { 26, 3, 18, 18, 4, 7, 5, 26, 9 },
                { 12568, 4578, 578, 58, 9, 1256, 3, 126, 47 },
                { 56, 47, 9, 3, 12, 56, 12, 8, 47 },
                { 4, 257, 157, 15, 6, 3, 28, 9, 58 },
                { 158, 9, 58, 2, 7, 15, 4, 3, 6 },
                { 3, 25, 6, 9, 8, 4, 7, 25, 1 }
            };

            // 126, 126, 126
            // 4,0 - 4,5 - 4,7
            int[,] solvedSudokuBoard =
            {
                { 9, 58, 3, 4, 12, 12, 6, 7, 58 },
                { 7, 1, 2, 6, 5, 8, 9, 4, 3 },
                { 58, 6, 4, 7, 3, 9, 18, 15, 2 },
                { 26, 3, 18, 18, 4, 7, 5, 26, 9 },
                { 126, 4578, 578, 58, 9, 126, 3, 126, 47 },
                { 56, 47, 9, 3, 12, 56, 12, 8, 47 },
                { 4, 257, 157, 15, 6, 3, 28, 9, 58 },
                { 158, 9, 58, 2, 7, 15, 4, 3, 6 },
                { 3, 25, 6, 9, 8, 4, 7, 25, 1 }
            };
            _strategy.Solve(sudokuBoard);

            CollectionAssert.AreEqual(solvedSudokuBoard, sudokuBoard);
        }
        // https://sudoku.com/sudoku-rules/hidden-triples/
        [TestMethod]
        public void SolveTestBlockExampe1()
        {
            int[,] sudokuBoard =
            {
                { 1356, 3569, 8, 23469, 1246, 7, 123569, 1234569, 23459 },
                { 1367, 4, 2, 3689, 168, 5, 13679, 13689, 3789 },
                { 13567, 35679, 15679, 234689, 12468, 23489, 1235679, 12345689, 2345789 },
                { 2457, 257, 3, 24579, 2457, 6, 8, 2459, 1 },
                { 124578, 257, 1457, 2345789, 124578, 123489, 23579, 23459, 6 },
                { 9, 256, 1456, 234578, 1245678, 12348, 2357, 2345, 23457 },
                { 56, 8, 569, 1, 3, 2, 4, 7, 59 },
                { 234567, 23567, 4567, 45678, 9, 48, 12356, 123568, 2358 },
                { 234567, 1, 45679, 45678, 45678, 48, 23569, 235689, 23589 }
            };

            int[,] solvedSudokuBoard =
            {
                { 1356, 3569, 8, 23469, 1246, 7, 123569, 1234569, 23459 },
                { 1367, 4, 2, 3689, 168, 5, 13679, 13689, 3789 },
                { 13567, 35679, 15679, 234689, 12468, 23489, 1235679, 12345689, 2345789 },
                { 2457, 257, 3, 24579, 2457, 6, 8, 2459, 1 },
                { 124578, 257, 1457, 2345789, 124578, 123489, 23579, 23459, 6 },
                { 9, 256, 1456, 234578, 1245678, 12348, 2357, 2345, 23457 },
                { 56, 8, 569, 1, 3, 2, 4, 7, 59 },
                { 234567, 23567, 4567, 567, 9, 48, 12356, 123568, 2358 },
                { 234567, 1, 45679, 567, 567, 48, 23569, 235689, 23589 }
            };

            _strategy.Solve(sudokuBoard);

            CollectionAssert.AreEqual(solvedSudokuBoard, sudokuBoard);
        }

    }
}
