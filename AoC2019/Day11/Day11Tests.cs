using NUnit.Framework;

namespace Day11
{
    [TestFixture]
    public class Tests
    {
        // Turn 0 = Left
        //  0 , -1 => -1,  0
        // -1 ,  0 =>  0, +1
        //  0 , +1 => +1,  0
        // +1 ,  0 =>  0, -1
        [Test]
        [TestCase(+0, -1, -1, +0)]
        [TestCase(-1, +0, +0, +1)]
        [TestCase(+0, +1, +1, +0)]
        [TestCase(+1, +0, +0, -1)]
        public void TurnRobotLeft(int dx, int dy, int expectedDX, int expectedDY)
        {
            Program.TurnRobot(0, ref dx, ref dy);
            Assert.That(dx, Is.EqualTo(expectedDX));
            Assert.That(dy, Is.EqualTo(expectedDY));
        }

        // Turn 1 = Right
        //  0 , -1 => +1,  0
        // +1 ,  0 =>  0, +1
        //  0 , +1 => -1,  0
        // -1 ,  0 =>  0, -1
        [Test]
        [TestCase(+0, -1, +1, +0)]
        [TestCase(+1, +0, +0, +1)]
        [TestCase(+0, +1, -1, +0)]
        [TestCase(-1, +0, +0, -1)]
        public void TurnRobotRight(int dx, int dy, int expectedDX, int expectedDY)
        {
            Program.TurnRobot(1, ref dx, ref dy);
            Assert.That(dx, Is.EqualTo(expectedDX));
            Assert.That(dy, Is.EqualTo(expectedDY));
        }
    }
}
