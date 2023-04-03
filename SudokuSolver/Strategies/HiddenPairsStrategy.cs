using SudokuSolver.Workers;
using System;
using System.Collections.Generic;
using System.Linq;
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
                    EliminateHiddenPairInRow(sudokuBoard, row, col);
                }
            }
            return sudokuBoard;
        }


        private void EliminateHiddenPairInRow(int[,] sudokuBoard, int givenRow, int givenCol)
        {
            FindHiddenPairInRow(sudokuBoard, givenRow);
            for (int col = 0; col < sudokuBoard.GetLength(1); col++)
            {
                
            }
        }


        public void FindHiddenPairInRow(int[,] sudokuBoard, int givenRow)
        {
            var dict = GetDigitOccurrencesDictionary(sudokuBoard, givenRow);

            foreach(var key in dict.Keys)
            {
                if (dict[key].Count != 2)
                {
                    dict.Remove(key);
                }
            }


        }

        /// <summary>
        /// Go through each cell in the given row and count the occurrences of all the digits for the row.
        /// </summary>
        /// <param name="sudokuBoard"></param>
        /// <param name="givenRow"></param>
        /// <returns>A dictionary that holds the each occurrence for every digit.</returns>
        private Dictionary<char, List<int>> GetDigitOccurrencesDictionary(int[,] sudokuBoard, int givenRow)
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
