using SudokuSolver.Data;
using SudokuSolver.Workers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Strategies
{
    internal class NakedTriosStrategy : ISudokuStrategy
    {
        private readonly SudokuMapper _sudokuMapper;
        public NakedTriosStrategy(SudokuMapper sudokuMapper) {
            _sudokuMapper = sudokuMapper;
        }

        /// <summary>
        /// Attempts solving the given sudoku board with the Naked Trio Strategy.
        /// </summary>
        /// <param name="sudokuBoard">The represantation of the sudoku board.</param>
        /// <returns>The state of the sudoku board after solving attempt.</returns>
        public int[,] Solve(int[,] sudokuBoard)
        {
            for (int row = 0; row < sudokuBoard.GetLength(1); row++)
            {
                for (int col = 0; col < sudokuBoard.GetLength(1); col++)
                {
                    ELiminateNakedTrioFromOthersInRow(sudokuBoard, row, col);
                    EliminateNakedTrioFromOthersInCol(sudokuBoard, row, col);
                    EliminateNakedTrioFromOthersInBlock(sudokuBoard, row, col);
                }
            }
            return sudokuBoard;
        }

        /// <summary>
        /// If a cell in the same block as the naked trio has more than three digit notes, removes the all of the naked trio's digits from the notes.
        /// </summary>
        /// <param name="sudokuBoard">The represantation of the sudoku board.</param>
        /// <param name="givenRow">Row of the pivot cell.</param>
        /// <param name="givenCol">Column of the pivot cell.</param>
        internal void EliminateNakedTrioFromOthersInBlock(int[,] sudokuBoard, int givenRow, int givenCol)
        {
            var nakedTrio = HasNakedTrioInBlock(sudokuBoard, givenRow, givenCol);
            if (!nakedTrio.IsNakedTrio) return;

            SudokuMap blockMap = _sudokuMapper.Find(givenRow, givenCol);
            for (int row = blockMap.StartRow; row < blockMap.StartRow+3; row++)
            {
                for (int col = blockMap.StartCol; col < blockMap.StartCol+3; col++)
                {
                    var cell = sudokuBoard[row, col];
                    if (nakedTrio.First != cell && nakedTrio.Second != cell && nakedTrio.Third != cell)
                    {
                        var strValuesToEliminate = nakedTrio.First.ToString() + nakedTrio.Second.ToString() + nakedTrio.Third.ToString();
                        ELiminateNakedTrio(sudokuBoard, strValuesToEliminate, row, col);
                    }
                }
            }
        }

        /// <summary>
        /// Checks if there exist another two cells in the same block for the given cell with the given row and column info, so that they make a naked trio together.
        /// </summary>
        /// <param name="sudokuBoard">The represantation of the sudoku board.</param>
        /// <param name="givenRow">Row of the pivot cell.</param>
        /// <param name="givenCol">Column of the pivot cell.</param>
        /// <returns>A tuple with the the three naked numbers, and a bool value: IsNakedTrio, true if a proper naked trio, false otherwise.</returns>
        internal (int First, int Second, int Third, bool IsNakedTrio) HasNakedTrioInBlock(int[,] sudokuBoard, int givenRow, int givenCol)
        {
            SudokuMap blockMap = _sudokuMapper.Find(givenRow, givenCol);
            for (int row = blockMap.StartRow; row < blockMap.StartRow + 3; row++)
            {
                for (int col = blockMap.StartCol; col < blockMap.StartCol + 3; col++)
                {
                    for (int row2 = blockMap.StartRow; row2 < blockMap.StartRow + 3; row2++)
                    {
                        for (int col2 = blockMap.StartCol; col2 < blockMap.StartCol + 3; col2++)
                        {
                            
                            if (!AreSameCells(givenRow, givenCol, row, col) && !AreSameCells(givenRow, givenCol, row2, col2) && !AreSameCells(row, col, row2, col2))
                            {
                                
                                if (IsNakedTrio(sudokuBoard[givenRow, givenCol], sudokuBoard[row, col], sudokuBoard[row2, col2]))
                                {
                                    return (
                                        First: sudokuBoard[givenRow, givenCol],
                                        Second: sudokuBoard[row, col],
                                        Third: sudokuBoard[row2, col2],
                                        IsNakedTrio: true
                                        );
                                }
                            }
                        }
                    }
                }
            }
            return (-1, -1,-1,false);
        }

        /// <summary>
        /// If a cell in the same column as the naked trio has more than three digit notes, removes the all of the naked trio's digits from the notes.
        /// </summary>
        /// <param name="sudokuBoard">The represantation of the sudoku board.</param>
        /// <param name="givenRow">Row of the pivot cell.</param>
        /// <param name="givenCol">Column of the pivot cell.</param>
        internal void EliminateNakedTrioFromOthersInCol(int[,] sudokuBoard, int givenRow, int givenCol)
        {
            var nakedTrio = HasNakedTrioInCol(sudokuBoard, givenRow, givenCol);
            if (!nakedTrio.IsNakedTrio) return;
            for (int row = 0; row < sudokuBoard.GetLength(0); row++)
            {
                var cell = sudokuBoard[row, givenCol];
                if (cell != nakedTrio.First && cell != nakedTrio.Second && cell != nakedTrio.Third)
                {
                    var strValuesToEliminate = nakedTrio.First.ToString() + nakedTrio.Second.ToString() + nakedTrio.Third.ToString();
                    ELiminateNakedTrio(sudokuBoard, strValuesToEliminate, row, givenCol);
                }
            }
        }


        /// <summary>
        /// Checks if there exist another two cells in the same column for the given cell with the given row and column info, so that they make a naked trio together.
        /// </summary>
        /// <param name="sudokuBoard">The represantation of the sudoku board.</param>
        /// <param name="givenRow">Row of the pivot cell.</param>
        /// <param name="givenCol">Column of the pivot cell.</param>
        /// <returns>A tuple with the the three naked numbers, and a bool value: IsNakedTrio, true if a proper naked trio, false otherwise.</returns>
        internal (int First, int Second, int Third, bool IsNakedTrio) HasNakedTrioInCol(int[,] sudokuBoard, int givenRow, int givenCol)
        {
            for (int row = 0; row < sudokuBoard.GetLength(0); row++)
            {
                for (int row2 = 0; row2 < sudokuBoard.GetLength(0); row2++)
                {
                    if ((givenRow != row || givenRow != row2 || row!= row2) &&
                        IsNakedTrio(sudokuBoard[givenRow, givenCol], sudokuBoard[row, givenCol], sudokuBoard[row2, givenCol])) 
                    {
                        return (
                            First: sudokuBoard[givenRow, givenCol],
                            Second: sudokuBoard[row, givenCol],
                            Third: sudokuBoard[row2, givenCol],
                            IsNakedTrio: true
                        );
                    }
                }
            }
            return (-1, -1, -1, false);
        }

        /// <summary>
        /// If a cell in the same row as the naked trio has more than three digit notes, removes the all of the naked trio's digits from the notes.
        /// </summary>
        /// <param name="sudokuBoard">The represantation of the sudoku board.</param>
        /// <param name="givenRow">Row of the pivot cell.</param>
        /// <param name="givenCol">Column of the pivot cell.</param>
        internal void ELiminateNakedTrioFromOthersInRow(int[,] sudokuBoard, int givenRow, int givenCol)
        {
            var nakedTrio = HasNakedTrioInRow(sudokuBoard, givenRow, givenCol);
            if (!nakedTrio.IsNakedTrio) return;
            for (int col = 0; col < sudokuBoard.GetLength(1); col++)
            {
                var cell = sudokuBoard[givenRow, col];
                if (nakedTrio.First != cell && nakedTrio.Second != cell && nakedTrio.Third != cell)
                {
                    var strValuesToEliminate = nakedTrio.First.ToString() + nakedTrio.Second.ToString() + nakedTrio.Third.ToString();
                    ELiminateNakedTrio(sudokuBoard, strValuesToEliminate, givenRow, col);
                }
            }
        }

        /// <summary>
        /// Checks if there exist another two cells in the same row for the given cell with the given row and column info, so that they make a naked trio together.
        /// </summary>
        /// <param name="sudokuBoard">The represantation of the sudoku board.</param>
        /// <param name="givenRow">Row of the pivot cell.</param>
        /// <param name="givenCol">Column of the pivot cell.</param>
        /// <returns>A tuple with the the three naked numbers, and a bool value: IsNakedTrio, true if a proper naked trio, false otherwise.</returns>
        internal (int First, int Second, int Third, bool IsNakedTrio) HasNakedTrioInRow(int[,] sudokuBoard, int givenRow, int givenCol)
        {
            for (int col = 0; col < sudokuBoard.GetLength(1); col++)
            {
                for (int col2 = 0; col2 < sudokuBoard.GetLength(1); col2++)
                {
                    if ((givenCol != col || givenCol != col2 || col != col2) &&
                        IsNakedTrio(sudokuBoard[givenRow, col], sudokuBoard[givenRow, col2], sudokuBoard[givenRow, givenCol]))
                    {
                        return (
                            First: sudokuBoard[givenRow, givenCol], 
                            Second: sudokuBoard[givenRow, col], 
                            Third: sudokuBoard[givenRow, col2], 
                            IsNakedTrio: true
                        );
                    }
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
        internal void ELiminateNakedTrio(int[,] sudokuBoard, string strValuesToEliminate, int eliminateFromRow, int eliminateFromCol)
        {
            var valuesToEliminateArray = strValuesToEliminate.ToArray();
            foreach (var valueToEliminate in valuesToEliminateArray)
            {
                sudokuBoard[eliminateFromRow, eliminateFromCol] = Convert.ToInt32(sudokuBoard[eliminateFromRow, eliminateFromCol].ToString().Replace(valueToEliminate.ToString(), string.Empty));
            }
        }

        /// <summary>
        /// Given three numbers checks if they are a possible naked trio.
        /// </summary>
        /// <example>12, 18, 28</example>
        /// <param name="firstNaked">First naked trio candidate.</param>
        /// <param name="secondNaked">Second naked trio candidate.</param>
        /// <param name="thirdNaked">Third naked trio candidate.</param>
        /// <returns>Returns true if the three numbers are a naked trio, false otherwise.</returns>
        internal bool IsNakedTrio(int firstNaked, int secondNaked, int thirdNaked)
        {

            string strFirstNaked = firstNaked.ToString();
            string strSecondNaked = secondNaked.ToString();
            string strThirdNaked = thirdNaked.ToString();

            if (strFirstNaked.Length != 2 || strSecondNaked.Length != 2 || strThirdNaked.Length != 2) return false;

            if (strFirstNaked.ElementAt(0) == strSecondNaked.ElementAt(0))
            {
                if ((strFirstNaked.ElementAt(1) == strThirdNaked.ElementAt(0) &&
                    strSecondNaked.ElementAt(1) == strThirdNaked.ElementAt(1)) ||
                    (strFirstNaked.ElementAt(1) == strThirdNaked.ElementAt(1) &&
                    strSecondNaked.ElementAt(1) == strThirdNaked.ElementAt(0))) return true;
            }
            else if (strSecondNaked.ElementAt(0) == strThirdNaked.ElementAt(0))
            {
                if ((strSecondNaked.ElementAt(1) == strFirstNaked.ElementAt(0) &&
                    strThirdNaked.ElementAt(1) == strFirstNaked.ElementAt(1)) ||
                    (strSecondNaked.ElementAt(1) == strFirstNaked.ElementAt(1) &&
                    strThirdNaked.ElementAt(1) == strFirstNaked.ElementAt(0))) return true;
            }
            else if (strFirstNaked.ElementAt(0) == strThirdNaked.ElementAt(0))
            {
                if ((strFirstNaked.ElementAt(1) == strSecondNaked.ElementAt(0) &&
                    strThirdNaked.ElementAt(1) == strSecondNaked.ElementAt(1)) ||
                    (strFirstNaked.ElementAt(1) == strSecondNaked.ElementAt(1) &&
                    strThirdNaked.ElementAt(1) == strSecondNaked.ElementAt(0))) return true;
            }
            return false;
        }

        /// <summary>
        /// Checks two cells with the given row and column if they are the same cell.
        /// </summary>
        /// <param name="row1">Row of the first cell.</param>
        /// <param name="col1">Column of the first cel.l</param>
        /// <param name="row2">Row of the second cell.</param>
        /// <param name="col2">Column of the second cell.</param>
        /// <returns>Returns true if the two cells are the same and false otherwise.</returns>
        private bool AreSameCells(int row1, int col1, int row2, int col2)
        {
            return (row1 == row2 && col1 == col2);
        }
    }
}
