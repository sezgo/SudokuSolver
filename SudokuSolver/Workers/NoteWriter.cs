using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Workers
{
    internal class NoteWriter
    {
        private readonly SudokuMapper _sudokuMapper;

        public NoteWriter(SudokuMapper sudokuMapper)
        {
            _sudokuMapper = sudokuMapper;
        }

        public void UpdateNotes(int[,] sudokuBoard)
        {
            for (int row = 0; row < sudokuBoard.GetLength(0); row++)
            {
                for (int col = 0; col < sudokuBoard.GetLength(1); col++)
                {
                    if (sudokuBoard[row, col] == 0 || sudokuBoard[row, col].ToString().Length > 1)
                    {
                        var notesForRowAndCol = GetNotesForRowAndCol(sudokuBoard, row, col);
                        var notesForBlock = GetNotesForBlock(sudokuBoard, row, col);
                        sudokuBoard[row, col] = GetNotesIntersection(notesForRowAndCol, notesForBlock);
                    }
                }
            }
        }

        private int GetNotesForRowAndCol(int[,] sudokuBoard, int givenRow, int givenCol)
        {
            int[] possibilities = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            for (int col = 0; col < 9; col++)
            {
                if (IsValidSingle(sudokuBoard[givenRow, col]))
                {
                    possibilities[sudokuBoard[givenRow, col] - 1] = 0;
                }
            }
            for (int row = 0; row < 9; row++)
            {
                if (IsValidSingle(sudokuBoard[row, givenCol]))
                {
                    possibilities[sudokuBoard[row, givenCol] - 1] = 0;
                }
            }
            return Convert.ToInt32(string.Join(string.Empty, possibilities.Select(p => p).Where(p => p != 0)));
        }


        private int GetNotesForBlock(int[,] sudokuBoard, int givenRow, int givenCol)
        {
            int[] possibilities = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var sudokuMap = _sudokuMapper.Find(givenRow, givenCol);
            for (int row = sudokuMap.StartRow; row < sudokuMap.StartRow + 3; row++)
            {
                for (int col = sudokuMap.StartCol; col < sudokuMap.StartCol + 3; col++)
                {
                    if (IsValidSingle(sudokuBoard[row, col]))
                    {
                        possibilities[sudokuBoard[row, col] - 1] = 0;
                    }
                }
            }
            return Convert.ToInt32(string.Join(string.Empty, possibilities.Select(p => p).Where(p => p != 0)));
        }

        private int GetNotesIntersection(int notesForRowAndCol, int notesForBlock)
        {
            var notesForRowAndColCharArray = notesForRowAndCol.ToString().ToCharArray();
            var notesForBlockCharArray = notesForBlock.ToString().ToCharArray();
            var notesSubset = notesForRowAndColCharArray.Intersect(notesForBlockCharArray);
            return Convert.ToInt32(string.Join(string.Empty, notesSubset));
        }

        private bool IsValidSingle(int cellDigit)
        {
            return cellDigit != 0 && cellDigit.ToString().Length == 1;
        }
    }
}
