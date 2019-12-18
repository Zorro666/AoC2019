using System;
using NUnit.Framework;

namespace Day09
{
    [TestFixture]
    public class Tests
    {
        [Test]
        [TestCase("1102,34915192,34915192,7,4,7,99,0", 16)]
        public void RunProgramPart1LargeNumberOutput(string source, int expectedLength)
        {
            string allOutput = "";
            Assert.That(Program.RunProgram(source, 0, ref allOutput).ToString().Length, Is.EqualTo(expectedLength));
        }

        [Test]
        [TestCase("104,1125899906842624,99", 1125899906842624L)]
        [TestCase("1102,34915192,34915192,7,4,7,99,0", (34915192L * 34915192L))]
        public void RunProgramPart1ExpectedOutput(string source, Int64 expected)
        {
            string allOutput = "";
            Assert.That(Program.RunProgram(source, 0, ref allOutput), Is.EqualTo(expected));
        }

        [Test]
        [TestCase("109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99")]
        public void RunProgramPart1ExpectedStringOutput(string source)
        {
            string allOutput = "";
            var expected = source;
            Program.RunProgram(source, 0, ref allOutput);
            Assert.That(allOutput, Is.EqualTo(expected));
        }
    }
}
