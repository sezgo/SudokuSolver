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
        private readonly List<SudokuMap> mapList = new List<SudokuMap>
        {
            new SudokuMap
            {
                StartRow = 0,
                StartCol = 0
            },
            new SudokuMap
            {
                StartRow = 0,
                StartCol = 3
            },
            new SudokuMap
            {
                StartRow = 0,
                StartCol = 6
            },
            new SudokuMap
            {
                StartRow = 3,
                StartCol = 0
            },
            new SudokuMap
            {
                StartRow = 3,
                StartCol = 3
            },
            new SudokuMap
            {
                StartRow = 3,
                StartCol = 6
            },
            new SudokuMap
            {
                StartRow = 6,
                StartCol = 0
            },
            new SudokuMap
            {
                StartRow = 6,
                StartCol = 3
            },
            new SudokuMap
            {
                StartRow = 6,
                StartCol = 6
            },
        };

        /// <summary>
        /// From a given block index info finds the block map.
        /// </summary>
        /// <param name="givenBlockIndex">The given block index.</param>
        /// <returns>
        /// A Sudoku Map object which contains the start row and the column for the block.
        /// </returns>
        public SudokuMap Find(int givenBlockIndex)
        {   
            return mapList[givenBlockIndex];
        }
        /// <summary>
        /// From a given row a given column info finds the block map.
        /// </summary>
        /// <param name="givenRow">The given row</param>
        /// <param name="givenCol">The given column</param>
        /// <returns>
        /// A Sudoku Map object which contains the start row and the column for the block.
        /// </returns>
        public SudokuMap Find(int givenRow, int givenCol)
        {
            SudokuMap sudokuMap = new SudokuMap();

            if ((givenRow >= 0 && givenRow <= 2) && (givenCol >= 0 && givenCol <=2))
            {
                sudokuMap.StartRow = 0;
                sudokuMap.StartCol = 0;
            }
            else if ((givenRow >= 0 && givenRow <= 2) && (givenCol >= 3 && givenCol <= 5))
            {
                sudokuMap.StartRow = 0;
                sudokuMap.StartCol = 3;
            }
            else if ((givenRow >= 0 && givenRow <= 2) && (givenCol >= 6 && givenCol <= 8))
            {
                sudokuMap.StartRow = 0;
                sudokuMap.StartCol = 6;
            }
            else if ((givenRow >= 3 && givenRow <= 5) && (givenCol >= 0 && givenCol <= 2))
            {
                sudokuMap.StartRow = 3;
                sudokuMap.StartCol = 0;
            }
            else if ((givenRow >= 3 && givenRow <= 5) && (givenCol >= 3 && givenCol <= 5))
            {
                sudokuMap.StartRow = 3;
                sudokuMap.StartCol = 3;
            }
            else if ((givenRow >= 3 && givenRow <= 5) && (givenCol >= 6 && givenCol <= 8))
            {
                sudokuMap.StartRow = 3;
                sudokuMap.StartCol = 6;
            }
            else if ((givenRow >= 6 && givenRow <= 8) && (givenCol >= 0 && givenCol <= 2))
            {
                sudokuMap.StartRow = 6;
                sudokuMap.StartCol = 0;
            }
            else if ((givenRow >= 6 && givenRow <= 8) && (givenCol >= 3 && givenCol <= 5))
            {
                sudokuMap.StartRow = 6;
                sudokuMap.StartCol = 3;
            }
            else if ((givenRow >= 6 && givenRow <= 8) && (givenCol >= 6 && givenCol <= 8))
            {
                sudokuMap.StartRow = 6;
                sudokuMap.StartCol = 6;
            }

            return sudokuMap;
        }
        
        /// <summary>
        /// Calculates a cellIndex in range 0-9 for block cell.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns>Cell index.</returns>
        public int GetCellIndex(int row, int col)
        {
            var map = Find(row, col);
            var cellIndex = (row - map.StartRow) * 3 + ((col - map.StartCol));

            return cellIndex;
        }

        /// <summary>
        /// Calculates the cell row from a given cell index value.
        /// </summary>
        /// <param name="cellIndex">The index given to a block cell.</param>
        /// <param name="map">Sudoku Map object which contains the start row and the column for the block. </param>
        /// <returns>The row of the cell with the given cell index.</returns>
        public int GetCellRow(int cellIndex, SudokuMap map)
        {
            return (cellIndex / 3) + map.StartRow;
        }

        /// <summary>
        /// Calculates the cell column from a given cell index value.
        /// </summary>
        /// <param name="cellIndex">The index given to a block cell.</param>
        /// <param name="map">Sudoku Map object which contains the start row and the column for the block. </param>
        /// <returns>The column of the cell with the given cell index.</returns>
        public int GetCellCol(int cellIndex, SudokuMap map)
        {
            return (cellIndex % 3) + map.StartCol;
        }
    }
}
