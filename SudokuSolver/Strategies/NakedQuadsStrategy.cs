using SudokuSolver.Data;
using SudokuSolver.Workers;

namespace SudokuSolver.Strategies
{
    internal class NakedQuadsStrategy : ISudokuStrategy
    {
        private readonly SudokuMapper _sudokuMapper;
        public NakedQuadsStrategy(SudokuMapper sudokuMapper)
        {
            _sudokuMapper = sudokuMapper;
        }
        public int[,] Solve(int[,] sudokuBoard)
        {
            for (int index = 0; index < Constants.MaxGroupLength; index++)
            {
                SolveNakedQuadOnRow(sudokuBoard, index);
                SolveNakedQuadOnCol(sudokuBoard, index);
                SolveNakedQuadOnBlock(sudokuBoard, index);
            }

            return sudokuBoard;
        }

        /// <summary>
        /// If there is a naked quad in the block, removes the all of the naked quad's digits from the notes of the other cells.
        /// </summary>
        /// <param name="sudokuBoard">The represantation of the sudoku board.</param>
        /// <param name="givenBlockIndex">The given block index.</param>
        private void SolveNakedQuadOnBlock(int[,] sudokuBoard, int givenBlockIndex)
        {
            var nakedQuad = HasNakedQuadOnBlock(sudokuBoard, givenBlockIndex);
            if (!nakedQuad.IsNakedQuad) return;

            SudokuMap blockMap = _sudokuMapper.Find(givenBlockIndex);

            for (int cellIndex = 0; cellIndex < Constants.MaxGroupLength; cellIndex++)
            {
                var cellRow = _sudokuMapper.GetCellRow(cellIndex, blockMap);
                var cellCol = _sudokuMapper.GetCellCol(cellIndex, blockMap);
                var cell = sudokuBoard[cellRow, cellCol];

                if (nakedQuad.First != cell && nakedQuad.Second != cell && nakedQuad.Third != cell && nakedQuad.Fourth != cell)
                {
                    var strValuesToEliminate = nakedQuad.First.ToString() + nakedQuad.Second.ToString() + nakedQuad.Third.ToString() + nakedQuad.Fourth.ToString();
                    EliminateNakedQuad(sudokuBoard, strValuesToEliminate, cellRow, cellCol);
                }
            }
        }

        /// <summary>
        /// Checks if there is a naked quad in the given block.
        /// </summary>
        /// <param name="sudokuBoard">The represantation of the sudoku board.</param>
        /// <param name="givenBlockIndex">The given block index.</param>
        /// <returns>A tuple with the the four naked numbers, and a bool value: IsNakedQuad, true if a proper naked quad, false otherwise.</returns>
        private (int First, int Second, int Third, int Fourth, bool IsNakedQuad) HasNakedQuadOnBlock(int[,] sudokuBoard, int givenBlockIndex)
        {
            SudokuMap blockMap = _sudokuMapper.Find(givenBlockIndex);

            for (int firstCellIndex = 0; firstCellIndex < Constants.MaxGroupLength; firstCellIndex++)
            for (int secondCellIndex = 0; secondCellIndex < Constants.MaxGroupLength; secondCellIndex++)
            for (int thirdCellIndex = 0; thirdCellIndex < Constants.MaxGroupLength; thirdCellIndex++)
            for (int fourthCellIndex = 0; fourthCellIndex < Constants.MaxGroupLength; fourthCellIndex++)
            {
                if (!AreSameCells(firstCellIndex, secondCellIndex, thirdCellIndex, fourthCellIndex))
                {
                    var firstRow = _sudokuMapper.GetCellRow(firstCellIndex, blockMap);
                    var firstCol = _sudokuMapper.GetCellCol(firstCellIndex, blockMap);

                    var secondRow = _sudokuMapper.GetCellRow(secondCellIndex, blockMap);
                    var secondCol = _sudokuMapper.GetCellCol(secondCellIndex, blockMap);

                    var thirdRow = _sudokuMapper.GetCellRow(thirdCellIndex, blockMap);
                    var thirdCol = _sudokuMapper.GetCellCol(thirdCellIndex, blockMap);

                    var fourthRow = _sudokuMapper.GetCellRow(fourthCellIndex, blockMap);
                    var fourthCol = _sudokuMapper.GetCellCol(fourthCellIndex, blockMap);

                    if (IsNakedQuad(sudokuBoard[firstRow, firstCol],
                        sudokuBoard[secondRow, secondCol],
                        sudokuBoard[thirdRow, thirdCol],
                        sudokuBoard[fourthRow, fourthCol]))
                    {
                        return (
                            First: sudokuBoard[firstRow, firstCol],
                            Second: sudokuBoard[secondRow, secondCol],
                            Third: sudokuBoard[thirdRow, thirdCol],
                            Fourth: sudokuBoard[fourthRow, fourthCol],
                            IsNakedQuad: true
                            );
                    }
                }
            }
            

            return (-1, -1, -1, -1, false);
        }

