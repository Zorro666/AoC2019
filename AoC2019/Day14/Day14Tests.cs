using System;
using System.Collections;
using NUnit.Framework;

namespace Day14
{
    [TestFixture]
    public class Tests
    {
        static readonly string[] inputA = {
"10 ORE => 10 A",
"1 ORE => 1 B",
"7 A, 1 B => 1 C",
"7 A, 1 C => 1 D",
"7 A, 1 D => 1 E",
"7 A, 1 E => 1 FUEL"
        };
        static readonly string[] inputB = {
"9 ORE => 2 A",
"8 ORE => 3 B",
"7 ORE => 5 C",
"3 A, 4 B => 1 AB",
"5 B, 7 C => 1 BC",
"4 C, 1 A => 1 CA",
"2 AB, 3 BC, 4 CA => 1 FUEL"
        };
        static readonly string[] inputC = {
"157 ORE => 5 NZVS",
"165 ORE => 6 DCFZ",
"44 XJWVT, 5 KHKGT, 1 QDVJ, 29 NZVS, 9 GPVTF, 48 HKGWZ => 1 FUEL",
"12 HKGWZ, 1 GPVTF, 8 PSHF => 9 QDVJ",
"179 ORE => 7 PSHF",
"177 ORE => 5 HKGWZ",
"7 DCFZ, 7 PSHF => 2 XJWVT",
"165 ORE => 2 GPVTF",
"3 DCFZ, 7 NZVS, 5 HKGWZ, 10 PSHF => 8 KHKGT"
        };
        static readonly string[] inputD = {
"2 VPVL, 7 FWMGM, 2 " +
                "CXFTF, 11 MNCFX => 1 STKFG",
"17 NVRVD, 3 JNWZP => 8 VPVL",
"53 STKFG, 6 MNCFX, 46 VJHF, 81 HVMC, 68 CXFTF, 25 GNMV => 1 FUEL",
"22 VJHF, 37 MNCFX => 5 FWMGM",
"139 ORE => 4 NVRVD",
"144 ORE => 7 JNWZP",
"5 MNCFX, 7 RFSQX, 2 FWMGM, 2 VPVL, 19 CXFTF => 3 HVMC",
"5 VJHF, 7 MNCFX, 9 VPVL, 37 CXFTF => 6 GNMV",
"145 ORE => 6 MNCFX",
"1 NVRVD => 8 CXFTF",
"1 VJHF, 6 MNCFX => 4 RFSQX",
"176 ORE => 6 VJHF"
        };
        static readonly string[] inputE = {
"171 ORE => 8 CNZTR",
"7 ZLQW, 3 BMBT, 9 XCVML, 26 XMNCP, 1 WPTQ, 2 MZWV, 1 RJRHP => 4 PLWSL",
"114 ORE => 4 BHXH",
"14 VRPVC => 6 BMBT",
"6 BHXH, 18 KTJDG, 12 WPTQ, 7 PLWSL, 31 FHTLT, 37 ZDVW => 1 FUEL",
"6 WPTQ, 2 BMBT, 8 ZLQW, 18 KTJDG, 1 XMNCP, 6 MZWV, 1 RJRHP => 6 FHTLT",
"15 XDBXC, 2 LTCX, 1 VRPVC => 6 ZLQW",
"13 WPTQ, 10 LTCX, 3 RJRHP, 14 XMNCP, 2 MZWV, 1 ZLQW => 1 ZDVW",
"5 BMBT => 4 WPTQ",
"189 ORE => 9 KTJDG",
"1 MZWV, 17 XDBXC, 3 XCVML => 2 XMNCP",
"12 VRPVC, 27 CNZTR => 2 XDBXC",
"15 KTJDG, 12 BHXH => 5 XCVML",
"3 BHXH, 2 VRPVC => 7 MZWV",
"121 ORE => 7 VRPVC",
"7 XCVML => 6 RJRHP",
"5 BHXH, 4 VRPVC => 5 LTCX",
        };

        private static IEnumerable ParseInputCases => new[]
        {
            new TestCaseData(inputA, "A", "ORE").SetName("ParseInput.A A = 1 ORE").Returns(1.0),
            new TestCaseData(inputA, "B", "ORE").SetName("ParseInput.A B = 1 ORE").Returns(1.0),
            new TestCaseData(inputA, "C", "A").SetName("ParseInput.A C = 7 A").Returns(7.0),
            new TestCaseData(inputA, "C", "B").SetName("ParseInput.A C = 1 B").Returns(1.0),
            new TestCaseData(inputA, "D", "A").SetName("ParseInput.A D = 7 A").Returns(7.0),
            new TestCaseData(inputA, "D", "C").SetName("ParseInput.A D = 1 C").Returns(1.0),
            new TestCaseData(inputA, "E", "A").SetName("ParseInput.A E = 7 A").Returns(7.0),
            new TestCaseData(inputA, "E", "D").SetName("ParseInput.A E = 1 D").Returns(1.0),
            new TestCaseData(inputA, "FUEL", "A").SetName("ParseInput.A FUEL = 7 A").Returns(7.0),
            new TestCaseData(inputA, "FUEL", "E").SetName("ParseInput.A FUEL = 1 E").Returns(1.0),
            new TestCaseData(inputB, "FUEL", "AB").SetName("ParseInput.B FUEL = 2 AB").Returns(2.0),
            new TestCaseData(inputB, "FUEL", "BC").SetName("ParseInput.B FUEL = 3 BC").Returns(3.0),
            new TestCaseData(inputB, "FUEL", "CA").SetName("ParseInput.B FUEL = 4 CA").Returns(4.0),
        };

