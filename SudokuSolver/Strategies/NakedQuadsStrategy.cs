using SudokuSolver.Workers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Strategies
{
    internal class NakedQuadsStrategy : ISudokuStrategy
    {
        private readonly SudokuMapper _sudokuMapper;
        public NakedQuadsStrategy(SudokuMapper sudokuMapper)
        {
            _sudokuMapper = sudokuMapper;
        }
        public int[,] Solve(int[,] sudokuBoard)
        {
            for (int col = 0; col < sudokuBoard.GetLength(0); col++)
            {
                for (int row = 0; row < sudokuBoard.GetLength(1); row++)
                {
                    SolveNakedQuadForInRow(sudokuBoard, row, col);
                }
            }
            return sudokuBoard;
        }

        private void SolveNakedQuadForInRow(int[,] sudokuBoard, int givenRow, int givenCol)
        {
            var nakedQuad = HasNakedQuadInRow(sudokuBoard, givenRow, givenCol);
            if (!nakedQuad.IsNakedQuad) return;
            for (int col = 0; col < sudokuBoard.GetLength(1); col++)
            {
                var cell = sudokuBoard[givenRow, col];
                if (nakedQuad.First != cell && nakedQuad.Second != cell && nakedQuad.Third != cell && nakedQuad.Fourth != cell)
                {
                    var strValuesToEliminate = nakedQuad.First.ToString() + nakedQuad.Second.ToString() + nakedQuad.Third.ToString() + nakedQuad.Fourth.ToString();
                    EliminateNakedQuad(sudokuBoard, strValuesToEliminate, givenRow, col);
                }
            }
        }

        internal (int First, int Second, int Third, int Fourth, bool IsNakedQuad) HasNakedQuadInRow(int[,] sudokuBoard, int givenRow, int givenCol)
        {
            for (int col2 = 0; col2 < sudokuBoard.GetLength(1); col2++)
            {
                for (int col3 = 0; col3 < sudokuBoard.GetLength(1); col3++)
                {
                    for (int col4 = 0; col4 < sudokuBoard.GetLength(1); col4++)
                    {
                        if (new HashSet<int>(new int[] {givenCol, col2, col3, col4}).Count == 4 &&
                            IsNakedQuad(sudokuBoard[givenRow, givenCol], sudokuBoard[givenRow, col2], sudokuBoard[givenRow, col3], sudokuBoard[givenRow, col4]))
                        {
                            return (
                                First: sudokuBoard[givenRow, givenCol],
                                Second: sudokuBoard[givenRow, col2],
                                Third: sudokuBoard[givenRow, col3],
                                Fourth: sudokuBoard[givenRow, col4],
                                IsNakedQuad: true
                            );
                        }
                    }
                }
            }
            return (-1, -1, -1, -1, false);
        }


        private void EliminateNakedQuad(int[,] sudokuBoard, string strValuesToEliminate, int eliminateFromRow, int eliminateFromCol)
        {
            var valuesToEliminateSet = strValuesToEliminate.ToHashSet();
            foreach (var valueToEliminate in valuesToEliminateSet)
            {
                var cell = sudokuBoard[eliminateFromRow, eliminateFromCol];
                var strCell = cell.ToString();
                strCell = strCell.Replace(valueToEliminate.ToString(), string.Empty);
                sudokuBoard[eliminateFromRow, eliminateFromCol] = Convert.ToInt32(strCell);
            }
        }


        internal bool IsNakedQuad(int firstNaked, int secondNaked, int thirdNaked, int fourthNaked)
        {

            string strFirstNaked = firstNaked.ToString();
            string strSecondNaked = secondNaked.ToString();
            string strThirdNaked = thirdNaked.ToString();
            string strFourthNaked = fourthNaked.ToString();

            if (strFirstNaked.Length == 1 || strSecondNaked.Length == 1 || strThirdNaked.Length == 1 || strFourthNaked.Length == 1)
                return false;

            HashSet<char> set = new HashSet<char>();
            set.UnionWith(strFirstNaked);
            set.UnionWith(strSecondNaked);
            set.UnionWith(strThirdNaked);
            set.UnionWith(strFourthNaked);

            return set.Count == 4;
        }
    }
}
