using SudokuSolver.Workers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Strategies
{
    internal class NakedPairsStrategy : ISudokuStrategy
    {
        private readonly SudokuMapper _sudokuMapper;
        public NakedPairsStrategy(SudokuMapper sudokuMapper)
        {
            _sudokuMapper = sudokuMapper;
        }

        public int[,] Solve(int[,] sudokuBoard)
        {
            for (int row = 0; row < sudokuBoard.GetLength(0); row++)
            {
                for (int col = 0; col < sudokuBoard.GetLength(1); col++)
                {
                    ELiminateNakedPairFromOthersInRow(sudokuBoard, row, col);
                    ELiminateNakedPairFromOthersInCol(sudokuBoard, row, col);
                    ELiminateNakedPairFromOthersInBlock(sudokuBoard, row, col);
                }
            }

            return sudokuBoard;
        }

        private void ELiminateNakedPairFromOthersInRow(int[,] sudokuBoard, int givenRow, int givenCol)
        {
            if (!HasNakedPairInRow(sudokuBoard, givenRow, givenCol)) return;
            for (int col = 0; col < sudokuBoard.GetLength(1); col++)
            {
                if (sudokuBoard[givenRow, col] != sudokuBoard[givenRow, givenCol] && sudokuBoard[givenRow, col].ToString().Length > 1)
                {
                    ELiminateNakedPair(sudokuBoard, sudokuBoard[givenRow, givenCol], givenRow, col);
                }
            }
        }


        private bool HasNakedPairInRow(int[,] sudokuBoard, int givenRow, int givenCol)
        {
            for (int col = 0; col < sudokuBoard.GetLength(1); col++)
            {
                if (givenCol != col && IsNakedPair(sudokuBoard[givenRow, col], sudokuBoard[givenRow, givenCol]))
                {
                    return true;
                }
            }
            return false;
        }


        private void ELiminateNakedPairFromOthersInCol(int[,] sudokuBoard, int givenRow, int givenCol)
        {
            if (!HasNakedPairInCol(sudokuBoard, givenRow, givenCol)) return;

            for (int row = 0; row < sudokuBoard.GetLength(0); row++)
            {
                if (sudokuBoard[row, givenCol] != sudokuBoard[givenRow, givenCol] && sudokuBoard[row, givenCol].ToString().Length > 1)
                {
                    ELiminateNakedPair(sudokuBoard, sudokuBoard[givenRow, givenCol], row, givenCol);
                }
            }

        }

        private bool HasNakedPairInCol(int[,] sudokuBoard, int givenRow, int givenCol)
        {
            for (int row = 0; row < sudokuBoard.GetLength(0); row++)
            {
                if (givenRow != row && IsNakedPair(sudokuBoard[row, givenCol], sudokuBoard[givenRow, givenCol]))
                {
                    return true;
                }
            }
            return false;
        }

        private void ELiminateNakedPairFromOthersInBlock(int[,] sudokuBoard, int givenRow, int givenCol)
        {
            if (!HasNakedPairInBlock(sudokuBoard, givenRow, givenCol)) return;

            var sudokuMap = _sudokuMapper.Find(givenRow, givenCol);

            for (int row = sudokuMap.startRow; row < sudokuMap.startRow + 3; row++)
            {
                for (int col = sudokuMap.startCol; col < sudokuMap.startCol+ 3; col++)
                {
                    if (sudokuBoard[row, col].ToString().Length > 1 && sudokuBoard[row, col] != sudokuBoard[givenRow, givenCol])
                    {
                        ELiminateNakedPair(sudokuBoard, sudokuBoard[givenRow, givenCol], row, col);
                    }
                }
            }

        }

        private bool HasNakedPairInBlock(int[,] sudokuBoard, int givenRow, int givenCol)
        {
            for (int row = 0; row < sudokuBoard.GetLength(0); row++)
            {
                for (int col = 0; col < sudokuBoard.GetLength(1); col++)
                {
                    var elementSame = givenRow == row && givenCol == col;
                    var elementInSameBlock =
                        _sudokuMapper.Find(givenRow, givenCol).startRow ==
                        _sudokuMapper.Find(row, col).startRow &&
                        _sudokuMapper.Find(givenRow, givenCol).startCol ==
                        _sudokuMapper.Find(row, col).startCol;

                    if (!elementSame && elementInSameBlock && IsNakedPair(sudokuBoard[givenRow, givenCol], sudokuBoard[row, col])) return true;
                }
            }
            return false;
        }

        private void ELiminateNakedPair(int[,] sudokuBoard, int valuesToEliminate, int eliminateFromRow, int eliminateFromCol)
        {
            var valuesToEliminateArray = valuesToEliminate.ToString().ToCharArray();
            foreach (var valueToEliminate in valuesToEliminateArray)
            {
                sudokuBoard[eliminateFromRow, eliminateFromCol] = Convert.ToInt32(sudokuBoard[eliminateFromRow, eliminateFromCol].ToString().Replace(valuesToEliminate.ToString(), string.Empty));
            }
        }

        private bool IsNakedPair(int firstPair, int secondPair)
        {
            return firstPair.ToString().Length == 2 && firstPair == secondPair;
        }
    }
}


