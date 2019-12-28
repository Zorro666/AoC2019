using System;
using System.Collections.Generic;
using System.IO;

/*

--- Day 14: Space Stoichiometry ---

As you approach the rings of Saturn, your ship's low fuel indicator turns on. There isn't any fuel here, but the rings have plenty of raw material. Perhaps your ship's Inter-Stellar Refinery Union brand nanofactory can turn these raw materials into fuel.

You ask the nanofactory to produce a list of the reactions it can perform that are relevant to this process (your puzzle input). Every reaction turns some quantities of specific input chemicals into some quantity of an output chemical. Almost every chemical is produced by exactly one reaction; the only exception, ORE, is the raw material input to the entire process and is not produced by a reaction.

You just need to know how much ORE you'll need to collect before you can produce one unit of FUEL.

Each reaction gives specific quantities for its inputs and output; reactions cannot be partially run, so only whole integer multiples of these quantities can be used. (It's okay to have leftover chemicals when you're done, though.) For example, the reaction 1 A, 2 B, 3 C => 2 D means that exactly 2 units of chemical D can be produced by consuming exactly 1 A, 2 B and 3 C. You can run the full reaction as many times as necessary; for example, you could produce 10 D by consuming 5 A, 10 B, and 15 C.

Suppose your nanofactory produces the following list of reactions:

1: 10 ORE => 10 A
1: 1 ORE => 1 B
2: 7 A, 1 B => 1 C
3: 7 A, 1 C => 1 D
4: 7 A, 1 D => 1 E
5: 7 A, 1 E => 1 FUEL
The first two reactions use only ORE as inputs; they indicate that you can produce as much of chemical A as you want (in increments of 10 units, each 10 costing 10 ORE) and as much of chemical B as you want (each costing 1 ORE). To produce 1 FUEL, a total of 31 ORE is required: 1 ORE to produce 1 B, then 30 more ORE to produce the 7 + 7 + 7 + 7 = 28 A (with 2 extra A wasted) required in the reactions to convert the B into C, C into D, D into E, and finally E into FUEL. (30 A is produced because its reaction requires that it is created in increments of 10.)

Or, suppose you have the following list of reactions:

1: 9 ORE => 2 A
1: 8 ORE => 3 B
1: 7 ORE => 5 C
2: 3 A, 4 B => 1 AB
2: 5 B, 7 C => 1 BC
2: 4 C, 1 A => 1 CA
3: 2 AB, 3 BC, 4 CA => 1 FUEL
The above list of reactions requires 165 ORE to produce 1 FUEL:

Consume 45 ORE to produce 10 A.
Consume 64 ORE to produce 24 B.
Consume 56 ORE to produce 40 C.
Consume 6 A, 8 B to produce 2 AB.
Consume 15 B, 21 C to produce 3 BC.
Consume 16 C, 4 A to produce 4 CA.
Consume 2 AB, 3 BC, 4 CA to produce 1 FUEL.
Here are some larger examples:

13312 ORE for 1 FUEL:
1: 157 ORE => 5 NZVS
1: 165 ORE => 6 DCFZ
1: 165 ORE => 2 GPVTF
1: 179 ORE => 7 PSHF
1: 177 ORE => 5 HKGWZ
2: 12 HKGWZ, 1 GPVTF, 8 PSHF => 9 QDVJ
2: 7 DCFZ, 7 PSHF => 2 XJWVT
2: 3 DCFZ, 7 NZVS, 5 HKGWZ, 10 PSHF => 8 KHKGT
3: 44 XJWVT, 5 KHKGT, 1 QDVJ, 29 NZVS, 9 GPVTF, 48 HKGWZ => 1 FUEL

154 * DCFZ + 154 * PSHF
3 * DCFZ + 7 * NZVS + 5 * HKGWZ + 10 * PSHF 
12 * HKGWZ + 1 * GPVTF + 8 * PSHF 
29 * NZVS + 9 * GPVTF + 48 * HKGWZ

157 * DCFZ = 27 * 165 = 4455
172 * PSHF = 25 * 179 = 4475
36 * NZVS = 8 * 157 =  1256
65 * HKGWZ = 13 * 177 = 2301
10 * GPVTF = 5 * 165 = 825
4455 + 4475 + 1256 + 2301 + 825 = 

180697 ORE for 1 FUEL:
1 : 139 ORE => 4 NVRVD
1 : 144 ORE => 7 JNWZP
1 : 145 ORE => 6 MNCFX
1 : 176 ORE => 6 VJHF
2: 1 NVRVD => 8 CXFTF
2: 17 NVRVD, 3 JNWZP => 8 VPVL
2: 22 VJHF, 37 MNCFX => 5 FWMGM
2: 1 VJHF, 6 MNCFX => 4 RFSQX
3: 5 VJHF, 7 MNCFX, 9 VPVL, 37 CXFTF => 6 GNMV
3: 3 VPVL, 7 FWMGM, 2 CXFTF, 11 MNCFX => 1 STKFG
3: 5 MNCFX, 7 RFSQX, 2 FWMGM, 2 VPVL, 19 CXFTF => 3 HVMC
4: 53 STKFG, 6 MNCFX, 46 VJHF, 81 HVMC, 68 CXFTF, 25 GNMV => 1 FUEL

2210736 ORE for 1 FUEL:
171 ORE => 8 CNZTR
7 ZLQW, 3 BMBT, 9 XCVML, 26 XMNCP, 1 WPTQ, 2 MZWV, 1 RJRHP => 4 PLWSL
114 ORE => 4 BHXH
14 VRPVC => 6 BMBT
6 BHXH, 18 KTJDG, 12 WPTQ, 7 PLWSL, 31 FHTLT, 37 ZDVW => 1 FUEL
6 WPTQ, 2 BMBT, 8 ZLQW, 18 KTJDG, 1 XMNCP, 6 MZWV, 1 RJRHP => 6 FHTLT
15 XDBXC, 2 LTCX, 1 VRPVC => 6 ZLQW
13 WPTQ, 10 LTCX, 3 RJRHP, 14 XMNCP, 2 MZWV, 1 ZLQW => 1 ZDVW
5 BMBT => 4 WPTQ
189 ORE => 9 KTJDG
1 MZWV, 17 XDBXC, 3 XCVML => 2 XMNCP
12 VRPVC, 27 CNZTR => 2 XDBXC
15 KTJDG, 12 BHXH => 5 XCVML
3 BHXH, 2 VRPVC => 7 MZWV
121 ORE => 7 VRPVC
7 XCVML => 6 RJRHP
5 BHXH, 4 VRPVC => 5 LTCX

1: 171 ORE => 8 CNZTR
1: 114 ORE => 4 BHXH
1: 189 ORE => 9 KTJDG
1: 121 ORE => 7 VRPVC
2: 14 VRPVC => 6 BMBT
2: 5 BHXH, 4 VRPVC => 5 LTCX
2: 12 VRPVC, 27 CNZTR => 2 XDBXC
2: 3 BHXH, 2 VRPVC => 7 MZWV
2: 15 KTJDG, 12 BHXH => 5 XCVML
3: 5 BMBT => 4 WPTQ
3: 15 XDBXC, 2 LTCX, 1 VRPVC => 6 ZLQW
3: 1 MZWV, 17 XDBXC, 3 XCVML => 2 XMNCP
3: 7 XCVML => 6 RJRHP
4: 6 WPTQ, 2 BMBT, 8 ZLQW, 18 KTJDG, 1 XMNCP, 6 MZWV, 1 RJRHP => 6 FHTLT
4: 7 ZLQW, 3 BMBT, 9 XCVML, 26 XMNCP, 1 WPTQ, 2 MZWV, 1 RJRHP => 4 PLWSL
4: 13 WPTQ, 10 LTCX, 3 RJRHP, 14 XMNCP, 2 MZWV, 1 ZLQW => 1 ZDVW
5: 6 BHXH, 18 KTJDG, 12 WPTQ, 7 PLWSL, 31 FHTLT, 37 ZDVW => 1 FUEL

Given the list of reactions in your puzzle input, what is the minimum amount of ORE required to produce exactly 1 FUEL?

--- Part Two ---

After collecting ORE for a while, you check your cargo hold: 1 trillion (1000000000000) units of ORE.

With that much ore, given the examples above:

The 13312 ORE-per-FUEL example could produce 82892753 FUEL.
The 180697 ORE-per-FUEL example could produce 5586022 FUEL.
The 2210736 ORE-per-FUEL example could produce 460664 FUEL.
Given 1 trillion ORE, what is the maximum amount of FUEL you can produce?

*/

