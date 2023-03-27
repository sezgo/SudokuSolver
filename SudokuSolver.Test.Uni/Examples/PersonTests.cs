using SudokuSolver.Examples;

namespace SudokuSolver.Test.Unit.Examples
{
    [TestClass]
    public class PersonTests
    {
        [TestMethod]
        public void PersonShouldDrive()
        {
            Car car;
            car = new Ferrari();
            Person person = new Person(car);
            Assert.IsTrue(person.Drive());

            car = new Lamborghini();
            person = new Person(car);
            Assert.IsTrue(person.Drive());

        }
    }
}
