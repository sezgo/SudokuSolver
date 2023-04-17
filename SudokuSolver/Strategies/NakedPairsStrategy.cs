using SudokuSolver.Data;
using SudokuSolver.Workers;

namespace SudokuSolver.Strategies
{
    internal class NakedPairsStrategy : ISudokuStrategy
    {
        private readonly SudokuMapper _sudokuMapper;
        public NakedPairsStrategy(SudokuMapper sudokuMapper)
        {
            _sudokuMapper = sudokuMapper;
        }

        public int[,] Solve(int[,] sudokuBoard)
        {
            for (int index = 0; index < Constants.MaxGroupLength; index++)
            {
                SolveNakedPairOnRow(sudokuBoard, index);
                SolveNakedPairOnCol(sudokuBoard, index);
                SolveNakedPairOnBlock(sudokuBoard, index);
            }

            return sudokuBoard;
        }

        /// <summary>
        /// If there is a naked pair in the block, removes the naked quad digits from other non naked pair cells.
        /// </summary>
        /// <param name="sudokuBoard">The current represantation of the sudoku board.</param>
        /// <param name="givenBlockIndex">The given block index.</param>
        private void SolveNakedPairOnBlock(int[,] sudokuBoard, int givenBlockIndex)
        {
            var nakedPair = HasNakedPairOnBlock(sudokuBoard, givenBlockIndex);
            if (!nakedPair.IsNakedPair) return;

            SudokuMap blockMap = _sudokuMapper.Find(givenBlockIndex);

            for (int celIndex = 0; celIndex < Constants.MaxGroupLength; celIndex++)
            {
                var cellRow = _sudokuMapper.GetCellRow(celIndex, blockMap);
                var cellCol = _sudokuMapper.GetCellCol(celIndex, blockMap);
                var cell = sudokuBoard[cellRow, cellCol];

                if (nakedPair.First != cell)
                {
                    var strValuesToEliminate = nakedPair.First.ToString();
                    ELiminateNakedPair(sudokuBoard, strValuesToEliminate, cellRow, cellCol);
                }
            }

        }

        /// <summary>
        /// Checks if there is a naked pair in the given block.
        /// </summary>
        /// <param name="sudokuBoard">The represantation of the sudoku board.</param>
        /// <param name="givenBlockIndex">The given block index.</param>
        /// <returns>A tuple with the the two naked numbers, and a bool value: IsNakedPair, true if a proper naked pair, false otherwise.</returns>
        private (int First, int Second, bool IsNakedPair) HasNakedPairOnBlock(int[,] sudokuBoard, int givenBlockIndex)
        {
            SudokuMap blockMap = _sudokuMapper.Find(givenBlockIndex);

            for (int firstCellIndex = 0; firstCellIndex < Constants.MaxGroupLength; firstCellIndex++)
                for (int secondCellIndex = 0; secondCellIndex < Constants.MaxGroupLength; secondCellIndex++)
                {
                    if (!AreSameCells(firstCellIndex, secondCellIndex))
                    {
                        var firstRow = _sudokuMapper.GetCellRow(firstCellIndex, blockMap);
                        var firstCol = _sudokuMapper.GetCellCol(firstCellIndex, blockMap);

                        var secondRow = _sudokuMapper.GetCellRow(secondCellIndex, blockMap);
                        var secondCol = _sudokuMapper.GetCellCol(secondCellIndex, blockMap);

                        if (IsNakedPair(sudokuBoard[firstRow, firstCol], sudokuBoard[secondRow, secondCol]))
                        {
                            return (
                                First: sudokuBoard[firstRow, firstCol],
                                Second: sudokuBoard[secondRow, secondCol],
                                IsNakedPair: true
                            );
                        }
                    }

                }

            return (-1, -1, false);
        }

        /// <summary>
        /// If there is a naked pair in the column, removes the naked quad digits from other non naked pair cells.
        /// </summary>
        /// <param name="sudokuBoard">The current represantation of the sudoku board.</param>
        /// <param name="givenCol">The given column.</param>
        private void SolveNakedPairOnCol(int[,] sudokuBoard, int givenCol)
        {
            var nakedPair = HasNakedPairOnCol(sudokuBoard, givenCol);
            if (!nakedPair.IsNakedPair) return;

            for (int row = 0; row < Constants.MaxGroupLength; row++)
            {
                var cell = sudokuBoard[row, givenCol];
                if (nakedPair.First != cell)
                {
                    var strValuesToEliminate = nakedPair.First.ToString();
                    ELiminateNakedPair(sudokuBoard, strValuesToEliminate, row, givenCol);
                }
            }

        }

