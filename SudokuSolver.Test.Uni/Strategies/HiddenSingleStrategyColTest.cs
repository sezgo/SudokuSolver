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
        [DataRow(0, 2, 3)]
        public void CleanHiddenSingleInCol_HiddenSinglesExist_SolvesFirstHiddenSingle(int row, int col, int expected)
        {
            var currentState = _sudokuBoardStateManager.GenerateState(sudokuBoard);

            _hiddenSingleStrategy.CleanHiddenSingleInCol(sudokuBoard, col);
            var nextState = _sudokuBoardStateManager.GenerateState(sudokuBoard);

            Assert.AreNotEqual(nextState, currentState);
            Assert.AreEqual(expected, sudokuBoard[row, col]);
        }

        [TestMethod]
        public void CleanHiddenSingleInCol_HiddenSingleDoesntExist_DoesntChangeBoardState()
        {
            var currentState = _sudokuBoardStateManager.GenerateState(sudokuBoard);
            _hiddenSingleStrategy.CleanHiddenSingleInCol(sudokuBoard, 5);
            var nextState = _sudokuBoardStateManager.GenerateState(sudokuBoard);
            Assert.AreEqual(nextState, currentState);
        }


        [TestMethod()]
        public void HasHiddenSingleInCol_HiddenSingleExists_ReturnsFirstHiddenSingle()
        {
            var hiddenSingle = _hiddenSingleStrategy.HasHiddenSingleInCol(sudokuBoard, 2);
            Assert.IsTrue(hiddenSingle.Single != -1);
            Assert.IsTrue(hiddenSingle.Single == 3);
            Assert.IsTrue(hiddenSingle.Row == 0);
        }


        // NonHiddenSingle: (-1, -1, -1)
        [TestMethod()]
        public void HasHiddenSingleInCol_HiddenSingleDoesntExists_ReturnsNonHiddenSingle()
        {
            var hiddenSingle = _hiddenSingleStrategy.HasHiddenSingleInCol(sudokuBoard, 5);
            Assert.IsTrue(hiddenSingle.Single == -1);
        }
        [TestMethod()]
        public void HasHiddenSingleInCol_EmptyCol_ReturnsNonHiddenSingle()
        {
            var hiddenSingle = _hiddenSingleStrategy.HasHiddenSingleInCol(sudokuBoard, 1);
            Assert.IsTrue(hiddenSingle.Single == -1);
        }


        [TestMethod]
        public void IsHiddenSingle_HiddenSingle_ReturnsTrue()
        {
            var result = _hiddenSingleStrategy.IsHiddenSingleInCol(sudokuBoard, 4, 2, '1');
            Assert.IsTrue(result);


            result = _hiddenSingleStrategy.IsHiddenSingleInCol(sudokuBoard, 0, 2, '3');
            Assert.IsTrue(result);

        }
        [TestMethod]
        public void IsHiddenSingleInCol_NonHiddenSingle_ReturnsFalse()
        {
            var result = _hiddenSingleStrategy.IsHiddenSingleInCol(sudokuBoard, 0, 2, '2');
            Assert.IsFalse(result);

            result = _hiddenSingleStrategy.IsHiddenSingleInCol(sudokuBoard, 7, 2, '8');
            Assert.IsFalse(result);
        }
        [TestMethod]
        public void IsHiddenSingleInCol_SolvedCell_ReturnsFalse()
        {

            var result = _hiddenSingleStrategy.IsHiddenSingleInCol(sudokuBoard, 3, 2, '9');
            Assert.IsFalse(result);
        }
    }
}
