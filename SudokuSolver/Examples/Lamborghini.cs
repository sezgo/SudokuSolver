using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Examples
{
    internal class Lamborghini : Car
    {
        public override bool Drive()
        {
            if (_on) return true; // Driving
            else return false; // cant drive its off
        }
    }
}
