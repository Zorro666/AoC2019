using NUnit.Framework;

namespace Day03
{
    [TestFixture]
    public class Tests
    {
        [Test]
        [TestCase("R8,U5,L5,D3", "U7,R6,D4,L4", 6)]
        [TestCase("R75,D30,R83,U83,L12,D49,R71,U7,L72", "U62,R66,U55,R34,D71,R55,D58,R83", 159)]
        [TestCase("R98,U47,R26,D36,R33,U87,L62,D20,R33,U53,R51", "U98,R91,D20,R16,D67,R40,U7,R15,U6,R7", 135)]
        public void PlaceWireTestsClosestDistance(string path1, string path2, int distance)
        {
            Assume.That(Program.PlaceWire(1, path1).Item1, Is.Zero);
            Assert.That(Program.PlaceWire(2, path2).Item1, Is.EqualTo(distance));
        }

        [Test]
        [TestCase("R75,D30,R83,U83,L12,D49,R71,U7,L72", "U62,R66,U55,R34,D71,R55,D58,R83", 610)]
        [TestCase("R98, U47, R26, D63, R33, U87, L62, D20, R33, U53, R51", "U98,R91,D20,R16,D67,R40,U7,R15,U6,R7", 410)]
        public void PlaceWireTestsShortestSteps(string path1, string path2, int steps)
        {
            Assume.That(Program.PlaceWire(1, path1).Item1, Is.Zero);
            Assume.That(Program.PlaceWire(1, path1).Item2, Is.Zero);
            Assert.That(Program.PlaceWire(2, path2).Item2, Is.EqualTo(steps));
        }
    }
}