        [Test]
        [TestCaseSource("ParseInputCases")]
        public double ParseInput(string[] lines, string compoundOutput, string compoundInput)
        {
            Program.ParseInput(lines);
            return Program.CompoundCost(compoundOutput, compoundInput);
        }

        public static IEnumerable CountCompoundDepthCases
        {
            get
            {
                yield return new TestCaseData(inputA, "A").SetName("CountCompoundDepth.A A = 1").Returns(1);
                yield return new TestCaseData(inputA, "B").SetName("CountCompoundDepth.A B = 1").Returns(1);
                yield return new TestCaseData(inputA, "C").SetName("CountCompoundDepth.A C = 2").Returns(2);
                yield return new TestCaseData(inputA, "D").SetName("CountCompoundDepth.A D = 3").Returns(3);
                yield return new TestCaseData(inputA, "E").SetName("CountCompoundDepth.A E = 4").Returns(4);
                yield return new TestCaseData(inputA, "FUEL").SetName("CountCompoundDepth.A FUEL = 5").Returns(5);
                yield return new TestCaseData(inputB, "A").SetName("CountCompoundDepth.B A = 1").Returns(1);
                yield return new TestCaseData(inputB, "B").SetName("CountCompoundDepth.B B = 1").Returns(1);
                yield return new TestCaseData(inputB, "C").SetName("CountCompoundDepth.B C = 1").Returns(1);
                yield return new TestCaseData(inputB, "AB").SetName("CountCompoundDepth.B AB = 2").Returns(2);
                yield return new TestCaseData(inputB, "BC").SetName("CountCompoundDepth.B BC = 2").Returns(2);
                yield return new TestCaseData(inputB, "CA").SetName("CountCompoundDepth.B CA = 2").Returns(2);
                yield return new TestCaseData(inputB, "FUEL").SetName("CountCompoundDepth.B FUEL = 3").Returns(3);
                yield return new TestCaseData(inputC, "NZVS").SetName("CountCompoundDepth.C NZVS = 1").Returns(1);
                yield return new TestCaseData(inputC, "DCFZ").SetName("CountCompoundDepth.C DCFZ = 1").Returns(1);
                yield return new TestCaseData(inputC, "GPVTF").SetName("CountCompoundDepth.C GPVTF = 1").Returns(1);
                yield return new TestCaseData(inputC, "PSHF").SetName("CountCompoundDepth.C PSHF = 1").Returns(1);
                yield return new TestCaseData(inputC, "HKGWZ").SetName("CountCompoundDepth.C HKGWZ = 1").Returns(1);
                yield return new TestCaseData(inputC, "QDVJ").SetName("CountCompoundDepth.C QDVJ = 2").Returns(2);
                yield return new TestCaseData(inputC, "XJWVT").SetName("CountCompoundDepth.C XJWVT = 2").Returns(2);
                yield return new TestCaseData(inputC, "KHKGT").SetName("CountCompoundDepth.C KHKGT = 2").Returns(2);
                yield return new TestCaseData(inputC, "FUEL").SetName("CountCompoundDepth.C FUEL = 3").Returns(3);
                yield return new TestCaseData(inputD, "NVRVD").SetName("CountCompoundDepth.D NVRV = 1").Returns(1);
                yield return new TestCaseData(inputD, "JNWZP").SetName("CountCompoundDepth.D JNWZP = 1").Returns(1);
                yield return new TestCaseData(inputD, "MNCFX").SetName("CountCompoundDepth.D MNCFX = 1").Returns(1);
                yield return new TestCaseData(inputD, "VJHF").SetName("CountCompoundDepth.D VJHF = 1").Returns(1);
                yield return new TestCaseData(inputD, "CXFTF").SetName("CountCompoundDepth.D CXFTF = 2").Returns(2);
                yield return new TestCaseData(inputD, "VPVL").SetName("CountCompoundDepth.D VPVL = 2").Returns(2);
                yield return new TestCaseData(inputD, "FWMGM").SetName("CountCompoundDepth.D FWMGM = 2").Returns(2);
                yield return new TestCaseData(inputD, "RFSQX").SetName("CountCompoundDepth.D RFSQX = 2").Returns(2);
                yield return new TestCaseData(inputD, "GNMV").SetName("CountCompoundDepth.D GNMV = 3").Returns(3);
                yield return new TestCaseData(inputD, "STKFG").SetName("CountCompoundDepth.D STKFG = 3").Returns(3);
                yield return new TestCaseData(inputD, "HVMC").SetName("CountCompoundDepth.D HVMC = 3").Returns(3);
                yield return new TestCaseData(inputD, "FUEL").SetName("CountCompoundDepth.D FUEL = 4").Returns(4);
            }
        }