        /// <summary>
        /// If there is a naked quad in the column, removes the all of the naked quad's digits from the notes of the other cells.
        /// </summary>
        /// <param name="sudokuBoard">The represantation of the sudoku board.</param>
        /// <param name="givenCol">The given column.</param>
        private void SolveNakedQuadOnCol(int[,] sudokuBoard, int givenCol)
        {
            var nakedQuad = HasNakedQuadOnCol(sudokuBoard, givenCol);
            if (!nakedQuad.IsNakedQuad) return;
            for (int row = 0; row < Constants.MaxGroupLength; row++)
            {
                var cell = sudokuBoard[row, givenCol];
                if (nakedQuad.First != cell && nakedQuad.Second != cell && nakedQuad.Third != cell && nakedQuad.Fourth != cell)
                {
                    var strValuesToEliminate = nakedQuad.First.ToString() + nakedQuad.Second.ToString() + nakedQuad.Third.ToString() + nakedQuad.Fourth.ToString();
                    EliminateNakedQuad(sudokuBoard, strValuesToEliminate, row, givenCol);
                }
            }
        }

        /// <summary>
        /// Checks if there is a naked quad in the given column.
        /// </summary>
        /// <param name="sudokuBoard">The represantation of the sudoku board.</param>
        /// <param name="givenCol">The given column..</param>
        /// <returns>A tuple with the the four naked numbers, and a bool value: IsNakedQuad, true if a proper naked quad, false otherwise.</returns>
        private (int First, int Second, int Third, int Fourth, bool IsNakedQuad) HasNakedQuadOnCol(int[,] sudokuBoard, int givenCol)
        {
            for (int row1 = 0; row1 < Constants.MaxGroupLength; row1++)
            for (int row2 = 0; row2 < Constants.MaxGroupLength; row2++)
            for (int row3 = 0; row3 < Constants.MaxGroupLength; row3++)
            for (int row4 = 0; row4 < Constants.MaxGroupLength; row4++)
            {
                if (!AreSameCells(row1, row2, row3, row4) &&
                                IsNakedQuad(sudokuBoard[row1, givenCol], sudokuBoard[row2, givenCol], sudokuBoard[row3, givenCol], sudokuBoard[row4, givenCol]))
                {
                    return (
                        First: sudokuBoard[row1, givenCol],
                        Second: sudokuBoard[row2, givenCol],
                        Third: sudokuBoard[row3, givenCol],
                        Fourth: sudokuBoard[row4, givenCol],
                        IsNakedQuad: true
                    );
                }
            }
            
            return (-1, -1, -1, -1, false);
        }

        /// <summary>
        /// If there is a naked quad in the row, removes the naked quad digits from other non naked quad cells.
        /// </summary>
        /// <param name="sudokuBoard">The current represantation of the sudoku board.</param>
        /// <param name="givenRow">The given row.</param>
        private void SolveNakedQuadOnRow(int[,] sudokuBoard, int givenRow)
        {
            var nakedQuad = HasNakedQuadOnRow(sudokuBoard, givenRow);
            if (!nakedQuad.IsNakedQuad) return;
            for (int col = 0; col < Constants.MaxGroupLength; col++)
            {
                var cell = sudokuBoard[givenRow, col];
                if (nakedQuad.First != cell && nakedQuad.Second != cell && nakedQuad.Third != cell && nakedQuad.Fourth != cell)
                {
                    var strValuesToEliminate = nakedQuad.First.ToString() + nakedQuad.Second.ToString() + nakedQuad.Third.ToString() + nakedQuad.Fourth.ToString();
                    EliminateNakedQuad(sudokuBoard, strValuesToEliminate, givenRow, col);
                }
            }
        }

