using SudokuSolver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Workers
{
    internal class SudokuMapper
    {
        public SudokuMap Find(int givenRow, int givenCol)
        {
            SudokuMap sudokuMap = new SudokuMap();

            if ((givenRow >= 0 && givenRow <= 2) && (givenCol >= 0 && givenCol <=2))
            {
                sudokuMap.startRow = 0;
                sudokuMap.startCol = 0;
            }
            else if ((givenRow >= 0 && givenRow <= 2) && (givenCol >= 3 && givenCol <= 5))
            {
                sudokuMap.startRow = 0;
                sudokuMap.startCol = 3;
            }
            else if ((givenRow >= 0 && givenRow <= 2) && (givenCol >= 6 && givenCol <= 8))
            {
                sudokuMap.startRow = 0;
                sudokuMap.startCol = 6;
            }
            else if ((givenRow >= 3 && givenRow <= 5) && (givenCol >= 0 && givenCol <= 2))
            {
                sudokuMap.startRow = 3;
                sudokuMap.startCol = 0;
            }
            else if ((givenRow >= 3 && givenRow <= 5) && (givenCol >= 3 && givenCol <= 5))
            {
                sudokuMap.startRow = 3;
                sudokuMap.startCol = 3;
            }
            else if ((givenRow >= 3 && givenRow <= 5) && (givenCol >= 6 && givenCol <= 8))
            {
                sudokuMap.startRow = 3;
                sudokuMap.startCol = 6;
            }
            else if ((givenRow >= 6 && givenRow <= 8) && (givenCol >= 0 && givenCol <= 2))
            {
                sudokuMap.startRow = 6;
                sudokuMap.startCol = 0;
            }
            else if ((givenRow >= 6 && givenRow <= 8) && (givenCol >= 3 && givenCol <= 5))
            {
                sudokuMap.startRow = 6;
                sudokuMap.startCol = 3;
            }
            else if ((givenRow >= 6 && givenRow <= 8) && (givenCol >= 6 && givenCol <= 8))
            {
                sudokuMap.startRow = 6;
                sudokuMap.startCol = 6;
            }

            return sudokuMap;
        }
    }
}