        /// <summary>
        /// Checks if there is a naked pair in the given column.
        /// </summary>
        /// <param name="sudokuBoard">The represantation of the sudoku board.</param>
        /// <param name="givenCol">The given column.</param>
        /// <returns>A tuple with the the two naked numbers, and a bool value: IsNakedPair, true if a proper naked pair, false otherwise.</returns>
        private (int First, int Second, bool IsNakedPair) HasNakedPairOnCol(int[,] sudokuBoard, int givenCol)
        {
            for (int row1 = 0; row1 < Constants.MaxGroupLength; row1++)
                for (int row2 = 0; row2 < Constants.MaxGroupLength; row2++)
                {
                    if (!AreSameCells(row1, row2) && IsNakedPair(sudokuBoard[row1, givenCol], sudokuBoard[row2, givenCol]))
                    {
                        return (
                            First: sudokuBoard[row1, givenCol],
                            Second: sudokuBoard[row2, givenCol],
                            IsNakedPair: true
                        );
                    }
                }
            return (-1, -1, false);
        }


        /// <summary>
        /// If there is a naked pair in the row, removes the naked quad digits from other non naked pair cells.
        /// </summary>
        /// <param name="sudokuBoard">The current represantation of the sudoku board.</param>
        /// <param name="givenRow">The given row.</param>
        private void SolveNakedPairOnRow(int[,] sudokuBoard, int givenRow)
        {
            var nakedPair = HasNakedPairOnRow(sudokuBoard, givenRow);
            if (!nakedPair.IsNakedPair) return;
            for (int col = 0; col < Constants.MaxGroupLength; col++)
            {
                var cell = sudokuBoard[givenRow, col];
                if (nakedPair.First != cell)
                {
                    var strValuesToEliminate = nakedPair.First.ToString();
                    ELiminateNakedPair(sudokuBoard, strValuesToEliminate, givenRow, col);
                }
            }
        }


        /// <summary>
        /// Checks if there is a naked pair in the given row.
        /// </summary>
        /// <param name="sudokuBoard">The represantation of the sudoku board.</param>
        /// <param name="givenRow">The given row.</param>
        /// <returns>A tuple with the the two naked numbers, and a bool value: IsNakedPair, true if a proper naked pair, false otherwise.</returns>
        private (int First, int Second, bool IsNakedPair) HasNakedPairOnRow(int[,] sudokuBoard, int givenRow)
        {
            for (int col1 = 0; col1 < Constants.MaxGroupLength; col1++)
                for (int col2 = 0; col2 < Constants.MaxGroupLength; col2++)
                {
                    if (!AreSameCells(col1, col2) && IsNakedPair(sudokuBoard[givenRow, col1], sudokuBoard[givenRow, col2]))
                    {
                        return (First: sudokuBoard[givenRow, col1],
                                Second: sudokuBoard[givenRow, col2],
                                IsNakedPair: true
                        );
                    }
                }
            return (-1, -1, false);
        }

        /// <summary>
        /// Eliminates the given values from the cell with the given row and column info.
        /// If a cell value is changed 
        /// </summary>
        /// <param name="sudokuBoard">The represantation of the sudoku board.</param>
        /// <param name="strValuesToEliminate">Values to be removed from the cell.</param>
        /// <param name="eliminateFromRow">The row of the given cell from which the given values are to be removed.</param>
        /// <param name="eliminateFromCol">The column of the given cell from which the given values are to be removed.</param>
        private void ELiminateNakedPair(int[,] sudokuBoard, string strValuesToEliminate, int eliminateFromRow, int eliminateFromCol)
        {
            var valuesToEliminateSet = strValuesToEliminate.ToHashSet();
            foreach (var valueToEliminate in valuesToEliminateSet)
            {
                var cell = sudokuBoard[eliminateFromRow, eliminateFromCol];
                var strCell = cell.ToString();
                strCell = strCell.Replace(valueToEliminate.ToString(), string.Empty);
                sudokuBoard[eliminateFromRow, eliminateFromCol] = Convert.ToInt32(strCell);
            }
        }

        /// <summary>
        /// A naked pair is when you have exactly two cells within a row, column, or 3×3 block 
        /// that only have the exact same two candidates.  
        /// </summary>
        /// <param name="firstNaked">First naked pair candidate.</param>
        /// <param name="seconNaked">Second naked pair candidate.</param>
        /// <returns>True if given numbers are a naked pair, false otherwise.</returns>
        private bool IsNakedPair(int firstNaked, int seconNaked)
        {
            return firstNaked.ToString().Length == 2 && firstNaked == seconNaked;
        }

        /// <summary>
        /// Checks if any of the two cells with the given indexes are same.
        /// Index: row, column or block index.
        /// </summary>
        /// <param name="index1"></param>
        /// <param name="index2"></param>
        /// <returns>True if all cells are different, and false otherwise.</returns>
        private bool AreSameCells(int index1, int index2)
        {
            return new HashSet<int> { index1, index2 }.Count != 2;
        }

    }
}


