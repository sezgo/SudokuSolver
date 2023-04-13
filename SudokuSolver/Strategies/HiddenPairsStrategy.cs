using SudokuSolver.Workers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Strategies
{
    internal class HiddenPairsStrategy : ISudokuStrategy
    {
        private readonly SudokuMapper _sudokuMapper;
        public HiddenPairsStrategy(SudokuMapper sudokuMapper) 
        {
            _sudokuMapper = sudokuMapper;
        }

        public int[,] Solve(int[,] sudokuBoard)
        {
            for (int row = 0; row < sudokuBoard.GetLength(0); row++)
            {
                for(int col = 0; col < sudokuBoard.GetLength(1); col++)
                {
                    SolveForHiddenPairInRow(sudokuBoard, row, col);
                    SolveForHiddenPairInCol(sudokuBoard, row, col);
                    SolveForHiddenPairInBlock(sudokuBoard, row, col);
                }
            }
            return sudokuBoard;
        }

        /// <summary>
        /// For the given row and column checks if the pivot cell is a hidden pair with any other cell in the block,
        /// and solves the first encounter.
        /// </summary>
        /// <param name="sudokuBoard">The current stat of the board.</param>
        /// <param name="givenRow">The pivot cell row.</param>
        /// <param name="givenCol">The pivot cell column.</param>
        public void SolveForHiddenPairInBlock(int[,] sudokuBoard, int givenRow, int givenCol)
        {
            var dict = GetDigitOccurrencesDictionaryInBlock(sudokuBoard, givenRow, givenCol); 
            var map = _sudokuMapper.Find(givenRow, givenCol);
            var givenCellIndex  = _sudokuMapper.GetCellIndex(givenRow, givenCol);

            for (int cellIndex = 0; cellIndex < sudokuBoard.GetLength(0); cellIndex++)
            {
                var row = _sudokuMapper.GetCellRow(cellIndex, map);
                var col = _sudokuMapper.GetCellCol(cellIndex, map);

                if (givenRow == row && givenCol == col) continue;

                var hiddenPairDict = dict.Select(p => p)
                    .Where(p => p.Value.Count() == 2 && p.Value.Contains(cellIndex) && p.Value.Contains(givenCellIndex))
                    .ToDictionary(p => p.Key, p => p.Value);

                if (hiddenPairDict.Count == 2)
                {
                    var cellValue = Convert.ToInt32(string.Join(string.Empty, hiddenPairDict.Keys.Select(k => k).ToList()));
                    CleanCellForHiddenPair(sudokuBoard, row, col, cellValue);
                    CleanCellForHiddenPair(sudokuBoard, givenRow, givenCol, cellValue);
                    return;
                }
            }

        }

        /// <summary>
        /// Go through each cell in the given row and count the occurrences of all the digits for the row.
        /// </summary>
        /// <param name="sudokuBoard">The current state of the board.</param>
        /// <param name="givenCol">The given column.</param>
        /// <returns>
        /// A dictionary that holds the cell index each occurrence for every digit.
        /// </returns>
        private Dictionary<char, List<int>> GetDigitOccurrencesDictionaryInBlock(int[,] sudokuBoard, int givenRow, int givenCol)
        {
            Dictionary<char, List<int>> digitOccurrencesDictionary = new Dictionary<char, List<int>>();
            var map = _sudokuMapper.Find(givenRow, givenCol);
            for (int i = 1; i <= 9; i++)
            {
                digitOccurrencesDictionary[i.ToString()[0]] = new List<int>();
            }

            for (int cellIndex = 0; cellIndex < sudokuBoard.GetLength(0); cellIndex++)
            {

                var row = _sudokuMapper.GetCellRow(cellIndex, map);
                var col = _sudokuMapper.GetCellCol(cellIndex, map);

                var cell = sudokuBoard[row, col];
                if (cell == 0) continue;
                var possibilities = cell.ToString().ToCharArray();

                foreach (var possibility in possibilities)
                {
                    digitOccurrencesDictionary[possibility].Add(cellIndex);
                }
            }

            return digitOccurrencesDictionary;

        }


        /// <summary>
        /// For the given row and column checks if the pivot cell is a hidden pair with any other cell in the col,
        /// and solves the first encounter.
        /// </summary>
        /// <param name="sudokuBoard">The current stat of the board.</param>
        /// <param name="givenRow">The pivot cell row.</param>
        /// <param name="givenCol">The pivot cell column.</param>
        public void SolveForHiddenPairInCol(int[,] sudokuBoard, int givenRow, int givenCol)
        {

            for (int row = 0; row < sudokuBoard.GetLength(1); row++)
            {
                if (givenRow == row) continue;

                var dict = GetDigitOccurrencesDictionaryInCol(sudokuBoard, givenCol);
                var hiddenPairDict = dict.Select(p => p)
                    .Where(p => p.Value.Count() == 2 && p.Value.Contains(givenRow) && p.Value.Contains(row))
                    .ToDictionary(p => p.Key, p => p.Value);

                if (hiddenPairDict.Count == 2)
                {
                    var cellValue = Convert.ToInt32(string.Join(string.Empty, hiddenPairDict.Keys.Select(k => k).ToList()));
                    CleanCellForHiddenPair(sudokuBoard, givenRow, givenCol, cellValue);
                    CleanCellForHiddenPair(sudokuBoard, row, givenCol, cellValue);
                    return;
                }
            }
        }

        /// <summary>
        /// Go through each cell in the given row and count the occurrences of all the digits for the row.
        /// </summary>
        /// <param name="sudokuBoard">The current state of the board.</param>
        /// <param name="givenCol">The given column.</param>
        /// <returns>A dictionary that holds the each occurrence for every digit.</returns>
        private Dictionary<char, List<int>> GetDigitOccurrencesDictionaryInCol(int[,] sudokuBoard, int givenCol)
        {
            Dictionary<char, List<int>> digitOccurrencesDictionary = new Dictionary<char, List<int>>();
            for (int i = 1; i <= 9; i++)
            {
                digitOccurrencesDictionary[i.ToString()[0]] = new List<int>();
            }

            for (int row = 0; row < sudokuBoard.GetLength(0); row++)
            {
                var cell = sudokuBoard[row, givenCol];
                if (cell == 0) continue;
                var possibilities = cell.ToString().ToCharArray();

                foreach (var possibility in possibilities)
                {
                    digitOccurrencesDictionary[possibility].Add(row);
                }
            }

            return digitOccurrencesDictionary;

        }

        /// <summary>
        /// For the given row and column checks if the pivot cell is a hidden pair with any other cell in the row,
        /// and solves the first encounter.
        /// </summary>
        /// <param name="sudokuBoard">The current stat of the board.</param>
        /// <param name="givenRow">The pivot cell row.</param>
        /// <param name="givenCol">The pivot cell column.</param>
        public void SolveForHiddenPairInRow(int[,] sudokuBoard, int givenRow, int givenCol)
        {
            
            for (int col = 0; col < sudokuBoard.GetLength(1); col++)
            {
                if (givenCol == col) continue;
                
                var dict = GetDigitOccurrencesDictionaryInRow(sudokuBoard, givenRow);
                var hiddenPairDict = dict.Select(p => p)
                    .Where(p => p.Value.Count() == 2 && p.Value.Contains(givenCol) && p.Value.Contains(col))
                    .ToDictionary(p=> p.Key, p=> p.Value);
                
                if (hiddenPairDict.Count == 2)
                {
                    var cellValue = Convert.ToInt32(string.Join(string.Empty, hiddenPairDict.Keys.Select(k => k).ToList()));
                    CleanCellForHiddenPair(sudokuBoard, givenRow, givenCol, cellValue);
                    CleanCellForHiddenPair(sudokuBoard, givenRow, col, cellValue);
                    return;
                }
            }
        }

        /// <summary>
        /// Go through each cell in the given row and count the occurrences of all the digits for the row.
        /// </summary>
        /// <param name="sudokuBoard">The current state of the board.</param>
        /// <param name="givenRow"></param>
        /// <returns>A dictionary that holds the each occurrence for every digit.</returns>
        private Dictionary<char, List<int>> GetDigitOccurrencesDictionaryInRow(int[,] sudokuBoard, int givenRow)
        {
            Dictionary<char, List<int>> digitOccurrencesDictionary = new Dictionary<char, List<int>>();
            for (int i = 1; i <= 9; i++)
            {
                digitOccurrencesDictionary[i.ToString()[0]] = new List<int>();
            }

            for (int col = 0; col < sudokuBoard.GetLength(1); col++)
            {
                var cell = sudokuBoard[givenRow, col];
                if (cell == 0) continue;
                var possibilities = cell.ToString().ToCharArray();

                foreach (var possibility in possibilities)
                {
                    digitOccurrencesDictionary[possibility].Add(col);
                }
            }

            return digitOccurrencesDictionary;

        }

        /// <summary>
        /// Overrides the hidden pair cells with only hidden pair notes.
        /// </summary>
        /// <param name="sudokuBoard">The current state of the board.</param>
        /// <param name="givenRow">Row of the cell</param>
        /// <param name="gi1venCol">Column of the cell</param>
        /// <param name="hiddenPair">The notes of hidden pair.</param>
        private void CleanCellForHiddenPair(int[,] sudokuBoard, int givenRow, int gi1venCol, int hiddenPair)
        {
            sudokuBoard[givenRow, gi1venCol] = hiddenPair;
        }

    }
}
