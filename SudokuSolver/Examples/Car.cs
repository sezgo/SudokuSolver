using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Examples
{
    abstract class Car : IDrivable
    {
        protected bool _on;
        public bool On
        {
            get
            {
                return _on;
            }
        }
        public Car()
        {
            _on = false;
        }

        public void TurnOnOff()
        {
            _on = !_on;
        }

        public abstract bool Drive();

    }
}
