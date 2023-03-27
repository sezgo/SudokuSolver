using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Examples
{
    internal class Person
    {
        private IDrivable _car;

        public Person(IDrivable car)
        {
            _car = car;
        }

        public bool Drive()
        {
            _car.TurnOnOff();
            return _car.Drive();
        }
    }
}