        [Test]
        [TestCaseSource("CountCompoundDepthCases")]
        public int CountCompoundDepth(string[] lines, string compoundName)
        {
            Program.ParseInput(lines);
            return Program.CountCompoundDepth(compoundName);
        }

        public static IEnumerable VerifySortCases
        {
            get
            {
                yield return new TestCaseData(inputA, true).SetName("VerifySort.A");
                yield return new TestCaseData(inputB, true).SetName("VerifySort.B");
                yield return new TestCaseData(inputC, true).SetName("VerifySort.C");
                yield return new TestCaseData(inputD, true).SetName("VerifySort.D");
                yield return new TestCaseData(inputE, true).SetName("VerifySort.E");
            }
        }

        [Test]
        [TestCaseSource("VerifySortCases")]
        public void VerifySort(string[] lines, bool expected)
        {
            Program.ParseInput(lines);
            Assert.That(Program.VerifySort(), Is.EqualTo(expected));
        }

        public static IEnumerable MakeFuelCases
        {
            get
            {
                yield return new TestCaseData(inputA, 31).SetName("MakeFuel.A 31");
                yield return new TestCaseData(inputB, 165).SetName("MakeFuel.B 165");
                yield return new TestCaseData(inputC, 13312).SetName("MakeFuel.C 13312");
                yield return new TestCaseData(inputD, 180697).SetName("MakeFuel.D 180697");
                yield return new TestCaseData(inputE, 2210736).SetName("MakeFuel.E 2210736");
            }
        }
        [Test]
        [TestCaseSource("MakeFuelCases")]
        public void MakeFuel(string[] lines, Int64 expected)
        {
            Program.ParseInput(lines);
            Assert.That(Program.MakeFuel(), Is.EqualTo(expected));
        }

        public static IEnumerable FuelProducedCases
        {
            get
            {
                //yield return new TestCaseData(inputA, 1000000000000L).SetName("FuelProduced.A FUEL = 31").Returns(31);
                //yield return new TestCaseData(inputB, 1000000000000L).SetName("FuelProduced.B FUEL = 165").Returns(165);
                yield return new TestCaseData(inputC, 1000000000000L).SetName("FuelProduced.C FUEL = 82892753").Returns(82892753);
                yield return new TestCaseData(inputD, 1000000000000L).SetName("FuelProduced.D FUEL = 5586022").Returns(5586022);
                yield return new TestCaseData(inputE, 1000000000000L).SetName("FuelProduced.E FUEL = 460664").Returns(460664);
            }
        }

        [Test]
        [TestCaseSource("FuelProducedCases")]
        public Int64 FuelProduced(string[] lines, Int64 inputOre)
        {
            Program.ParseInput(lines);
            return Program.FuelProduced(inputOre);
        }

        public static IEnumerable FuelProducedSlowCases
        {
            get
            {
                yield return new TestCaseData(inputA, 321L).SetName("FuelProducedSlow.A 321 FUEL = 311").Returns(11);
                yield return new TestCaseData(inputA, 10000L).SetName("FuelProducedSlow.A 10000 FUEL = 344").Returns(344);
                yield return new TestCaseData(inputB, 1000000L).SetName("FuelProducedSlow.B FUEL = 6323").Returns(6323);
                yield return new TestCaseData(inputC, 10000000L).SetName("FuelProducedSlow.C FUEL = 828").Returns(828);
                yield return new TestCaseData(inputD, 10000000L).SetName("FuelProducedSlow.D FUEL = 55").Returns(55);
                yield return new TestCaseData(inputE, 10000000L).SetName("FuelProducedSlow.E FUEL = 4").Returns(4);
            }
        }

        [Test]
        [TestCaseSource("FuelProducedSlowCases")]
        public Int64 FuelProducedSlow(string[] lines, Int64 inputOre)
        {
            Program.ParseInput(lines);
            var expectedFuelMade = Program.FuelProducedSlow(inputOre);
            Program.ParseInput(lines);
            Assert.That(Program.FuelProduced(inputOre), Is.EqualTo(expectedFuelMade));
            return expectedFuelMade;
        }
    }
}
