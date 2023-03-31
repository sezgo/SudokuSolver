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
                CleanHiddenSingleInRow(sudokuBoard, row);
            }

            for (int col = 0; col < sudokuBoard.GetLength(0); col++)
            {
                //CleanHiddenSingleInCol(sudokuBoard, col);
            }

            for (int row = 0; row < sudokuBoard.GetLength(0); row++)
            {
                for (int col = 0; col < sudokuBoard.GetLength(1); col++)
                {  

                }
            }
            return sudokuBoard;
        }

        /*
         * 
         * Traverse the whole row
         *      For each note digit check all cells in the row
         *      If no other cell has that digit 
         *          Remove all other digits in the cell except the hidden single
         *      If no hidden single found return -1
         * 
         * */

        internal void CleanHiddenSingleInRow(int[,] sudokuBoard, int givenRow)
        {
            var hiddenSingle = HasHiddenSingleInRow(sudokuBoard, givenRow);

            if (hiddenSingle.Single != -1) 
                sudokuBoard[givenRow, hiddenSingle.Col] = hiddenSingle.Single;
            
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sudokuBoard">The represantation of the sudoku board.</param>
        /// <param name="givenRow"></param>
        /// <param name="givenCol"></param>
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
        /// Checks it
        /// </summary>
        /// <param name="possibility"></param>
        /// <returns></returns>
        internal bool IsHiddenSingleInRow(int[,] sudokuBoard, int givenRow, int givenCol, char possibility)
        {

            for (int col = 0; col < sudokuBoard.GetLength(1); col++)
            {
                if (col == givenCol) continue;
                var cell = sudokuBoard[givenRow, col];
                var possibilities = cell.ToString().ToCharArray();
                if (possibilities.Any(p => p == possibility) || possibilities.Length < 2) return false;

            }

            return true;
        }
    }
}