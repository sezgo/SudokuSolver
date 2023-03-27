using SudokuSolver.Examples;

namespace SudokuSolver.Test.Unit.Examples
{
    [TestClass]
    public class FerrariTests
    {
        [TestMethod]
        public void CarShouldToggleFromOffToOn()
        {

            Car ferrari = new Ferrari();
            Assert.IsFalse(ferrari.On);
            ferrari.TurnOnOff();
            Assert.IsTrue(ferrari.On);
        }
        [TestMethod]
        public void CarShouldNotDriveWhenOff()
        {
            Car car = new Ferrari();
            Assert.IsFalse(car.Drive());
        }
        [TestMethod]
        public void CarShouldDrive()
        {
            Car ferrari = new Ferrari();
            ferrari.TurnOnOff();
            Assert.IsTrue(ferrari.Drive());
        }
    }
}