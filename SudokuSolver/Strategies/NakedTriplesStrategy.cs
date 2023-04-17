using SudokuSolver.Data;
using SudokuSolver.Workers;

namespace SudokuSolver.Strategies
{
    internal class NakedTriplesStrategy : ISudokuStrategy
    {
        private readonly SudokuMapper _sudokuMapper;
        public NakedTriplesStrategy(SudokuMapper sudokuMapper) {
            _sudokuMapper = sudokuMapper;
        }

        public int[,] Solve(int[,] sudokuBoard)
        {
            for (int index = 0; index < Constants.MaxGroupLength; index++)
            {
                SolveNakedTripleOnRow(sudokuBoard, index);
                SolveNakedTripleOnCol(sudokuBoard, index);
                SolveNakedTripleOnBlock(sudokuBoard, index);
            }

            return sudokuBoard;
        }

        /// <summary>
        /// If there is a naked triple in the block, removes the all of the naked triple digits from the notes of the other cells.
        /// </summary>
        /// <param name="sudokuBoard">The represantation of the sudoku board.</param>
        /// <param name="givenBlockIndex">The given block index.</param>
        internal void SolveNakedTripleOnBlock(int[,] sudokuBoard, int givenBlockIndex)
        {
            var nakedTriple = HasNakedTripleOnBlock(sudokuBoard, givenBlockIndex);
            if (!nakedTriple.IsNakedTriple) return;

            SudokuMap blockMap = _sudokuMapper.Find(givenBlockIndex);

            for (int cellIndex = 0; cellIndex < Constants.MaxGroupLength; cellIndex++)
            {
                var cellRow = _sudokuMapper.GetCellRow(cellIndex, blockMap);
                var cellCol = _sudokuMapper.GetCellCol(cellIndex, blockMap);
                var cell = sudokuBoard[cellRow, cellCol];
                if (nakedTriple.First != cell && nakedTriple.Second != cell && nakedTriple.Third != cell)
                {
                    var strValuesToEliminate = nakedTriple.First.ToString() + nakedTriple.Second.ToString() + nakedTriple.Third.ToString();
                    ELiminateNakedTriple(sudokuBoard, strValuesToEliminate, cellRow, cellCol);
                }
            }
        }

        /// <summary>
        /// Checks if there is a naked triple in the given block.
        /// </summary>
        /// <param name="sudokuBoard">The represantation of the sudoku board.</param>
        /// <param name="givenBlockIndex">The given block index.</param>
        /// <returns>A tuple with the the three naked numbers, and a bool value: IsNakedTriple, true if a proper naked Triple, false otherwise.</returns>
        internal (int First, int Second, int Third, bool IsNakedTriple) HasNakedTripleOnBlock(int[,] sudokuBoard, int givenBlockIndex)
        {
            SudokuMap blockMap = _sudokuMapper.Find(givenBlockIndex);

            for (int firstCellIndex = 0; firstCellIndex < Constants.MaxGroupLength; firstCellIndex++)
            for (int secondCellIndex = 0; secondCellIndex < Constants.MaxGroupLength; secondCellIndex++)
            for (int thirdCellIndex = 0; thirdCellIndex < Constants.MaxGroupLength; thirdCellIndex++)
            {
                if (!AreSameCells(firstCellIndex, secondCellIndex, thirdCellIndex))
                {
                    var firstRow = _sudokuMapper.GetCellRow(firstCellIndex, blockMap);
                    var firstCol = _sudokuMapper.GetCellCol(firstCellIndex, blockMap);

                    var secondRow = _sudokuMapper.GetCellRow(secondCellIndex, blockMap);
                    var secondCol = _sudokuMapper.GetCellCol(secondCellIndex, blockMap);
                        
                    var thirdRow = _sudokuMapper.GetCellRow(thirdCellIndex, blockMap);
                    var thirdCol = _sudokuMapper.GetCellCol(thirdCellIndex, blockMap);
                        
                    if (IsNakedTriple(sudokuBoard[firstRow, firstCol], 
                        sudokuBoard[secondRow, secondCol],
                        sudokuBoard[thirdRow, thirdCol]))
                    {
                        return (
                            First: sudokuBoard[firstRow, firstCol],
                            Second: sudokuBoard[secondRow, secondCol],
                            Third: sudokuBoard[thirdRow, thirdCol],
                            IsNakedTriple: true
                            );
                    }
                }
                
            }

            return (-1, -1,-1,false);
        }

