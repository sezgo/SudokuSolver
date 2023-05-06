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
        public enum Group
        {
            Row,
            Col,
            Block
        }
        public int[,] Solve(int[,] sudokuBoard)
        {
            for (int index = 0; index < Constants.MaxGroupLength;  index++)
            {
                SolveHiddenTripleOnGroup(sudokuBoard, GetGroupArray(sudokuBoard, Group.Row, index), Group.Row, index);
                SolveHiddenTripleOnGroup(sudokuBoard, GetGroupArray(sudokuBoard, Group.Col, index), Group.Col, index);
                SolveHiddenTripleOnGroup(sudokuBoard, GetGroupArray(sudokuBoard, Group.Block, index), Group.Block, index);
            }
            return sudokuBoard;
        }

        private int[] GetGroupArray(int[,] sudokuBoard, Group groupType, int index)
        {
            int[] rowArray = new int[Constants.MaxGroupLength];

            if (groupType == Group.Row)
            {
                rowArray = Enumerable.Range(0, Constants.MaxGroupLength)
                    .Select(col => sudokuBoard[index, col])
                    .ToArray();
            }
            else if (groupType == Group.Col)
            {
                rowArray = Enumerable.Range(0, Constants.MaxGroupLength)
                    .Select(row => sudokuBoard[row, index])
                    .ToArray();
            }
            else if (groupType == Group.Block)
            {
                SudokuMapper mapper = new SudokuMapper();
                SudokuMap map = mapper.Find(index);
                rowArray = Enumerable.Range(0, Constants.MaxGroupLength)
                    .Select(block => sudokuBoard[mapper.GetCellRow(block, map), mapper.GetCellCol(block, map)])
                    .ToArray();
            }
            
            
            return rowArray;
        }
        //      ! 569 !    { 3, 7, 256, 4, 256, 8, 1, 2569, 29 }, 
        public void SolveHiddenTripleOnGroup(int[,]sudokuBoard, int[] group, Group groupType, int groupIndex)
        {
            var dict = GetDigitOccurrencesDictionaryInGroup(group);

            for (int index1 = 0; index1 < Constants.MaxGroupLength; index1++)
            {
                for (int index2 = 0; index2 < Constants.MaxGroupLength; index2++)
                {
                    for (int index3 = 0; index3 < Constants.MaxGroupLength; index3++)
                    {
                        if (AreSameCells(index1, index2, index3)) continue;

                        var hiddenTripleDict = dict.Select(t => t)
                            .Where(t => t.Value.Count() >= 2 && t.Value.Count() <= 3)
                            .Where(t => t.Value.Contains(index1) || t.Value.Contains(index2) || t.Value.Contains(index3))
                            .ToDictionary(t => t.Key, t => t.Value);
                        if (hiddenTripleDict.Count >= 3)
                        {
                            var firstCell = group[index1];
                            var secondCell = group[index2];
                            var thirdCell = group[index3];

                            var hiddenTripleCandidates = string.Join("", hiddenTripleDict.Keys);
                            if (IsHiddenTriple(firstCell, secondCell, thirdCell, hiddenTripleCandidates, group)) {
                                
                                var cellRow1 = groupIndex;
                                var cellCol1 = groupIndex;

                                var cellRow2 = groupIndex;
                                var cellCol2 = groupIndex;

                                var cellRow3 = groupIndex;
                                var cellCol3 = groupIndex;

                                if (groupType == Group.Row)
                                {
                                    cellCol1 = index1;
                                    cellCol2 = index2;
                                    cellCol3 = index3;
                                }

                                if (groupType == Group.Col)
                                {
                                    cellRow1 = index1;
                                    cellRow2 = index2;
                                    cellRow3 = index3;
                                }

                                if (groupType == Group.Block)
                                {
                                    SudokuMapper mapper = new SudokuMapper();
                                    SudokuMap map = mapper.Find(groupIndex);

                                    cellRow1 = mapper.GetCellRow(index1, map);
                                    cellCol1 = mapper.GetCellCol(index1, map);

                                    cellRow2 = mapper.GetCellRow(index2, map);
                                    cellCol2 = mapper.GetCellCol(index2, map);

                                    cellRow3 = mapper.GetCellRow(index3, map);
                                    cellCol3 = mapper.GetCellCol(index3, map);
                                }
                                CleanCellForHiddenTriple(sudokuBoard, cellRow1, cellCol1, hiddenTripleCandidates);
                                CleanCellForHiddenTriple(sudokuBoard, cellRow2, cellCol2, hiddenTripleCandidates);
                                CleanCellForHiddenTriple(sudokuBoard, cellRow3, cellCol3, hiddenTripleCandidates);
                            }
                        }
                    }
                }
            }

        }

        private Dictionary<char, List<int>> GetDigitOccurrencesDictionaryInGroup(int[] group)
        {
            Dictionary<char, List<int>> digitOccurrencesDictionary = new Dictionary<char, List<int>>();
            for (int i = 1; i <=9; i++)
            {
                digitOccurrencesDictionary[i.ToString()[0]] = new List<int>();
            }

            for (int index = 0; index < Constants.MaxGroupLength; index++)
            {
                var cell = group[index];
                if (cell.ToString().Length == 1) continue;
                var possibilities = cell.ToString().ToCharArray();

                foreach (var possibility in possibilities)
                {
                    digitOccurrencesDictionary[possibility].Add(index);
                }
            }
            return digitOccurrencesDictionary;
        }

        /// <summary>
        /// Checks the given three numbers, if they contain a hidden triplefor the given group.
        /// A hidden  triple occurs when there is a naked triple  but hidden behind other non-naked triple candidates.
        /// The digits of a hidden triple must not exist in any other cell than the three cells.
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
        internal bool IsHiddenTriple(int firstHidden, int secondHidden, int thirdHidden, string hiddenTripleCandidates, int[] group)
        {

            // Checks if anyother cell 
            foreach (var cell in group)
            {
                if (cell != firstHidden && cell != secondHidden && cell != thirdHidden)
                {
                    var cellStr = cell.ToString();
                    foreach (var digit in cellStr)
                    {
                        if (hiddenTripleCandidates.Contains(digit)) return false;
                    }
                }

            }

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
