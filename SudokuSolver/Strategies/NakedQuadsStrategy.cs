using SudokuSolver.Data;
using SudokuSolver.Workers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            for (int row = 0; row < sudokuBoard.GetLength(0); row++)
            {
                for (int col = 0; col < sudokuBoard.GetLength(1); col++)
                {
                    SolveNakedQuadForInRow(sudokuBoard, row, col);
                    SolveNakedQuadForInCol(sudokuBoard, row, col);
                    SolveNakedQuadForInBlock(sudokuBoard, row, col);
                }
            }
            return sudokuBoard;
        }

        /// <summary>
        /// If there is a naked quad in the block, removes all of the naked quad's digits from the notes of the other cells.
        /// </summary>
        /// <param name="sudokuBoard">The represantation of the sudoku board.</param>
        /// <param name="givenRow">Row of the pivot cell.</param>
        /// <param name="givenCol">Column of the pivot cell.</param>
        private void SolveNakedQuadForInBlock(int[,] sudokuBoard, int givenRow, int givenCol)
        {
            var nakedQuad = HasNakedQuadInBlock(sudokuBoard, givenRow, givenCol);
            if (!nakedQuad.IsNakedQuad) return;

            SudokuMap blockMap = _sudokuMapper.Find(givenRow, givenCol);

            var givenCellIndex = _sudokuMapper.GetCellIndex(givenRow, givenCol);

            for (int cellIndex = 0; cellIndex < sudokuBoard.GetLength(0); cellIndex++)
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
        /// Checks if there exist another three cells in the same block for the given cell with the given row and column info, so that they make a naked quad together.
        /// </summary>
        /// <param name="sudokuBoard">The represantation of the sudoku board.</param>
        /// <param name="givenRow">Row of the pivot cell.</param>
        /// <param name="givenCol">Column of the pivot cell.</param>
        /// <returns>A tuple with the the four naked numbers, and a bool value: IsNakedQuad, true if a proper naked quad, false otherwise.</returns>
        private (int First, int Second, int Third, int Fourth, bool IsNakedQuad) HasNakedQuadInBlock(int[,] sudokuBoard, int givenRow, int givenCol)
        {
            SudokuMap blockMap = _sudokuMapper.Find(givenRow, givenCol);

            var givenCellIndex = _sudokuMapper.GetCellIndex(givenRow, givenCol);

            for (int secondCellIndex = 0; secondCellIndex < sudokuBoard.GetLength(0); secondCellIndex++)
            {
                for (int thirdCellIndex = 0; thirdCellIndex < sudokuBoard.GetLength(0); thirdCellIndex++)
                {
                    for (int fourthCellIndex = 0; fourthCellIndex < sudokuBoard.GetLength(0); fourthCellIndex++)
                    {
                        if (!AreSameCells(givenCellIndex, secondCellIndex, thirdCellIndex, fourthCellIndex))
                        {
                            var secondRow = _sudokuMapper.GetCellRow(secondCellIndex, blockMap);
                            var secondCol = _sudokuMapper.GetCellCol(secondCellIndex, blockMap);

                            var thirdRow = _sudokuMapper.GetCellRow(thirdCellIndex, blockMap);
                            var thirdCol = _sudokuMapper.GetCellCol(thirdCellIndex, blockMap);

                            var fourthRow = _sudokuMapper.GetCellRow(fourthCellIndex, blockMap);
                            var fourthCol = _sudokuMapper.GetCellCol(fourthCellIndex, blockMap);

                            if (IsNakedQuad(sudokuBoard[givenRow, givenCol],
                                sudokuBoard[secondRow, secondCol],
                                sudokuBoard[thirdRow, thirdCol],
                                sudokuBoard[fourthRow, fourthCol]))
                            {
                                return (
                                    First: sudokuBoard[givenRow, givenCol],
                                    Second: sudokuBoard[secondRow, secondCol],
                                    Third: sudokuBoard[thirdRow, thirdCol],
                                    Fourth: sudokuBoard[fourthRow, fourthCol],
                                    IsNakedQuad: true
                                    );
                            }
                        }
                    }
                }
            }

            return (-1, -1, -1, -1, false);
        }

        /// <summary>
        /// If there is a naked quad in the column, removes the all of the naked quad's digits from the notes of the other cells.
        /// </summary>
        /// <param name="sudokuBoard">The represantation of the sudoku board.</param>
        /// <param name="givenRow">Row of the pivot cell.</param>
        /// <param name="givenCol">Column of the pivot cell.</param>
        private void SolveNakedQuadForInCol(int[,] sudokuBoard, int givenRow, int givenCol)
        {
            var nakedQuad = HasNakedQuadInCol(sudokuBoard, givenRow, givenCol);
            if (!nakedQuad.IsNakedQuad) return;
            for (int row = 0; row < sudokuBoard.GetLength(1); row++)
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
        /// Checks if there exist another three cells in the same column for the given cell with the given row and column info, so that they make a naked quad together.
        /// </summary>
        /// <param name="sudokuBoard">The represantation of the sudoku board.</param>
        /// <param name="givenRow">Row of the pivot cell.</param>
        /// <param name="givenCol">Column of the pivot cell.</param>
        /// <param name="givenCol">Column of the pivot cell.</param>
        /// <returns>A tuple with the the four naked numbers, and a bool value: IsNakedQuad, true if a proper naked quad, false otherwise.</returns>
        private (int First, int Second, int Third, int Fourth, bool IsNakedQuad) HasNakedQuadInCol(int[,] sudokuBoard, int givenRow, int givenCol)
        {
            for (int row2 = 0; row2 < sudokuBoard.GetLength(0); row2++)
            {
                for (int row3 = 0; row3 < sudokuBoard.GetLength(0); row3++)
                {
                    for (int row4 = 0; row4 < sudokuBoard.GetLength(0); row4++)
                    {
                        if (!AreSameCells(givenRow, row2, row3, row4) &&
                            IsNakedQuad(sudokuBoard[givenRow, givenCol], sudokuBoard[row2, givenCol], sudokuBoard[row3, givenCol], sudokuBoard[row4, givenCol]))
                        {
                            return (
                                First: sudokuBoard[givenRow, givenCol],
                                Second: sudokuBoard[row2, givenCol],
                                Third: sudokuBoard[row3, givenCol],
                                Fourth: sudokuBoard[row4, givenCol],
                                IsNakedQuad: true
                            );
                        }
                    }
                }
            }
            return (-1, -1, -1, -1, false);
        }

        /// <summary>
        /// If there is a naked quad in the row, removes the all of the naked quad's digits from the notes of the other cells.
        /// </summary>
        /// <param name="sudokuBoard">The represantation of the sudoku board.</param>
        /// <param name="givenRow">Row of the pivot cell.</param>
        /// <param name="givenCol">Column of the pivot cell.</param>
        private void SolveNakedQuadForInRow(int[,] sudokuBoard, int givenRow, int givenCol)
        {
            var nakedQuad = HasNakedQuadInRow(sudokuBoard, givenRow, givenCol);
            if (!nakedQuad.IsNakedQuad) return;
            for (int col = 0; col < sudokuBoard.GetLength(1); col++)
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
        /// Checks if there exist another three cells in the same row for the given cell with the given row and column info, so that they make a naked Triple together.
        /// </summary>
        /// <param name="sudokuBoard">The represantation of the sudoku board.</param>
        /// <param name="givenRow">Row of the pivot cell.</param>
        /// <param name="givenCol">Column of the pivot cell.</param>
        /// <returns>A tuple with the the four naked numbers, and a bool value: IsNakedQuad, true if a proper naked quad, false otherwise.</returns>
        internal (int First, int Second, int Third, int Fourth, bool IsNakedQuad) HasNakedQuadInRow(int[,] sudokuBoard, int givenRow, int givenCol)
        {
            for (int col2 = 0; col2 < sudokuBoard.GetLength(1); col2++)
            {
                for (int col3 = 0; col3 < sudokuBoard.GetLength(1); col3++)
                {
                    for (int col4 = 0; col4 < sudokuBoard.GetLength(1); col4++)
                    {
                        if (!AreSameCells(givenCol, col2, col3, col4) &&
                            IsNakedQuad(sudokuBoard[givenRow, givenCol], sudokuBoard[givenRow, col2], sudokuBoard[givenRow, col3], sudokuBoard[givenRow, col4]))
                        {
                            return (
                                First: sudokuBoard[givenRow, givenCol],
                                Second: sudokuBoard[givenRow, col2],
                                Third: sudokuBoard[givenRow, col3],
                                Fourth: sudokuBoard[givenRow, col4],
                                IsNakedQuad: true
                            );
                        }
                    }
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
        /// <param name="firstNaked"></param>
        /// <param name="secondNaked"></param>
        /// <param name="thirdNaked"></param>
        /// <param name="fourthNaked"></param>
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
