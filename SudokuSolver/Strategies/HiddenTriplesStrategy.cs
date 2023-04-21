using SudokuSolver.Data;
using SudokuSolver.Workers;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Strategies
{
    internal class HiddenTriplesStrategy : ISudokuStrategy
    {
        private enum Group
        {
            Row,
            Col,
            Block
        }
        public int[,] Solve(int[,] sudokuBoard)
        {
            for (int index = 0; index < Constants.MaxGroupLength;  index++)
            {
                //SolveHiddenTripleOnRow(sudokuBoard, index);
                int[] temp = GetGroupArray(sudokuBoard, index, Group.Block);
            }
            return sudokuBoard;
        }

        private int[] GetGroupArray(int[,] sudokuBoard, int index, Group group)
        {
            int[] rowArray = new int[Constants.MaxGroupLength];

            if (group == Group.Row)
            {
                rowArray = Enumerable.Range(0, Constants.MaxGroupLength)
                    .Select(col => sudokuBoard[index, col])
                    .ToArray();
            }
            else if (group == Group.Col)
            {
                rowArray = Enumerable.Range(0, Constants.MaxGroupLength)
                    .Select(row => sudokuBoard[row, index])
                    .ToArray();
            }
            else if (group == Group.Block)
            {
                SudokuMapper mapper = new SudokuMapper();
                SudokuMap map = mapper.Find(index);
                rowArray = Enumerable.Range(0, Constants.MaxGroupLength)
                    .Select(block => sudokuBoard[mapper.GetCellRow(block, map), mapper.GetCellCol(block, map)])
                    .ToArray();
            }
            
            
            return rowArray;
        }

        public void SolveHiddenTripleOnRow(int[,] sudokuBoard, int row)
        {
            var dict = GetDigitOccurrencesDictionaryInRow(sudokuBoard, row);
            for (int col1 = 0; col1 < Constants.MaxGroupLength; col1++)
            {
                for (int col2 = 0; col2 < Constants.MaxGroupLength; col2++)
                {
                    for (int col3 = 0; col3 < Constants.MaxGroupLength; col3++)
                    {
                        if (AreSameCells(col1, col2, col3)) continue;
                        var hiddenTripleDict = dict.Select(t => t)
                            .Where(t => t.Value.Count() >= 2 && t.Value.Count() <= 3)
                            .Where(t => t.Value.Contains(col1) || t.Value.Contains(col2) || t.Value.Contains(col3))
                            .ToDictionary(t => t.Key, t => t.Value);
                        if (hiddenTripleDict.Count >= 3)
                        {
                            var firstCell = sudokuBoard[row, col1];
                            var secondCell = sudokuBoard[row, col2];
                            var thirdCell = sudokuBoard[row, col3];

                            var hiddenTripleCandidates = string.Join("", hiddenTripleDict.Keys);
                            if (IsHiddenTriple(firstCell, secondCell, thirdCell, hiddenTripleCandidates)) {
                                CleanCellForHiddenTriple(sudokuBoard, row, col1, hiddenTripleCandidates);
                                CleanCellForHiddenTriple(sudokuBoard, row, col2, hiddenTripleCandidates);
                                CleanCellForHiddenTriple(sudokuBoard, row, col3, hiddenTripleCandidates);
                            }
                        }
                    }
                }
            }

        }

        private Dictionary<char, List<int>> GetDigitOccurrencesDictionaryInRow(int[,] sudokuBoard, int givenRow)
        {
            Dictionary<char, List<int>> digitOccurrencesDictionary = new Dictionary<char, List<int>>();
            for (int i = 1; i <=9; i++)
            {
                digitOccurrencesDictionary[i.ToString()[0]] = new List<int>();
            }

            for (int col = 0; col < Constants.MaxGroupLength; col++)
            {
                var cell = sudokuBoard[givenRow, col];
                if (cell.ToString().Length == 1) continue;
                var possibilities = cell.ToString().ToCharArray();

                foreach (var possibility in possibilities)
                {
                    digitOccurrencesDictionary[possibility].Add(col);
                }
            }
            return digitOccurrencesDictionary;
        }

        /// <summary>
        /// Checks the given three numbers, if they contain a hidden triple.
        /// A hidden  triple occurs when there is a naked triple  but hidden behind other non-naked triple candidates.
        /// </summary>
        /// 
        /// <example>
        /// { 137, 9, 18, 347, 2, 347, 158, 56, 16 } -- 37, 347, 347
        /// </example>
        /// 
        /// <param name="firstHidden">First hidden Triple candidate.</param>
        /// <param name="secondHidden">Second hidden Triple candidate.</param>
        /// <param name="thirdHidden">Third hidden Triple candidate.</param>
        /// 
        /// <returns>Returns true if the three numbers are a naked Triple, false otherwise.</returns>
        internal bool IsHiddenTriple(int firstHidden, int secondHidden, int thirdHidden, string hiddenTripleCandidates)
        {
            string strFirstNaked = StripCell(firstHidden.ToString(), hiddenTripleCandidates);
            string strSecondNaked = StripCell(secondHidden.ToString(), hiddenTripleCandidates);
            string strThirdNaked = StripCell(thirdHidden.ToString(), hiddenTripleCandidates);

            if (strFirstNaked.Length <= 1 || strSecondNaked.Length <= 1 || strThirdNaked.Length <= 1)
                return false;

            HashSet<char> set = new HashSet<char>();
            set.UnionWith(strFirstNaked);
            set.UnionWith(strSecondNaked);
            set.UnionWith(strThirdNaked);

            return set.Count == 3;
        }

        private string StripCell(string cellValue, string hiddenTripleCandidates)
        {
            foreach (var charInCell in cellValue)
            {
                if (!hiddenTripleCandidates.Contains(charInCell))
                {
                    cellValue = cellValue.Replace(charInCell.ToString(), string.Empty);
                }
            }
            return cellValue;
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

        /// <summary>
        /// Cleans the cell from non triple candidates.
        /// </summary>
        /// <param name="sudokuBoard">The current state of the board.</param>
        /// <param name="givenRow">Row of the cell</param>
        /// <param name="gi1venCol">Column of the cell</param>
        /// <param name="hiddenTriple">The notes of hidden pair.</param>
        private void CleanCellForHiddenTriple(int[,] sudokuBoard, int givenRow, int givenCol, string hiddenTripleCandidates)
        {
            var strCellValue = sudokuBoard[givenRow, givenCol].ToString();
            sudokuBoard[givenRow, givenCol] = Convert.ToInt32(StripCell(strCellValue, hiddenTripleCandidates));
        }
    }
}