        /// <summary>
        /// If there is a naked triple in the block, removes the all of the naked triple' digits from the notes of the other cells.
        /// </summary>
        /// <param name="sudokuBoard">The represantation of the sudoku board.</param>
        /// <param name="givenCol">The given column.</param>
        internal void SolveNakedTripleOnCol(int[,] sudokuBoard, int givenCol)
        {
            var nakedTriple = HasNakedTripleOnCol(sudokuBoard, givenCol);
            if (!nakedTriple.IsNakedTriple) return;
            for (int row = 0; row < Constants.MaxGroupLength; row++)
            {
                var cell = sudokuBoard[row, givenCol];
                if (cell != nakedTriple.First && cell != nakedTriple.Second && cell != nakedTriple.Third)
                {
                    var strValuesToEliminate = nakedTriple.First.ToString() + nakedTriple.Second.ToString() + nakedTriple.Third.ToString();
                    ELiminateNakedTriple(sudokuBoard, strValuesToEliminate, row, givenCol);
                }
            }
        }


        /// <summary>
        /// Checks if there is a naked triple in the given column.
        /// </summary>
        /// <param name="sudokuBoard">The represantation of the sudoku board.</param>
        /// <param name="givenCol">The given column.</param>
        /// <returns>A tuple with the the three naked numbers, and a bool value: IsNakedTriple, true if a proper naked Triple, false otherwise.</returns>
        internal (int First, int Second, int Third, bool IsNakedTriple) HasNakedTripleOnCol(int[,] sudokuBoard, int givenCol)
        {
            for (int row1 = 0; row1 < Constants.MaxGroupLength; row1++)
            for (int row2 = 0; row2 < Constants.MaxGroupLength; row2++)
            for (int row3 = 0; row3 < Constants.MaxGroupLength; row3++)
            {
                if ((row1 != row2 && row1 != row3 && row2!= row3) &&
                    IsNakedTriple(sudokuBoard[row1, givenCol], sudokuBoard[row2, givenCol], sudokuBoard[row3, givenCol])) 
                {
                    return (
                        First: sudokuBoard[row1, givenCol],
                        Second: sudokuBoard[row2, givenCol],
                        Third: sudokuBoard[row3, givenCol],
                        IsNakedTriple: true
                    );
                }
                
            }
            return (-1, -1, -1, false);
        }

        /// <summary>
        /// If there is a naked triple in the row, removes the naked triple digits from other non naked triple cells.
        /// </summary>
        /// <param name="sudokuBoard">The current represantation of the sudoku board.</param>
        /// <param name="givenRow">The given row.</param>
        internal void SolveNakedTripleOnRow(int[,] sudokuBoard, int givenRow)
        {
            var nakedTriple = HasNakedTripleOnRow(sudokuBoard, givenRow);
            if (!nakedTriple.IsNakedTriple) return;
            for (int col = 0; col < Constants.MaxGroupLength; col++)
            {
                var cell = sudokuBoard[givenRow, col];
                if (nakedTriple.First != cell && nakedTriple.Second != cell && nakedTriple.Third != cell)
                {
                    var strValuesToEliminate = nakedTriple.First.ToString() + nakedTriple.Second.ToString() + nakedTriple.Third.ToString();
                    ELiminateNakedTriple(sudokuBoard, strValuesToEliminate, givenRow, col);
                }
            }
        }

