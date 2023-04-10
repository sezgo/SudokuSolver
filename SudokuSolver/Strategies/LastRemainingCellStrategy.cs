using SudokuSolver.Test.Unit.Strategies;
using SudokuSolver.Workers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Strategies
{
    internal class LastRemainingCellStrategy : ISudokuStrategy
    {
        public int[,] Solve(int[,] sudokuBoard)
        {
            new NoteWriter(new SudokuMapper()).UpdateNotes(sudokuBoard);
            new HiddenSinglesStrategy(new SudokuMapper()).Solve(sudokuBoard);

            return sudokuBoard;
        }
        

    }
}
