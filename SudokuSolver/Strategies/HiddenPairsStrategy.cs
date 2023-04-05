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
                }
            }
            return sudokuBoard;
        }

        public void SolveForHiddenPairInRow(int[,] sudokuBoard, int givenRow, int givenCol)
        {
            
            for (int col = 0; col < sudokuBoard.GetLength(1); col++)
            {
                if (givenCol == col) continue;
                
                var dict = GetDigitOccurrencesDictionaryInRow(sudokuBoard, givenRow);
                var digits = dict.Select(p => p)
                    .Where(p => p.Value.Count() == 2 && p.Value.Contains(givenCol) && p.Value.Contains(col))
                    .ToDictionary(p=> p.Key, p=> p.Value);
                
                if (digits.Any())
                {
                    var cellValue = Convert.ToInt32(string.Join(string.Empty, digits.Keys.Select(k => k).ToList()));
                    CleanCellForHiddenPair(sudokuBoard, givenRow, givenCol, cellValue);
                    CleanCellForHiddenPair(sudokuBoard, givenRow, col, cellValue);
                    return;
                }
            }
        }

        private void CleanCellForHiddenPair(int[,] sudokuBoard, int givenRow, int gi1venCol, int hiddenPair)
        {
            sudokuBoard[givenRow, gi1venCol] = hiddenPair;
        }

        /// <summary>
        /// Go through each cell in the given row and count the occurrences of all the digits for the row.
        /// </summary>
        /// <param name="sudokuBoard"></param>
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
                var possibilities = cell.ToString().ToCharArray();

                foreach (var possibility in possibilities)
                {
                    digitOccurrencesDictionary[possibility].Add(col);
                }
            }

            return digitOccurrencesDictionary;

        }
    }
}