        /// <summary>
        /// Checks if there is a naked quad in the given row.
        /// </summary>
        /// <param name="sudokuBoard">The represantation of the sudoku board.</param>
        /// <param name="givenRow">The given row.</param>
        /// <returns>A tuple with the the four naked numbers, and a bool value: IsNakedQuad, true if a proper naked quad, false otherwise.</returns>
        internal (int First, int Second, int Third, int Fourth, bool IsNakedQuad) HasNakedQuadOnRow(int[,] sudokuBoard, int givenRow)
        {
            for (int col1 = 0; col1 < Constants.MaxGroupLength; col1++)
            for (int col2 = 0; col2 < Constants.MaxGroupLength; col2++)
            for (int col3 = 0; col3 < Constants.MaxGroupLength; col3++)
            for (int col4 = 0; col4 < Constants.MaxGroupLength; col4++)
            {
                if (!AreSameCells(col1, col2, col3, col4) &&
                    IsNakedQuad(sudokuBoard[givenRow, col1], sudokuBoard[givenRow, col2], sudokuBoard[givenRow, col3], sudokuBoard[givenRow, col4]))
                {
                    return (
                        First: sudokuBoard[givenRow, col1],
                        Second: sudokuBoard[givenRow, col2],
                        Third: sudokuBoard[givenRow, col3],
                        Fourth: sudokuBoard[givenRow, col4],
                        IsNakedQuad: true
                    );
                }
            }
            
            return (-1, -1, -1, -1, false);
        }

        /// <summary>
        /// Eliminates the given values from the cell with the given row and column info.
        /// </summary>
        /// <param name="sudokuBoard">The represantation of the sudoku board.</param>
        /// <param name="strValuesToEliminate">Values to be removed from the cell.</param>
        /// <param name="eliminateFromRow">The row of the given cell from which the given values are to be removed.</param>
        /// <param name="eliminateFromCol">The column of the given cell from which the given values are to be removed.</param>
        private void EliminateNakedQuad(int[,] sudokuBoard, string strValuesToEliminate, int eliminateFromRow, int eliminateFromCol)
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
        /// Checks the given four numbers, if they are a naked quad.
        /// A naked quad occurs when you have exactly four cells within a row, column or a 3x3 region
        /// where the only candidates are the same four digits or a subset of these four digits.
        /// </summary>
        /// 
        /// <example>12, 24, 23, 34</example>
        /// <example>1234, 1234, 123, 34</example>
        /// 
        /// <param name="firstNaked">First naked Quad candidate.</param>
        /// <param name="secondNaked">Second naked Quad candidate.</param>
        /// <param name="thirdNaked">Third naked Quad candidate.</param>
        /// <param name="fourthNaked">Fourth naked Quad candidate.</param>
        /// <returns>True if given numbers are a naked quad, false otherwise.</returns>
        internal bool IsNakedQuad(int firstNaked, int secondNaked, int thirdNaked, int fourthNaked)
        {

            string strFirstNaked = firstNaked.ToString();
            string strSecondNaked = secondNaked.ToString();
            string strThirdNaked = thirdNaked.ToString();
            string strFourthNaked = fourthNaked.ToString();

            if (strFirstNaked.Length == 1 || strSecondNaked.Length == 1 || strThirdNaked.Length == 1 || strFourthNaked.Length == 1)
                return false;

            HashSet<char> set = new HashSet<char>();
            set.UnionWith(strFirstNaked);
            set.UnionWith(strSecondNaked);
            set.UnionWith(strThirdNaked);
            set.UnionWith(strFourthNaked);

            return set.Count == 4;
        }

        /// <summary>
        /// Checks if any of the four cells with the given indexes are same.
        /// Index: row, column or block index.
        /// </summary>
        /// <param name="index1"></param>
        /// <param name="index2"></param>
        /// <param name="index3"></param>
        /// <param name="index4"></param>
        /// <returns>True if all cells are different, and false otherwise.</returns>
        public bool AreSameCells(int index1, int index2, int index3, int index4)
        {
            return new HashSet<int> { index1, index2, index3, index4}.Count != 4;
        }
    }
}