namespace Day14
{
    class Program
    {
        private Program(string inputFile, bool part1)
        {
            var lines = ReadProgram(inputFile);
            ParseInput(lines);
            if (part1)
            {
                var result = MakeFuel();
                Console.WriteLine($"Day14 Result1:{result}");
                if (result != 346961)
                {
                    throw new InvalidProgramException($"Part1 is broken {result} != 346961");
                }
            }
            else
            {
                var result = Program.FuelProduced(1000000000000L);
                Console.WriteLine($"Day14 Result2:{result}");

            }
        }

        struct RecipeInput
        {
            public RecipeInput(string inName, Int64 inQuantity)
            {
                name = inName;
                quantity = inQuantity;
            }
            public string name;
            public Int64 quantity;
        };

        struct Recipe
        {
            public string outputName;
            public Int64 outputQuantity;
            public RecipeInput[] inputs;
        };

        static Recipe[] sRecipes;
        static Dictionary<string, Int64> sInventory;

        private string[] ReadProgram(string inputFile)
        {
            var lines = File.ReadAllLines(inputFile);
            return lines;
        }

        static void AddRecipe(Recipe inRecipe)
        {
            for (var i = 0; i < sRecipes.Length; ++i)
            {
                if (sRecipes[i].outputName == inRecipe.outputName)
                {
                    return;
                }
                if (string.IsNullOrEmpty(sRecipes[i].outputName))
                {
                    sRecipes[i] = inRecipe;
                    return;
                }
            }
            throw new IndexOutOfRangeException($"Failed to AddRecipe {inRecipe.outputName} Length {sRecipes.Length}");
        }

