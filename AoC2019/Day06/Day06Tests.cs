using NUnit.Framework;

namespace Day06
{
    [TestFixture]
    public class Tests
    {
        [Test]
        [TestCase("COM", "B")]
        public void AddNodeSetsParent(string parent, string node)
        {
            Program.InitNodes();
            Program.AddNode(node, parent);
            Assert.That(Program.GetParent(node), Is.EqualTo(parent));
        }

        [Test]
        [TestCase(new string[] { "COM)B", "B)C" }, "B", "COM", TestName = "LayoutSetParents(B)")]
        [TestCase(new string[] { "COM)B", "B)C" }, "C", "B", TestName = "LayoutSetParents(C")]
        [TestCase(new string[] { "COM)B", "B)C", "C)D", "D)E", "E)F" }, "D", "C", TestName = "LayoutSetParents(D)")]
        [TestCase(new string[] { "COM)B", "B)C", "C)D", "D)E", "E)F" }, "E", "D", TestName = "LayoutSetParents(E)")]
        [TestCase(new string[] { "COM)B", "B)C", "C)D", "D)E", "E)F" }, "F", "E", TestName = "LayoutSetParents(F)")]
        [TestCase(new string[] { "B)C", "COM)B" }, "B", "COM")]
        public void LayoutSetParents(string[] elements, string node, string parent)
        {
            Program.ParseLayout(elements);
            Assert.That(Program.GetParent(node), Is.EqualTo(parent));
        }

        [Test]
        [TestCase(new string[] { "COM)B", "B)C" }, 2)]
        [TestCase(new string[] { "COM)B", "B)C", "C)D", "D)E", "E)F" }, 5)]
        public void DirectOrbits(string[] elements, int expected)
        {
            Program.ParseLayout(elements);
            Assert.That(Program.DirectOrbits(), Is.EqualTo(expected));
        }

        [Test]
        [TestCase(new string[] { "COM)B", "B)C" }, 1)]
        [TestCase(new string[] { "COM)B", "B)C", "C)D", "D)E", "E)F" }, 10)]
        public void InDirectOrbits(string[] elements, int expected)
        {
            Program.ParseLayout(elements);
            Assert.That(Program.InDirectOrbits(), Is.EqualTo(expected));
        }

        [Test]
        [TestCase(new string[] { "COM)B", "B)C", "C)D", "D)E", "E)F", "B)G", "G)H", "D)I", "E)J", "J)K", "K)L" }, 42, TestName = "TotalOrbits(42)")]
        public void TotalOrbits(string[] elements, int expected)
        {
            Program.ParseLayout(elements);
            Assert.That(Program.TotalOrbits(), Is.EqualTo(expected));
        }

        [Test]
        [TestCase(new string[] { "COM)B", "B)C" }, "C", new string[] { "B", "COM" })]
        [TestCase(new string[] { "COM)B", "B)C" }, "B", new string[] { "COM" })]
        [TestCase(new string[] { "COM)B", "B)C", "C)D", "D)E", "E)F" }, "D", new string[] { "C", "B", "COM" })]
        [TestCase(new string[] { "COM)B", "B)C", "C)D", "D)E", "E)F" }, "E", new string[] { "D", "C", "B", "COM" })]
        [TestCase(new string[] { "COM)B", "B)C", "C)D", "D)E", "E)F" }, "F", new string[] { "E", "D", "C", "B", "COM" })]
        public void GetParents(string[] elements, string node, string[] expected)
        {
            Program.ParseLayout(elements);
            Assert.That(Program.GetParents(node), Is.EqualTo(expected));
        }

        [Test]
        [TestCase(new string[] { "COM)B", "B)C", "C)D", "D)E", "E)F", "B)G", "G)H", "D)I", "E)J", "J)K", "K)L", "K)YOU", "I)SAN" }, "YOU", "SAN", 4, TestName = "NumTransfers(4)")]
        public void NumTransfers(string[] elements, string start, string end, int expected)
        {
            Program.ParseLayout(elements);
            Assert.That(Program.NumTransfers(start, end), Is.EqualTo(expected));
        }
    }
}
