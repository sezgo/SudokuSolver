using SudokuSolver.Workers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Strategies
{
    internal class LastFreeCellStrategy : ISudokuStrategy
    {
        public int[,] Solve(int[,] sudokuBoard)
        {
            new PossibilityCalculator(new SudokuMapper()).Calculate(sudokuBoard);
            return sudokuBoard;
        }
    }
}