        static bool DoesRecipeExist(string compoundName)
        {
            for (var i = 0; i < sRecipes.Length; ++i)
            {
                if (sRecipes[i].outputName == compoundName)
                {
                    return true;
                }
            }
            return false;
        }

        static Recipe FindRecipe(string compoundName)
        {
            for (var i = 0; i < sRecipes.Length; ++i)
            {
                if (sRecipes[i].outputName == compoundName)
                {
                    return sRecipes[i];
                }
            }
            throw new IndexOutOfRangeException($"Failed to find recipe {compoundName} Length {sRecipes.Length}");
        }

        public static bool VerifySort()
        {
            var knownOutputs = new HashSet<string>();
            foreach (var recipe in sRecipes)
            {
                knownOutputs.Add(recipe.outputName);
                foreach (var input in recipe.inputs)
                {
                    if (knownOutputs.Contains(input.name))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static int CountCompoundDepth(string compoundName)
        {
            Recipe recipe = FindRecipe(compoundName);
            var maxDepth = int.MinValue;
            var depth = 0;
            foreach (var input in recipe.inputs)
            {
                if (input.name != "ORE")
                {
                    depth = CountCompoundDepth(input.name) + 1;
                }
                else
                {
                    ++depth;
                }
                maxDepth = Math.Max(maxDepth, depth);
            }
            return maxDepth;
        }

        // 44 XJWVT, 5 KHKGT, 1 QDVJ, 29 NZVS, 9 GPVTF, 48 HKGWZ => 1 FUEL
        public static void ParseInput(string[] lines)
        {
            sRecipes = new Recipe[lines.Length];
            sInventory = new Dictionary<string, Int64>();
            foreach (var line in lines)
            {
                var data = line.Trim();
                var tokens = data.Split("=>");
                if (tokens.Length != 2)
                {
                    throw new InvalidDataException($"Wrong format LHS => RHS {tokens.Length}");
                }
                Recipe recipe = new Recipe();
                List<RecipeInput> inputsList = new List<RecipeInput>();
                var lhs = tokens[0].Trim();
                var inputTokens = lhs.Split(',');
                foreach (var inputToken in inputTokens)
                {
                    var input = inputToken.Trim();
                    var compoundTokens = input.Split(' ');
                    if (compoundTokens.Length != 2)
                    {
                        throw new InvalidDataException($"Wrong format XXX NAME {compoundTokens.Length}");
                    }
                    Int64 compoundCount = Int64.Parse(compoundTokens[0]);
                    string compoundName = compoundTokens[1];
                    inputsList.Add(new RecipeInput(compoundName, compoundCount));
                }
                var rhs = tokens[1].Trim();
                var outputTokens = rhs.Split(' ');
                if (outputTokens.Length != 2)
                {
                    throw new InvalidDataException($"Wrong format XXX NAME {outputTokens.Length}");
                }
                Int64 outputCount = Int64.Parse(outputTokens[0]);
                string outputName = outputTokens[1];

                recipe.outputName = outputName;
                recipe.outputQuantity = outputCount;
                recipe.inputs = inputsList.ToArray();
                AddRecipe(recipe);
                Console.WriteLine($"Output {outputName} {outputCount}");
                sInventory[outputName] = 0;
            }
            sInventory["ORE"] = 0;

            SortRecipes();
            foreach (var recipe in sRecipes)
            {
                var outputName = recipe.outputName;
                var outputCount = recipe.outputQuantity;
                var depth = CountCompoundDepth(outputName);
                Console.WriteLine($"Depth[{depth}] Output {outputName} {outputCount}");
            }
        }

        static void SortRecipes()
        {
            for (int i = 0; i < sRecipes.Length - 1; ++i)
            {
                for (int j = i + 1; j < sRecipes.Length; ++j)
                {
                    var iDepth = CountCompoundDepth(sRecipes[i].outputName);
                    var jDepth = CountCompoundDepth(sRecipes[j].outputName);
                    if (jDepth > iDepth)
                    {
                        var temp = sRecipes[i];
                        sRecipes[i] = sRecipes[j];
                        sRecipes[j] = temp;
                    }
                }
            }
        }

        public static double CompoundCost(string compoundOutput, string compoundInput)
        {
            if (!DoesRecipeExist(compoundOutput))
            {
                throw new ArgumentOutOfRangeException(nameof(compoundOutput), $"Compound {compoundOutput} not found in recipes");
            }
            var recipe = FindRecipe(compoundOutput);
            foreach (var input in recipe.inputs)
            {
                if (input.name == compoundInput)
                {
                    return (double)input.quantity / (double)recipe.outputQuantity;
                }

            }
            return 0;
        }

        static void MakeCompound(string compoundOutput, Int64 outputRequired)
        {
            if (!DoesRecipeExist(compoundOutput))
            {
                throw new ArgumentOutOfRangeException(nameof(compoundOutput), $"Compound {compoundOutput} not found in recipes");
            }
            var recipe = FindRecipe(compoundOutput);
            var outputProduced = sInventory[recipe.outputName];
            while (outputProduced < outputRequired)
            {
                foreach (var input in recipe.inputs)
                {
                    if (input.name != "ORE")
                    {
                        while (sInventory[input.name] < input.quantity)
                        {
                            MakeCompound(input.name, input.quantity);
                        }
                    }
                    sInventory[input.name] -= input.quantity;
                }
                sInventory[recipe.outputName] += recipe.outputQuantity;
                outputProduced = sInventory[recipe.outputName];
            }
        }

        static void ConvertGoodsBackToOre()
        {
            foreach (var recipe in sRecipes)
            {
                var goodOutput = recipe.outputName;
                var goodOutputPerRepeat = recipe.outputQuantity;
                var outputAvailable = sInventory[goodOutput];
                if (outputAvailable > 0)
                {
                    Int64 numRepeats = outputAvailable / goodOutputPerRepeat;
                    sInventory[goodOutput] -= numRepeats * goodOutputPerRepeat;
                    foreach (var input in recipe.inputs)
                    {
                        sInventory[input.name] += numRepeats * input.quantity;
                    }
                }
            }
        }

        public static Int64 MakeFuel()
        {
            MakeCompound("FUEL", 1);
            return -sInventory["ORE"];
        }

        public static Int64 FuelProducedSlow(Int64 inputOre)
        {
            Int64 fuelMade = 0;
            Int64 oreRemaining = inputOre;
            sInventory["ORE"] = oreRemaining;
            Console.WriteLine($"Start SLOW fuelMade:{fuelMade} ore:{oreRemaining}");
            while (sInventory["ORE"] >= 0)
            {
                sInventory["FUEL"] = 0;
                MakeFuel();
#if JAKE_DEBUG
                foreach (var (componentName, componentQuantity) in sInventory)
                {
                    Console.WriteLine($"MakeFuel Inventory: Component {componentName} Quantity {componentQuantity}");
                }
#endif // #if JAKE_DEBUG
                if (sInventory["ORE"] < 0)
                {
                    break;
                }
                ++fuelMade;
#if JAKE_DEBUG
                Console.WriteLine($"fuelMade:{fuelMade} ore:{oreRemaining}");
#endif // #if JAKE_DEBUG
                if (fuelMade > 30000)
                {
                    Console.WriteLine($"BROKE LOOP ore:{oreRemaining}");
                    break;
                }
            }
            Console.WriteLine($"End SLOW fuelMade:{fuelMade} ore:{oreRemaining}");
            return fuelMade;
        }

        public static Int64 FuelProduced(Int64 inputOre)
        {
            Int64 fuelMade = 0;
            sInventory["ORE"] = 0;
            Int64 minOreCost = MakeFuel();
            Int64 oreRemaining = inputOre;
            Int64 minFuelMade = oreRemaining / minOreCost;
            Dictionary<string, Int64> sGoodRemainingPerFuel = new Dictionary<string, Int64>();
            List<string> inventoryKeys = new List<string>(sInventory.Keys);
            foreach (var key in inventoryKeys)
            {
                sGoodRemainingPerFuel[key] = sInventory[key];
                sInventory[key] = 0;
            }
            sGoodRemainingPerFuel["FUEL"] = 0;
            sGoodRemainingPerFuel["ORE"] = 0;
            foreach (var (componentName, componentQuantity) in sGoodRemainingPerFuel)
            {
                Console.WriteLine($"GoodPerFuel {componentName} x {componentQuantity}");
            }

            while (minFuelMade > 0)
            {
                Console.WriteLine($"minFuelMade:{minFuelMade} minOreCost:{minOreCost} ore:{oreRemaining}");
                fuelMade += minFuelMade;
                foreach (var key in inventoryKeys)
                {
                    sInventory[key] += sGoodRemainingPerFuel[key] * minFuelMade;
                }
                oreRemaining -= minOreCost * minFuelMade;
                sInventory["ORE"] = oreRemaining;
                sInventory["FUEL"] = 0;
#if JAKE_DEBUG
                foreach (var (componentName, componentQuantity) in sInventory)
                {
                    Console.WriteLine($"Before Convert {componentName} x {componentQuantity}");
                }
#endif // #if JAKE_DEBUG
                ConvertGoodsBackToOre();
#if JAKE_DEBUG
                foreach (var (componentName, componentQuantity) in sInventory)
                {
                    Console.WriteLine($"After Convert {componentName} x {componentQuantity}");
                }
#endif // #if JAKE_DEBUG
                oreRemaining = sInventory["ORE"];
                minFuelMade = oreRemaining / minOreCost;
            };
            Console.WriteLine($"Start fuelMade:{fuelMade} ore:{oreRemaining}");
            fuelMade += FuelProducedSlow(oreRemaining);
            Console.WriteLine($"End fuelMade:{fuelMade} ore:{oreRemaining}");
            return fuelMade;
        }

        public static void Run()
        {
            Console.WriteLine("Day14 : Start");
            _ = new Program("Day14/input.txt", true);
            _ = new Program("Day14/input.txt", false);
            Console.WriteLine("Day14 : End");
        }
    }
}
