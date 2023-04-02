using SudokuSolver.Workers;

namespace SudokuSolver.Test.Unit.Strategies.Tests
{
    [TestClass]
    public class HiddenSingleStrategyBlockTest
    {
        private readonly HiddenSingleStrategy _hiddenSingleStrategy = new(new SudokuMapper());
        private readonly SudokuBoardStateManager _sudokuBoardStateManager = new();
        internal int[,] sudokuBoard =
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

        
        [TestMethod]
        public void IsHiddenSingle_HiddenSingle_ReturnsTrue()
        {
            var result = _hiddenSingleStrategy.IsHiddenSingleForCol(sudokuBoard, 4, 2, '1');
            Assert.IsTrue(result);


            result = _hiddenSingleStrategy.IsHiddenSingleForCol(sudokuBoard, 0, 2, '3');
            Assert.IsTrue(result);

        }
        [TestMethod]
        public void IsHiddenSingleInCol_NonHiddenSingle_ReturnsFalse()
        {
            var result = _hiddenSingleStrategy.IsHiddenSingleForCol(sudokuBoard, 0, 2, '2');
            Assert.IsFalse(result);

            result = _hiddenSingleStrategy.IsHiddenSingleForCol(sudokuBoard, 7, 2, '8');
            Assert.IsFalse(result);
        }
        [TestMethod]
        public void IsHiddenSingleInCol_SolvedCell_ReturnsFalse()
        {

            var result = _hiddenSingleStrategy.IsHiddenSingleForCol(sudokuBoard, 3, 2, '9');
            Assert.IsFalse(result);
        }
    }
}
