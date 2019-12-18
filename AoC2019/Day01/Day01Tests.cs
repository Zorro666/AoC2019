using NUnit.Framework;

namespace Day01
{
    [TestFixture]
    public class Tests
    {
        [Test]
        [TestCase(12, 2)]
        [TestCase(14, 2)]
        [TestCase(1969, 654)]
        [TestCase(100756, 33583)]
        public void ComputeFuelMatchesExamples(int mass, int expected)
        {
            Assert.That(Program.ComputeFuel(mass), Is.EqualTo(expected));
        }

        [Test]
        [TestCase(12, 2)]
        [TestCase(14, 2)]
        [TestCase(1969, 966)]
        [TestCase(100756, 50346)]
        public void ComputeFuelRecursiveMatchesExamples(int mass, int expected)
        {
            Assert.That(Program.ComputeFuelRecursive(mass), Is.EqualTo(expected));
        }
    }
}
