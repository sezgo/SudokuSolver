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
        NakedTriosStrategy _nakedTriosStrategy = new NakedTriosStrategy(new SudokuMapper());
        [TestMethod]
        public void ShouldEliminateNakedTrioFromOthersInBlock()
        {
            int[,] sudokuBoard = {
                { 15, 0, 0, 0, 0, 0, 1234, 12, 14 },
                { 18, 58, 0, 0, 0, 0, 24, 0, 0 },
                { 1589, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 25, 237, 0, 0, 0, 0 },
                { 0, 0, 0, 35, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 23, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            };

            _nakedTriosStrategy.EliminateNakedTrioFromOthersInBlock(sudokuBoard, 0, 0);
            Assert.IsTrue(sudokuBoard[2,0] == 9);

            _nakedTriosStrategy.EliminateNakedTrioFromOthersInBlock(sudokuBoard, 0, 7);
            Assert.IsTrue(sudokuBoard[0, 6] == 3);


            _nakedTriosStrategy.EliminateNakedTrioFromOthersInBlock(sudokuBoard, 3, 3);
            Assert.IsTrue(sudokuBoard[3, 4] == 7);
        }
        [TestMethod]
        public void ShouldHaveNakedTrioInBlock()
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


            // Fifth Block
            var trio = _nakedTriosStrategy.HasNakedTrioInBlock(sudokuBoard, 7, 0);
            var expected = (First: 58, Second: 15, Third: 18, IsNakedTrio: true);
            Assert.IsTrue(trio.IsNakedTrio);
            Assert.AreEqual(expected, trio);

            // Seventh Block
            trio = _nakedTriosStrategy.HasNakedTrioInBlock(sudokuBoard, 3, 3);
            expected = (First: 15, Second: 18, Third: 58, IsNakedTrio: true);
            Assert.IsTrue(trio.IsNakedTrio);
            Assert.AreEqual(expected, trio);


            // First Block:
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if((row != 0 && col != 0) &&
                        (row != 0 && col != 1) &&
                        (row != 1 && col != 0)) Assert.IsFalse(_nakedTriosStrategy.HasNakedTrioInBlock(sudokuBoard, row, col).IsNakedTrio, $"{row},{col}: {sudokuBoard[row,col]}");
                }
            }
            Assert.IsTrue(_nakedTriosStrategy.HasNakedTrioInBlock(sudokuBoard, 0, 0).IsNakedTrio);
            Assert.IsTrue(_nakedTriosStrategy.HasNakedTrioInBlock(sudokuBoard, 0, 1).IsNakedTrio);
            Assert.IsTrue(_nakedTriosStrategy.HasNakedTrioInBlock(sudokuBoard, 1, 0).IsNakedTrio);
            Assert.IsFalse(_nakedTriosStrategy.HasNakedTrioInBlock(sudokuBoard, 2, 0).IsNakedTrio);

            // Second block:
            for (int row = 0; row < 3; row++)
            {
                for (int col = 3; col < 6; col++)
                {
                     Assert.IsFalse(_nakedTriosStrategy.HasNakedTrioInBlock(sudokuBoard, row, col).IsNakedTrio, $"{row},{col}: {sudokuBoard[row, col]}");
                }
            }
        }
        [TestMethod]
        public void ShouldHaveNakedTrioInCol()
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
            Assert.IsTrue(_nakedTriosStrategy.HasNakedTrioInCol(sudokuBoard, 0, 0).IsNakedTrio);
            Assert.IsTrue(_nakedTriosStrategy.HasNakedTrioInCol(sudokuBoard, 8, 6).IsNakedTrio);
            Assert.IsFalse(_nakedTriosStrategy.HasNakedTrioInCol(sudokuBoard, 0, 8).IsNakedTrio);
            Assert.IsFalse(_nakedTriosStrategy.HasNakedTrioInCol(sudokuBoard, 0, 1).IsNakedTrio);
        }

        [TestMethod]
        public void ShouldEliminateNakedTrioFromOthersInCol()
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

            _nakedTriosStrategy.EliminateNakedTrioFromOthersInCol(sudokuBoard, 0, 0);
            Assert.IsTrue(sudokuBoard[4, 0] == 6);
            Assert.IsTrue(sudokuBoard[3, 0] == 79);
            _nakedTriosStrategy.EliminateNakedTrioFromOthersInCol(sudokuBoard, 8, 6);
            Assert.IsTrue(sudokuBoard[0, 6] == 34);
        }

        [TestMethod]
        public void ShouldHaveNakedTrioInRow()
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
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            };
            Assert.IsTrue(_nakedTriosStrategy.HasNakedTrioInRow(sudokuBoard, 0 ,0).IsNakedTrio);
            Assert.IsTrue(_nakedTriosStrategy.HasNakedTrioInRow(sudokuBoard, 0, 7).IsNakedTrio);
            Assert.IsTrue(_nakedTriosStrategy.HasNakedTrioInRow(sudokuBoard, 0, 8).IsNakedTrio);
            Assert.IsFalse(_nakedTriosStrategy.HasNakedTrioInRow(sudokuBoard, 5, 0).IsNakedTrio);
        }

        [TestMethod]
        public void ShouldELiminateNakedTrioFromOthersInRow()
        {
            int[,] sudokuBoard =
            {
                { 35, 0, 0, 0, 3457, 0, 0, 45, 34 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            };
            _nakedTriosStrategy.ELiminateNakedTrioFromOthersInRow(sudokuBoard, 0, 0);
            Assert.IsTrue(sudokuBoard[0,4]==7);
        }

        [TestMethod]
        public void ShouldELiminateNakedTrioFromGivenRowAndCol()
        {
            int[,] sudokuBoard =
            {
                { 35, 0, 0, 0, 3457, 0, 0, 45, 34 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            };
            _nakedTriosStrategy.ELiminateNakedTrio(sudokuBoard, "345", 0, 4);
            Assert.IsTrue(sudokuBoard[0,4] == 7);

        }

        [TestMethod]
        public void ShouldVeriftGivenNakedTrios()
        {
            Assert.IsTrue(_nakedTriosStrategy.IsNakedTrio(15, 18, 58));
            Assert.IsTrue(_nakedTriosStrategy.IsNakedTrio(18, 15, 58));

            Assert.IsFalse(_nakedTriosStrategy.IsNakedTrio(18, 15, 57));

            Assert.IsFalse(_nakedTriosStrategy.IsNakedTrio(18, 15, 18));

            Assert.IsFalse(_nakedTriosStrategy.IsNakedTrio(18, 15, 15));


            Assert.IsTrue(_nakedTriosStrategy.IsNakedTrio(15, 21, 25));
            Assert.IsTrue(_nakedTriosStrategy.IsNakedTrio(15, 25, 21));

            Assert.IsFalse(_nakedTriosStrategy.IsNakedTrio(15, 21, 23));


            Assert.IsTrue(_nakedTriosStrategy.IsNakedTrio(34, 45, 35));
            Assert.IsTrue(_nakedTriosStrategy.IsNakedTrio(35, 45, 34));

            Assert.IsFalse(_nakedTriosStrategy.IsNakedTrio(35, 46, 34));
        }
    }
}