        /// <summary>
        /// Checks if there is a naked triple in the given row.
        /// </summary>
        /// <param name="sudokuBoard">The represantation of the sudoku board.</param>
        /// <param name="givenRow">The given row.</param>
        /// <returns>A tuple with the the four naked numbers, and a bool value: IsNakedQuad, true if a proper naked triple, false otherwise.</returns>
        internal (int First, int Second, int Third, bool IsNakedTriple) HasNakedTripleOnRow(int[,] sudokuBoard, int givenRow)
        {

            for (int col1 = 0; col1 < Constants.MaxGroupLength; col1++)
            for (int col2 = 0; col2 < Constants.MaxGroupLength; col2++)
            for (int col3 = 0; col3 < Constants.MaxGroupLength; col3++)
            {
                if (!AreSameCells(col1, col2, col3) &&
                    IsNakedTriple(sudokuBoard[givenRow, col2], sudokuBoard[givenRow, col3], sudokuBoard[givenRow, col1]))
                {
                    return (
                        First: sudokuBoard[givenRow, col1], 
                        Second: sudokuBoard[givenRow, col2], 
                        Third: sudokuBoard[givenRow, col3], 
                        IsNakedTriple: true
                    );
                }
                
            }
            return (-1, -1, -1, false);
        }

        /// <summary>
        /// Eliminates the given values from the cell with the given row and column info.
        /// </summary>
        /// <param name="sudokuBoard">The represantation of the sudoku board.</param>
        /// <param name="strValuesToEliminate">Values to be removed from the cell.</param>
        /// <param name="eliminateFromRow">The row of the given cell from which the given values are to be removed.</param>
        /// <param name="eliminateFromCol">The column of the given cell from which the given values are to be removed.</param>
        internal void ELiminateNakedTriple(int[,] sudokuBoard, string strValuesToEliminate, int eliminateFromRow, int eliminateFromCol)
        {
            var valuesToEliminateArray = strValuesToEliminate.ToHashSet();
            foreach (var valueToEliminate in valuesToEliminateArray)
            {
                var cell = sudokuBoard[eliminateFromRow, eliminateFromCol];
                var strCell = cell.ToString();
                strCell = strCell.Replace(valueToEliminate.ToString(), string.Empty);
                sudokuBoard[eliminateFromRow, eliminateFromCol] = Convert.ToInt32(strCell);
            }
        }

        /// <summary>
        /// Checks the given three numbers, if they are a naked triple.
        /// A naked triple occurs when you have exactly three cells within a row, column, or 3×3 region 
        /// where the only candidates are the same three digits or a subset of these three digits. 
        /// </summary>
        /// 
        /// <example>12, 18, 28</example>
        /// <example>178, 178, 178</example>
        /// <example>18, 38, 138</example>
        /// <example>18, 158, 158</example>
        /// 
        /// <param name="firstNaked">First naked Triple candidate.</param>
        /// <param name="secondNaked">Second naked Triple candidate.</param>
        /// <param name="thirdNaked">Third naked Triple candidate.</param>
        /// 
        /// <returns>Returns true if the three numbers are a naked Triple, false otherwise.</returns>
        internal bool IsNakedTriple(int firstNaked, int secondNaked, int thirdNaked)
        {

            string strFirstNaked = firstNaked.ToString();
            string strSecondNaked = secondNaked.ToString();
            string strThirdNaked = thirdNaked.ToString();

            if (strFirstNaked.Length == 1 || strSecondNaked.Length == 1 || strThirdNaked.Length == 1)
                return false;

            HashSet<char> set = new HashSet<char>();
            set.UnionWith(strFirstNaked);
            set.UnionWith(strSecondNaked);
            set.UnionWith(strThirdNaked);

            return set.Count == 3;
        }

        /// <summary>
        /// Checks if any of the three cells with the given indexes are same.
        /// Index: row, column or block index.
        /// </summary>
        /// <param name="index1"></param>
        /// <param name="index2"></param>
        /// <param name="index3"></param>
        /// <returns>True if all cells are different, and false otherwise.</returns>
        private bool AreSameCells(int index1, int index2, int index3)
        {
            return new HashSet<int> { index1, index2, index3 }.Count != 3;
        }
    }
}
