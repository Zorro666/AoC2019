using NUnit.Framework;

namespace Day04
{
    [TestFixture]
    public class Tests
    {
        [Test]
        [TestCase(1111111, true)]
        [TestCase(223450, false)]
        [TestCase(123789, false)]
        [TestCase(123788, true)]
        public void ValidPasswordPart1(int password, bool expected)
        {
            Assert.That(Program.ValidPassword(password, false), Is.EqualTo(expected));
        }

        [Test]
        [TestCase(1111111, false)]
        [TestCase(223450, false)]
        [TestCase(123789, false)]
        [TestCase(123788, true)]
        [TestCase(112233, true)]
        [TestCase(123444, false)]
        [TestCase(111122, true)]
        public void ValidPasswordPart2(int password, bool expected)
        {
            Assert.That(Program.ValidPassword(password, true), Is.EqualTo(expected));
        }
    }
}
