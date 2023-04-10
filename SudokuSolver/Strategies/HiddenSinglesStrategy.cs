using SudokuSolver.Strategies;
using SudokuSolver.Workers;

namespace SudokuSolver.Test.Unit.Strategies
{
    internal class HiddenSinglesStrategy : ISudokuStrategy
    {
        private readonly SudokuMapper _sudokuMapper;
        public HiddenSinglesStrategy(SudokuMapper sudokuMapper)
        {
            _sudokuMapper = sudokuMapper;
        }

        public int[,] Solve(int[,] sudokuBoard)
        {
            for (int row = 0; row < sudokuBoard.GetLength(0); row++)
            {
                for (int col = 0; col < sudokuBoard.GetLength(1); col++)
                {
                    CleanHiddenSingle(sudokuBoard, row, col);
                }
            }
            return sudokuBoard;
        }

        /// <summary>
        /// For a given cell checks all candidates if a candidate is a hidden single then removes all other candidates from that cell.
        /// </summary>
        /// <param name="sudokuBoard">The represantation of the sudoku board.</param>
        /// <param name="givenRow"></param>
        /// <param name="givenCol"></param>
        public void CleanHiddenSingle(int[,] sudokuBoard, int givenRow, int givenCol)
        {
            var givenCellMap = _sudokuMapper.Find(givenRow, givenCol);
            
            var cell = sudokuBoard[givenRow, givenCol];
            var candidates = cell.ToString().ToCharArray();

            foreach (var candidate in candidates)
            {
                if (IsHiddenSingleForRow(sudokuBoard, givenRow, givenCol, candidate) ||
                    IsHiddenSingleForCol(sudokuBoard, givenRow, givenCol, candidate) || 
                    IsHiddenSingleForBlock(sudokuBoard, givenRow, givenCol, candidate))
                {
                    sudokuBoard[givenRow, givenCol] = Convert.ToInt32(candidate.ToString());
                    break;
                }
            }
                
            
        }

        /// <summary>
        /// Checks the other cells in the  given block except the cell with the given row and column for encounters of the candidate.
        /// </summary>
        /// <param name="candidate">The possible hidden single.</param>
        /// <returns>True if candidate is the only encounter in the block, false otherwise.</returns>
        public bool IsHiddenSingleForBlock(int[,] sudokuBoard, int givenRow, int givenCol, char candidate)
        {
            // Already a solved cell
            if (sudokuBoard[givenRow, givenCol].ToString().Equals(candidate.ToString())) return false;

            var givenCellMap = _sudokuMapper.Find(givenRow, givenCol);

            for (int row = givenCellMap.StartRow; row < givenCellMap.StartRow + 3; row++)
            {
                for (int col = givenCellMap.StartCol; col < givenCellMap.StartCol + 3; col++)
                {
                    if (row == givenRow && col == givenCol) continue;
                    var cell = sudokuBoard[row, col];
                    var possibilities = cell.ToString().ToCharArray();
                    if (possibilities.Any(p => p == candidate)) return false;
                }
            }

            return true;
        }


        /// <summary>
        /// Checks the other cells in the given given column except the cell with the given row for encounters of the candidate.
        /// </summary>
        /// <param name="candidate">The possible hidden single.</param>
        /// <returns>True if candidate is the only encounter in the column, false otherwise.</returns>
        public bool IsHiddenSingleForCol(int[,] sudokuBoard, int givenRow, int givenCol, char candidate)
        {
            // Already a solved cell
            if (sudokuBoard[givenRow, givenCol].ToString().Equals(candidate.ToString())) return false;

            for (int row = 0; row < sudokuBoard.GetLength(1); row++)
            {
                if (row == givenRow) continue;
                var cell = sudokuBoard[row, givenCol];
                var possibilities = cell.ToString().ToCharArray();
                if (possibilities.Any(p => p == candidate)) return false;
            }

            return true;
        }


        /// <summary>
        /// Checks the other cells in the given given row except the cell with the given column for encounters of the candidate.
        /// </summary>
        /// <param name="candidate">The possible hidden single.</param>
        /// <returns>True if candidate is the only encounter in the row, false otherwise.</returns>
        internal bool IsHiddenSingleForRow(int[,] sudokuBoard, int givenRow, int givenCol, char candidate)
        {
            // Empty Cell
            if (sudokuBoard[givenRow, givenCol].ToString().Length == 1) return false;

            for (int col = 0; col < sudokuBoard.GetLength(1); col++)
            {
                if (col == givenCol) continue;
                var cell = sudokuBoard[givenRow, col];
                var possibilities = cell.ToString().ToCharArray();
                if (possibilities.Any(p => p == candidate)) return false;

            }

            return true;
        }
    }
}