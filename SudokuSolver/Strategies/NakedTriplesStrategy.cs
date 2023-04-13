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
    internal class NakedTriplesStrategy : ISudokuStrategy
    {
        private readonly SudokuMapper _sudokuMapper;
        public NakedTriplesStrategy(SudokuMapper sudokuMapper) {
            _sudokuMapper = sudokuMapper;
        }

        /// <summary>
        /// Attempts solving the given sudoku board with the Naked Triple Strategy.
        /// </summary>
        /// <param name="sudokuBoard">The represantation of the sudoku board.</param>
        /// <returns>The state of the sudoku board after solving attempt.</returns>
        public int[,] Solve(int[,] sudokuBoard)
        {
            for (int row = 0; row < sudokuBoard.GetLength(1); row++)
            {
                for (int col = 0; col < sudokuBoard.GetLength(1); col++)
                {
                    ELiminateNakedTripleFromOthersInRow(sudokuBoard, row, col);
                    EliminateNakedTripleFromOthersInCol(sudokuBoard, row, col);
                    EliminateNakedTripleFromOthersInBlock(sudokuBoard, row, col);
                }
            }
            return sudokuBoard;
        }

        /// <summary>
        /// If a cell in the same block as the naked Triple has more than three digit notes, removes the all of the naked Triple's digits from the notes.
        /// </summary>
        /// <param name="sudokuBoard">The represantation of the sudoku board.</param>
        /// <param name="givenRow">Row of the pivot cell.</param>
        /// <param name="givenCol">Column of the pivot cell.</param>
        internal void EliminateNakedTripleFromOthersInBlock(int[,] sudokuBoard, int givenRow, int givenCol)
        {
            var nakedTriple = HasNakedTripleInBlock(sudokuBoard, givenRow, givenCol);
            if (!nakedTriple.IsNakedTriple) return;

            SudokuMap blockMap = _sudokuMapper.Find(givenRow, givenCol);
            for (int row = blockMap.StartRow; row < blockMap.StartRow+3; row++)
            {
                for (int col = blockMap.StartCol; col < blockMap.StartCol+3; col++)
                {
                    var cell = sudokuBoard[row, col];
                    if (nakedTriple.First != cell && nakedTriple.Second != cell && nakedTriple.Third != cell)
                    {
                        var strValuesToEliminate = nakedTriple.First.ToString() + nakedTriple.Second.ToString() + nakedTriple.Third.ToString();
                        ELiminateNakedTriple(sudokuBoard, strValuesToEliminate, row, col);
                    }
                }
            }
        }

        /// <summary>
        /// Checks if there exist another two cells in the same block for the given cell with the given row and column info, so that they make a naked Triple together.
        /// </summary>
        /// <param name="sudokuBoard">The represantation of the sudoku board.</param>
        /// <param name="givenRow">Row of the pivot cell.</param>
        /// <param name="givenCol">Column of the pivot cell.</param>
        /// <returns>A tuple with the the three naked numbers, and a bool value: IsNakedTriple, true if a proper naked Triple, false otherwise.</returns>
        internal (int First, int Second, int Third, bool IsNakedTriple) HasNakedTripleInBlock(int[,] sudokuBoard, int givenRow, int givenCol)
        {
            SudokuMap blockMap = _sudokuMapper.Find(givenRow, givenCol);

            var givenCellIndex = _sudokuMapper.GetCellIndex(givenRow, givenCol);

            for (int secondCellIndex = 0; secondCellIndex < sudokuBoard.GetLength(0); secondCellIndex++)
            {
                for (int thirdCellIndex = 0; thirdCellIndex < sudokuBoard.GetLength(0); thirdCellIndex++)
                {
                    if (!AreSameCells(givenCellIndex, secondCellIndex, thirdCellIndex))
                    {
                        var secondRow = _sudokuMapper.GetCellRow(secondCellIndex, blockMap);
                        var secondCol = _sudokuMapper.GetCellCol(secondCellIndex, blockMap);
                        
                        var thirdRow = _sudokuMapper.GetCellRow(thirdCellIndex, blockMap);
                        var thirdCol = _sudokuMapper.GetCellCol(thirdCellIndex, blockMap);
                        
                        if (IsNakedTriple(sudokuBoard[givenRow, givenCol], 
                            sudokuBoard[secondRow, secondCol],
                            sudokuBoard[thirdRow, thirdCol]))
                        {
                            return (
                                First: sudokuBoard[givenRow, givenCol],
                                Second: sudokuBoard[secondRow, secondCol],
                                Third: sudokuBoard[thirdRow, thirdCol],
                                IsNakedTriple: true
                                );
                        }
                    }
                }
            }

            return (-1, -1,-1,false);
        }

        /// <summary>
        /// If a cell in the same column as the naked Triple has more than three digit notes, removes the all of the naked Triple's digits from the notes.
        /// </summary>
        /// <param name="sudokuBoard">The represantation of the sudoku board.</param>
        /// <param name="givenRow">Row of the pivot cell.</param>
        /// <param name="givenCol">Column of the pivot cell.</param>
        internal void EliminateNakedTripleFromOthersInCol(int[,] sudokuBoard, int givenRow, int givenCol)
        {
            var nakedTriple = HasNakedTripleInCol(sudokuBoard, givenRow, givenCol);
            if (!nakedTriple.IsNakedTriple) return;
            for (int row = 0; row < sudokuBoard.GetLength(0); row++)
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
        /// Checks if there exist another two cells in the same column for the given cell with the given row and column info, so that they make a naked Triple together.
        /// </summary>
        /// <param name="sudokuBoard">The represantation of the sudoku board.</param>
        /// <param name="givenRow">Row of the pivot cell.</param>
        /// <param name="givenCol">Column of the pivot cell.</param>
        /// <returns>A tuple with the the three naked numbers, and a bool value: IsNakedTriple, true if a proper naked Triple, false otherwise.</returns>
        internal (int First, int Second, int Third, bool IsNakedTriple) HasNakedTripleInCol(int[,] sudokuBoard, int givenRow, int givenCol)
        {
            for (int row = 0; row < sudokuBoard.GetLength(0); row++)
            {
                for (int row2 = 0; row2 < sudokuBoard.GetLength(0); row2++)
                {
                    if ((givenRow != row && givenRow != row2 && row!= row2) &&
                        IsNakedTriple(sudokuBoard[givenRow, givenCol], sudokuBoard[row, givenCol], sudokuBoard[row2, givenCol])) 
                    {
                        return (
                            First: sudokuBoard[givenRow, givenCol],
                            Second: sudokuBoard[row, givenCol],
                            Third: sudokuBoard[row2, givenCol],
                            IsNakedTriple: true
                        );
                    }
                }
            }
            return (-1, -1, -1, false);
        }

        /// <summary>
        /// If a cell in the same row as the naked Triple has more than three digit notes, removes the all of the naked Triple's digits from the notes.
        /// </summary>
        /// <param name="sudokuBoard">The represantation of the sudoku board.</param>
        /// <param name="givenRow">Row of the pivot cell.</param>
        /// <param name="givenCol">Column of the pivot cell.</param>
        internal void ELiminateNakedTripleFromOthersInRow(int[,] sudokuBoard, int givenRow, int givenCol)
        {
            var nakedTriple = HasNakedTripleInRow(sudokuBoard, givenRow, givenCol);
            if (!nakedTriple.IsNakedTriple) return;
            for (int col = 0; col < sudokuBoard.GetLength(1); col++)
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
        /// Checks if there exist another two cells in the same row for the given cell with the given row and column info, so that they make a naked Triple together.
        /// </summary>
        /// <param name="sudokuBoard">The represantation of the sudoku board.</param>
        /// <param name="givenRow">Row of the pivot cell.</param>
        /// <param name="givenCol">Column of the pivot cell.</param>
        /// <returns>A tuple with the the three naked numbers, and a bool value: IsNakedTriple, true if a proper naked Triple, false otherwise.</returns>
        internal (int First, int Second, int Third, bool IsNakedTriple) HasNakedTripleInRow(int[,] sudokuBoard, int givenRow, int givenCol)
        {
            for (int col = 0; col < sudokuBoard.GetLength(1); col++)
            {
                for (int col2 = 0; col2 < sudokuBoard.GetLength(1); col2++)
                {
                    if ((givenCol != col && givenCol != col2 && col != col2) &&
                        IsNakedTriple(sudokuBoard[givenRow, col], sudokuBoard[givenRow, col2], sudokuBoard[givenRow, givenCol]))
                    {
                        return (
                            First: sudokuBoard[givenRow, givenCol], 
                            Second: sudokuBoard[givenRow, col], 
                            Third: sudokuBoard[givenRow, col2], 
                            IsNakedTriple: true
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
        internal void ELiminateNakedTriple(int[,] sudokuBoard, string strValuesToEliminate, int eliminateFromRow, int eliminateFromCol)
        {
            var valuesToEliminateArray = strValuesToEliminate.ToHashSet();
            foreach (var valueToEliminate in valuesToEliminateArray)
            {
                var cell = sudokuBoard[eliminateFromRow, eliminateFromCol];
                var strCell = cell.ToString();
                strCell.Replace(valueToEliminate.ToString(), string.Empty);
                sudokuBoard[eliminateFromRow, eliminateFromCol] = Convert.ToInt32(sudokuBoard[eliminateFromRow, eliminateFromCol].ToString().Replace(valueToEliminate.ToString(), string.Empty));
            }
        }

        /// <summary>
        /// Given three numbers checks if they are a possible naked triple.
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
        /// Checks two cells with the given row and column if they are the same cell.
        /// </summary>
        /// <param name="row1">Row of the first cell.</param>
        /// <param name="col1">Column of the first cel.l</param>
        /// <param name="row2">Row of the second cell.</param>
        /// <param name="col2">Column of the second cell.</param>
        /// <returns>Returns true if the two cells are the same and false otherwise.</returns>
        private bool AreSameCells(int index1, int index2, int index3)
        {
            return (index1 == index2 || index1 == index3 || index2 == index3);
        }
    }
}
