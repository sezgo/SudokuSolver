using SudokuSolver.Strategies;
using SudokuSolver.Workers;

namespace SudokuSolver.Test.Unit.Strategies
{
    internal class HiddenSingleStrategy : ISudokuStrategy
    {
        private readonly SudokuMapper _sudokuMapper;
        public HiddenSingleStrategy(SudokuMapper sudokuMapper)
        {
            _sudokuMapper = sudokuMapper;
        }

        public int[,] Solve(int[,] sudokuBoard)
        {
            for (int row = 0; row < sudokuBoard.GetLength(0); row++)
            {
                for (int col = 0; col < sudokuBoard.GetLength(1); col++)
                {
                    CleanHiddenSingleInRow(sudokuBoard, row);
                    CleanHiddenSingleInCol(sudokuBoard, col);
                    CleanHiddenSingleInBlock(sudokuBoard, row, col);
                }
            }
            return sudokuBoard;
        }

        /// <summary>
        /// Cleans hidden single in the given block.
        /// </summary>
        /// <param name="sudokuBoard">The represantation of the sudoku board.</param>
        /// <param name="givenRow"></param>
        /// <param name="givenCol"></param>
        public void CleanHiddenSingleInBlock(int[,] sudokuBoard, int givenRow, int givenCol)
        {
            var hiddenSingle = HasHiddenSingleInBlock(sudokuBoard, givenRow, givenCol);

            if (hiddenSingle.Single != -1)
                sudokuBoard[givenRow, givenCol] = hiddenSingle.Single;

        }

        /// <summary>
        /// Checks if the given block has any hidden singles.
        /// </summary>
        /// <param name="sudokuBoard">The represantation of the sudoku board.</param>
        /// <returns>If no hidden single found in the block returns (-1, -1, -1), otherwise returns hidden single and its row and column.</returns>
        public (int Single, int Row, int Col) HasHiddenSingleInBlock(int[,] sudokuBoard, int givenRow, int givenCol)
        {
            var givenCellMap = _sudokuMapper.Find(givenRow, givenCol);
            for (int row = givenCellMap.StartRow; row < givenCellMap.StartRow + 3; row++)
            {
                for (int col = givenCellMap.StartCol; col < givenCellMap.StartCol + 3; col++)
                {
                    var cell = sudokuBoard[row, col];
                    var candidates = cell.ToString().ToCharArray();

                    foreach (var candidate in candidates)
                    {
                        if (IsHiddenSingleInBlock(sudokuBoard, row, col, candidate))
                        {
                            return (Single: Convert.ToInt16(candidate.ToString()), Row: row, Col: col);
                        }
                    }
                }
            }

            return (-1, -1, -1);
        }

        /// <summary>
        /// Checks the other cells in the  given block except the cell with the given row and column for encounters of the candidate.
        /// </summary>
        /// <param name="candidate">The possible hidden single.</param>
        /// <returns>True if candidate is the only encounter in the block, false otherwise.</returns>
        public bool IsHiddenSingleInBlock(int[,] sudokuBoard, int givenRow, int givenCol, char candidate)
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
        /// Cleans hidden single in the given column.
        /// </summary>
        /// <param name="sudokuBoard">The represantation of the sudoku board.</param>
        /// <param name="givenCol"></param>
        public void CleanHiddenSingleInCol(int[,] sudokuBoard, int givenCol)
        {
            var hiddenSingle = HasHiddenSingleInCol(sudokuBoard, givenCol);

            if (hiddenSingle.Single != -1)
                sudokuBoard[hiddenSingle.Row, givenCol] = hiddenSingle.Single;
        }

        /// <summary>
        /// Checks if the given column has any hidden singles.
        /// </summary>
        /// <param name="sudokuBoard">The represantation of the sudoku board.</param>
        /// <returns>If no hidden single found in the column returns (-1, -1, -1), otherwise returns hidden single and its row and column.</returns>
        public (int Single, int Row, int Col) HasHiddenSingleInCol(int[,] sudokuBoard, int givenCol)
        {
            for (int row = 0; row < sudokuBoard.GetLength(1); row++)
            {
                var cell = sudokuBoard[row, givenCol];
                var candidates = cell.ToString().ToCharArray();
                foreach (var candidate in candidates)
                {
                    if (IsHiddenSingleInCol(sudokuBoard, row, givenCol, candidate))
                    {
                        return (Single: Convert.ToInt16(candidate.ToString()), Row: row, Col: givenCol);
                    }
                }
            }

            return (-1, -1, -1);
        }

        /// <summary>
        /// Checks the other cells in the given given column except the cell with the given row for encounters of the candidate.
        /// </summary>
        /// <param name="candidate">The possible hidden single.</param>
        /// <returns>True if candidate is the only encounter in the column, false otherwise.</returns>
        public bool IsHiddenSingleInCol(int[,] sudokuBoard, int givenRow, int givenCol, char candidate)
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
        /// Cleans hidden single in the given row.
        /// </summary>
        /// <param name="sudokuBoard">The represantation of the sudoku board.</param>
        /// <param name="givenRow"></param>
        internal void CleanHiddenSingleInRow(int[,] sudokuBoard, int givenRow)
        {
            var hiddenSingle = HasHiddenSingleInRow(sudokuBoard, givenRow);

            if (hiddenSingle.Single != -1) 
                sudokuBoard[givenRow, hiddenSingle.Col] = hiddenSingle.Single;
            
            
        }

        /// <summary>
        /// Checks if the given row has any hidden singles.
        /// </summary>
        /// <param name="sudokuBoard">The represantation of the sudoku board.</param>
        /// <returns>If no hidden single found in the row returns (-1, -1, -1), otherwise returns hidden single and its row and column.</returns>
        public (int Single, int Row, int Col) HasHiddenSingleInRow(int[,] sudokuBoard, int givenRow)
        {
            for (int col = 0; col < sudokuBoard.GetLength(1); col++)
            {
                var cell = sudokuBoard[givenRow, col];
                var possibilities = cell.ToString().ToCharArray();
                foreach (var possibility in possibilities)
                {
                    if (IsHiddenSingleInRow(sudokuBoard, givenRow, col, possibility))
                    {
                        return (Single: Convert.ToInt16(possibility.ToString()),  Row: givenRow, Col: col);
                    }
                }
            }

            return (-1, -1, -1);
        }

        /// <summary>
        /// Checks the other cells in the given given row except the cell with the given column for encounters of the candidate.
        /// </summary>
        /// <param name="candidate">The possible hidden single.</param>
        /// <returns>True if candidate is the only encounter in the row, false otherwise.</returns>
        internal bool IsHiddenSingleInRow(int[,] sudokuBoard, int givenRow, int givenCol, char candidate)
        {
            // Already a solved cell
            if (sudokuBoard[givenRow, givenCol].ToString().Equals(candidate.ToString())) return false;

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