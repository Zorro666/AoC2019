using System;
using NUnit.Framework;

namespace Day12
{
    [TestFixture]
    public class Tests
    {
        [Test]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 0, -1, 0, 2, TestName = "ParseInput.Body(0) Position=-1,0,2)")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 1, 2, -10, -7, TestName = "ParseInput.Body(1) Position=2,-10,-7)")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 2, 4, -8, 8, TestName = "ParseInput.Body(2) Position=4,-8,8)")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 3, 3, 5, -1, TestName = "ParseInput.Body(3) Position=3,5,-1)")]
        public void ParseInputPosition(string[] lines, int body, int expectedX, int expectedY, int expectedZ)
        {
            Program.ParseInput(lines);
            Assert.That(Program.GetBody(body).PosX, Is.EqualTo(expectedX));
            Assert.That(Program.GetBody(body).PosY, Is.EqualTo(expectedY));
            Assert.That(Program.GetBody(body).PosZ, Is.EqualTo(expectedZ));
        }

        [Test]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 0, TestName = "ParseInput.Body(0) Velocity=0,0,0)")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 1, TestName = "ParseInput.Body(1) Velocity=0,0,0)")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 2, TestName = "ParseInput.Body(2) Velocity=0,0,0)")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 3, TestName = "ParseInput.Body(3) Velocity=0,0,0)")]
        public void ParseInputVelocityIsZero(string[] lines, int body)
        {
            Program.ParseInput(lines);
            Assert.That(Program.GetBody(body).VelX, Is.Zero);
            Assert.That(Program.GetBody(body).VelY, Is.Zero);
            Assert.That(Program.GetBody(body).VelZ, Is.Zero);
        }

        [Test]
        /*
		<x=-1, y=0, z=2>
		<x=2, y=-10, z=-7>
		<x=4, y=-8, z=8>
		<x=3, y=5, z=-1>
		*/
        /*
		After 0 steps:
		pos=<x=-1, y=  0, z= 2>, vel=<x= 0, y= 0, z= 0>
		pos=<x= 2, y=-10, z=-7>, vel=<x= 0, y= 0, z= 0>
		pos=<x= 4, y= -8, z= 8>, vel=<x= 0, y= 0, z= 0>
		pos=<x= 3, y=  5, z=-1>, vel=<x= 0, y= 0, z= 0>
		*/
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 0, 0, -1, 0, 2, TestName = "Simulate.A Step(0) Body(0) Position=-1,0,2")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 0, 1, 2, -10, -7, TestName = "Simulate.A Step(0) Body(1) Position=2,-10,-7)")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 0, 2, 4, -8, 8, TestName = "Simulate.A Step(0) Body(2) Position=4,-8,8)")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 0, 3, 3, 5, -1, TestName = "Simulate.A Step(0) Body(3) Position=3,5,-1)")]
        /*
		After 1 step:
		pos=<x= 2, y=-1, z= 1>, vel=<x= 3, y=-1, z=-1>
		pos=<x= 3, y=-7, z=-4>, vel=<x= 1, y= 3, z= 3>
		pos=<x= 1, y=-7, z= 5>, vel=<x=-3, y= 1, z=-3>
		pos=<x= 2, y= 2, z= 0>, vel=<x=-1, y=-3, z= 1>
		*/
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 1, 0, 2, -1, 1, TestName = "Simulate.A Step(1) Body(0) Position=2,-1,1")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 1, 1, 3, -7, -4, TestName = "Simulate.A Step(1) Body(1) Position=3,-7,-4)")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 1, 2, 1, -7, 5, TestName = "Simulate.A Step(1) Body(2) Position=1,-7,5)")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 1, 3, 2, 2, 0, TestName = "Simulate.A Step(1) Body(3) Position=2,2,0)")]
        /*
		After 2 steps:
		pos=<x= 5, y=-3, z=-1>, vel=<x= 3, y=-2, z=-2>
		pos=<x= 1, y=-2, z= 2>, vel=<x=-2, y= 5, z= 6>
		pos=<x= 1, y=-4, z=-1>, vel=<x= 0, y= 3, z=-6>
		pos=<x= 1, y=-4, z= 2>, vel=<x=-1, y=-6, z= 2>
		*/
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 2, 0, 5, -3, -1, TestName = "Simulate.A Step(2) Body(0) Position=5,-3,-1")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 2, 1, 1, -2, 2, TestName = "Simulate.A Step(2) Body(1) Position=1,-2,2)")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 2, 2, 1, -4, -1, TestName = "Simulate.A Step(2) Body(2) Position=1,-4,-1)")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 2, 3, 1, -4, 2, TestName = "Simulate.A Step(2) Body(3) Position=1,-4,2)")]
        /*
		After 3 steps:
		pos=<x= 5, y=-6, z=-1>, vel=<x= 0, y=-3, z= 0>
		pos=<x= 0, y= 0, z= 6>, vel=<x=-1, y= 2, z= 4>
		pos=<x= 2, y= 1, z=-5>, vel=<x= 1, y= 5, z=-4>
		pos=<x= 1, y=-8, z= 2>, vel=<x= 0, y=-4, z= 0>
		*/
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 3, 0, 5, -6, -1, TestName = "Simulate.A Step(3) Body(0) Position=5,-6,-1")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 3, 1, 0, 0, 6, TestName = "Simulate.A Step(3) Body(1) Position=0,0,6)")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 3, 2, 2, 1, -5, TestName = "Simulate.A Step(3) Body(2) Position=2,1,-5)")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 3, 3, 1, -8, 2, TestName = "Simulate.A Step(3) Body(3) Position=1,-8,2)")]
        /*
		After 4 steps:
		pos=<x= 2, y=-8, z= 0>, vel=<x=-3, y=-2, z= 1>
		pos=<x= 2, y= 1, z= 7>, vel=<x= 2, y= 1, z= 1>
		pos=<x= 2, y= 3, z=-6>, vel=<x= 0, y= 2, z=-1>
		pos=<x= 2, y=-9, z= 1>, vel=<x= 1, y=-1, z=-1>
		*/
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 4, 0, 2, -8, 0, TestName = "Simulate.A Step(4) Body(0) Position=2,-8,0")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 4, 1, 2, 1, 7, TestName = "Simulate.A Step(4) Body(1) Position=2,1,7)")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 4, 2, 2, 3, -6, TestName = "Simulate.A Step(4) Body(2) Position=2,3,-6)")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 4, 3, 2, -9, 1, TestName = "Simulate.A Step(4) Body(3) Position=2,-9,1)")]
        /*
		After 5 steps:
		pos=<x=-1, y=-9, z= 2>, vel=<x=-3, y=-1, z= 2>
		pos=<x= 4, y= 1, z= 5>, vel=<x= 2, y= 0, z=-2>
		pos=<x= 2, y= 2, z=-4>, vel=<x= 0, y=-1, z= 2>
		pos=<x= 3, y=-7, z=-1>, vel=<x= 1, y= 2, z=-2>
		*/
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 5, 0, -1, -9, 2, TestName = "Simulate.A Step(5) Body(0) Position=-1,-9,2")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 5, 1, 4, 1, 5, TestName = "Simulate.A Step(5) Body(1) Position=4,1,5)")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 5, 2, 2, 2, -4, TestName = "Simulate.A Step(5) Body(2) Position=2,2,-4)")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 5, 3, 3, -7, -1, TestName = "Simulate.A Step(5) Body(3) Position=3,-7,-1)")]
        /*
		After 6 steps:
		pos=<x=-1, y=-7, z= 3>, vel=<x= 0, y= 2, z= 1>
		pos=<x= 3, y= 0, z= 0>, vel=<x=-1, y=-1, z=-5>
		pos=<x= 3, y=-2, z= 1>, vel=<x= 1, y=-4, z= 5>
		pos=<x= 3, y=-4, z=-2>, vel=<x= 0, y= 3, z=-1>
		*/
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 6, 0, -1, -7, 3, TestName = "Simulate.A Step(6) Body(0) Position=-1,-7,3")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 6, 1, 3, 0, 0, TestName = "Simulate.A Step(6) Body(1) Position=3,0,0)")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 6, 2, 3, -2, 1, TestName = "Simulate.A Step(6) Body(2) Position=3,-2,1)")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 6, 3, 3, -4, -2, TestName = "Simulate.A Step(6) Body(3) Position=3,-4,-2)")]
        /*
		After 7 steps:
		pos=<x= 2, y=-2, z= 1>, vel=<x= 3, y= 5, z=-2>
		pos=<x= 1, y=-4, z=-4>, vel=<x=-2, y=-4, z=-4>
		pos=<x= 3, y=-7, z= 5>, vel=<x= 0, y=-5, z= 4>
		pos=<x= 2, y= 0, z= 0>, vel=<x=-1, y= 4, z= 2>
		*/
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 7, 0, 2, -2, 1, TestName = "Simulate.A Step(7) Body(0) Position=2,-2,1")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 7, 1, 1, -4, -4, TestName = "Simulate.A Step(7) Body(1) Position=1,-4,-4)")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 7, 2, 3, -7, 5, TestName = "Simulate.A Step(7) Body(2) Position=3,-7,5)")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 7, 3, 2, 0, 0, TestName = "Simulate.A Step(7) Body(3) Position=2,0,0)")]
        /*
		After 8 steps:
		pos=<x= 5, y= 2, z=-2>, vel=<x= 3, y= 4, z=-3>
		pos=<x= 2, y=-7, z=-5>, vel=<x= 1, y=-3, z=-1>
		pos=<x= 0, y=-9, z= 6>, vel=<x=-3, y=-2, z= 1>
		pos=<x= 1, y= 1, z= 3>, vel=<x=-1, y= 1, z= 3>
		*/
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 8, 0, 5, 2, -2, TestName = "Simulate.A Step(8) Body(0) Position=5,2,-2")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 8, 1, 2, -7, -5, TestName = "Simulate.A Step(8) Body(1) Position=2,-7,-5)")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 8, 2, 0, -9, 6, TestName = "Simulate.A Step(8) Body(2) Position=0,-9,6)")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 8, 3, 1, 1, 3, TestName = "Simulate.A Step(8) Body(3) Position=1,1,3)")]
        /*
		After 9 steps:
		pos=<x= 5, y= 3, z=-4>, vel=<x= 0, y= 1, z=-2>
		pos=<x= 2, y=-9, z=-3>, vel=<x= 0, y=-2, z= 2>
		pos=<x= 0, y=-8, z= 4>, vel=<x= 0, y= 1, z=-2>
		pos=<x= 1, y= 1, z= 5>, vel=<x= 0, y= 0, z= 2>
		*/
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 9, 0, 5, 3, -4, TestName = "Simulate.A Step(9) Body(0) Position=5,3,-4")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 9, 1, 2, -9, -3, TestName = "Simulate.A Step(9) Body(1) Position=2,-9,-3)")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 9, 2, 0, -8, 4, TestName = "Simulate.A Step(9) Body(2) Position=0,-8,4)")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 9, 3, 1, 1, 5, TestName = "Simulate.A Step(9) Body(3) Position=1,1,5)")]
        /*
		After 10 steps:
		pos=<x= 2, y= 1, z=-3>, vel=<x=-3, y=-2, z= 1>
		pos=<x= 1, y=-8, z= 0>, vel=<x=-1, y= 1, z= 3>
		pos=<x= 3, y=-6, z= 1>, vel=<x= 3, y= 2, z=-3>
		pos=<x= 2, y= 0, z= 4>, vel=<x= 1, y=-1, z=-1>
		*/
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 10, 0, 2, 1, -3, TestName = "Simulate.A Step(10) Body(0) Position=2,1,-3")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 10, 1, 1, -8, 0, TestName = "Simulate.A Step(10) Body(1) Position=1,-8,0)")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 10, 2, 3, -6, 1, TestName = "Simulate.A Step(10) Body(2) Position=3,-6,1)")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 10, 3, 2, 0, 4, TestName = "Simulate.A Step(10) Body(3) Position=2,0,4)")]
        /*
		<x=-8, y=-10, z=0>
		<x=5, y=5, z=10>
		<x=2, y=-7, z=3>
		<x=9, y=-8, z=-3>
		*/
        /*
		After 0 steps:
		pos=<x= -8, y=-10, z=  0>, vel=<x=  0, y=  0, z=  0>
		pos=<x=  5, y=  5, z= 10>, vel=<x=  0, y=  0, z=  0>
		pos=<x=  2, y= -7, z=  3>, vel=<x=  0, y=  0, z=  0>
		pos=<x=  9, y= -8, z= -3>, vel=<x=  0, y=  0, z=  0>
        */
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 0, 0, -8, -10, 0, TestName = "Simulate.B Step(0) Body(0) Position=-8,-10,0")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 0, 1, 5, 5, 10, TestName = "Simulate.B Step(0) Body(1) Position=5,5,10")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 0, 2, 2, -7, 3, TestName = "Simulate.B Step(0) Body(2) Position=2,-7,3")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 0, 3, 9, -8, -3, TestName = "Simulate.B Step(0) Body(3) Position=9,-8,-3")]
        /*
		After 10 steps:
		pos=<x= -9, y=-10, z=  1>, vel=<x= -2, y= -2, z= -1>
		pos=<x=  4, y= 10, z=  9>, vel=<x= -3, y=  7, z= -2>
		pos=<x=  8, y=-10, z= -3>, vel=<x=  5, y= -1, z= -2>
		pos=<x=  5, y=-10, z=  3>, vel=<x=  0, y= -4, z=  5>
        */
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 10, 0, -9, -10, 1, TestName = "Simulate.B Step(10) Body(0) Position=-9,-10,1")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 10, 1, 4, 10, 9, TestName = "Simulate.B Step(10) Body(1) Position=4,10,9")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 10, 2, 8, -10, -3, TestName = "Simulate.B Step(10) Body(2) Position=8,-10,-3")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 10, 3, 5, -10, 3, TestName = "Simulate.B Step(10) Body(3) Position=5,-10,3")]
        /*
        After 20 steps:
		pos=<x=-10, y=  3, z= -4>, vel=<x= -5, y=  2, z=  0>
		pos=<x=  5, y=-25, z=  6>, vel=<x=  1, y=  1, z= -4>
		pos=<x= 13, y=  1, z=  1>, vel=<x=  5, y= -2, z=  2>
		pos=<x=  0, y=  1, z=  7>, vel=<x= -1, y= -1, z=  2>
        */
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 20, 0, -10, 3, -4, TestName = "Simulate.B Step(20) Body(0) Position=-10,3,-4")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 20, 1, 5, -25, 6, TestName = "Simulate.B Step(20) Body(1) Position=5,-25,6")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 20, 2, 13, 1, 1, TestName = "Simulate.B Step(20) Body(2) Position=13,1,1")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 20, 3, 0, 1, 7, TestName = "Simulate.B Step(20) Body(3) Position=0,1,7")]
        /*
		After 30 steps:
		pos=<x= 15, y= -6, z= -9>, vel=<x= -5, y=  4, z=  0>
		pos=<x= -4, y=-11, z=  3>, vel=<x= -3, y=-10, z=  0>
		pos=<x=  0, y= -1, z= 11>, vel=<x=  7, y=  4, z=  3>
		pos=<x= -3, y= -2, z=  5>, vel=<x=  1, y=  2, z= -3>
        */
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 30, 0, 15, -6, -9, TestName = "Simulate.B Step(30) Body(0) Position=15,-6,-9")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 30, 1, -4, -11, 3, TestName = "Simulate.B Step(30) Body(1) Position=-4,-11,3")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 30, 2, 0, -1, 11, TestName = "Simulate.B Step(30) Body(2) Position=0,-1,11")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 30, 3, -3, -2, 5, TestName = "Simulate.B Step(30) Body(3) Position=-3,-2,5")]
        /*
		After 40 steps:
		pos=<x= 14, y=-12, z= -4>, vel=<x= 11, y=  3, z=  0>
		pos=<x= -1, y= 18, z=  8>, vel=<x= -5, y=  2, z=  3>
		pos=<x= -5, y=-14, z=  8>, vel=<x=  1, y= -2, z=  0>
		pos=<x=  0, y=-12, z= -2>, vel=<x= -7, y= -3, z= -3>
        */
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 40, 0, 14, -12, -4, TestName = "Simulate.B Step(40) Body(0) Position=14,-12,-4")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 40, 1, -1, 18, 8, TestName = "Simulate.B Step(40) Body(1) Position=-1,18,8")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 40, 2, -5, -14, 8, TestName = "Simulate.B Step(40) Body(2) Position=-5,-14,8")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 40, 3, 0, -12, -2, TestName = "Simulate.B Step(40) Body(3) Position=0,-12,-2")]
        /*
		After 50 steps:
		pos=<x=-23, y=  4, z=  1>, vel=<x= -7, y= -1, z=  2>
		pos=<x= 20, y=-31, z= 13>, vel=<x=  5, y=  3, z=  4>
		pos=<x= -4, y=  6, z=  1>, vel=<x= -1, y=  1, z= -3>
		pos=<x= 15, y=  1, z= -5>, vel=<x=  3, y= -3, z= -3>
        */
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 50, 0, -23, 4, 1, TestName = "Simulate.B Step(50) Body(0) Position=-23,4,1")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 50, 1, 20, -31, 13, TestName = "Simulate.B Step(50) Body(1) Position=20,-31,13")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 50, 2, -4, 6, 1, TestName = "Simulate.B Step(50) Body(2) Position=-4,6,1")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 50, 3, 15, 1, -5, TestName = "Simulate.B Step(50) Body(3) Position=15,1,-5")]
        /*
		After 60 steps:
		pos=<x= 36, y=-10, z=  6>, vel=<x=  5, y=  0, z=  3>
		pos=<x=-18, y= 10, z=  9>, vel=<x= -3, y= -7, z=  5>
		pos=<x=  8, y=-12, z= -3>, vel=<x= -2, y=  1, z= -7>
		pos=<x=-18, y= -8, z= -2>, vel=<x=  0, y=  6, z= -1>
        */
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 60, 0, 36, -10, 6, TestName = "Simulate.B Step(60) Body(0) Position=36,-10,6")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 60, 1, -18, 10, 9, TestName = "Simulate.B Step(60) Body(1) Position=-18,10,9")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 60, 2, 8, -12, -3, TestName = "Simulate.B Step(60) Body(2) Position=8,-12,-3")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 60, 3, -18, -8, -2, TestName = "Simulate.B Step(60) Body(3) Position=-18,-8,-2")]
        /*
		After 70 steps:
		pos=<x=-33, y= -6, z=  5>, vel=<x= -5, y= -4, z=  7>
		pos=<x= 13, y= -9, z=  2>, vel=<x= -2, y= 11, z=  3>
		pos=<x= 11, y= -8, z=  2>, vel=<x=  8, y= -6, z= -7>
		pos=<x= 17, y=  3, z=  1>, vel=<x= -1, y= -1, z= -3>
        */
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 70, 0, -33, -6, 5, TestName = "Simulate.B Step(70) Body(0) Position=-33,-6,5")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 70, 1, 13, -9, 2, TestName = "Simulate.B Step(70) Body(1) Position=13,-9,2")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 70, 2, 11, -8, 2, TestName = "Simulate.B Step(70) Body(2) Position=11,-8,2")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 70, 3, 17, 3, 1, TestName = "Simulate.B Step(70) Body(3) Position=17,3,1")]
        /*
		After 80 steps:
		pos=<x= 30, y= -8, z=  3>, vel=<x=  3, y=  3, z=  0>
		pos=<x= -2, y= -4, z=  0>, vel=<x=  4, y=-13, z=  2>
		pos=<x=-18, y= -7, z= 15>, vel=<x= -8, y=  2, z= -2>
		pos=<x= -2, y= -1, z= -8>, vel=<x=  1, y=  8, z=  0>
        */
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 80, 0, 30, -8, 3, TestName = "Simulate.B Step(80) Body(0) Position=30,-8,3")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 80, 1, -2, -4, 0, TestName = "Simulate.B Step(80) Body(1) Position=-2,-4,0")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 80, 2, -18, -7, 15, TestName = "Simulate.B Step(80) Body(2) Position=-18,-7,15")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 80, 3, -2, -1, -8, TestName = "Simulate.B Step(80) Body(3) Position=-2,-1,8")]
        /*
		After 90 steps:
		pos=<x=-25, y= -1, z=  4>, vel=<x=  1, y= -3, z=  4>
		pos=<x=  2, y= -9, z=  0>, vel=<x= -3, y= 13, z= -1>
		pos=<x= 32, y= -8, z= 14>, vel=<x=  5, y= -4, z=  6>
		pos=<x= -1, y= -2, z= -8>, vel=<x= -3, y= -6, z= -9>
        */
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 90, 0, -25, -1, 4, TestName = "Simulate.B Step(90) Body(0) Position=-25,-1,4")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 90, 1, 2, -9, 0, TestName = "Simulate.B Step(90) Body(1) Position=2,-9,0")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 90, 2, 32, -8, 14, TestName = "Simulate.B Step(90) Body(2) Position=32,-8,14")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 90, 3, -1, -2, -8, TestName = "Simulate.B Step(90) Body(3) Position=-1,-2,-8")]
        /*
		After 100 steps:
		pos=<x=  8, y=-12, z= -9>, vel=<x= -7, y=  3, z=  0>
		pos=<x= 13, y= 16, z= -3>, vel=<x=  3, y=-11, z= -5>
		pos=<x=-29, y=-11, z= -1>, vel=<x= -3, y=  7, z=  4>
		pos=<x= 16, y=-13, z= 23>, vel=<x=  7, y=  1, z=  1>
        */
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 100, 0, 8, -12, -9, TestName = "Simulate.B Step(100) Body(0) Position=8,-12,-9")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 100, 1, 13, 16, -3, TestName = "Simulate.B Step(100) Body(1) Position=13,16,-3")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 100, 2, -29, -11, -1, TestName = "Simulate.B Step(100) Body(2) Position=-29,-11,-1")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 100, 3, 16, -13, 23, TestName = "Simulate.B Step(100) Body(3) Position=16,-13,23")]
        public void SimulatePosition(string[] lines, int steps, int body, int expectedPosX, int expectedPosY, int expectedPosZ)
        {
            Program.ParseInput(lines);
            Program.Simulate(steps);
            Assert.That(Program.GetBody(body).PosX, Is.EqualTo(expectedPosX));
            Assert.That(Program.GetBody(body).PosY, Is.EqualTo(expectedPosY));
            Assert.That(Program.GetBody(body).PosZ, Is.EqualTo(expectedPosZ));
        }

        [Test]
        /*
		<x=-1, y=0, z=2>
		<x=2, y=-10, z=-7>
		<x=4, y=-8, z=8>
		<x=3, y=5, z=-1>
		*/
        /*
		After 0 steps:
		pos=<x=-1, y=  0, z= 2>, vel=<x= 0, y= 0, z= 0>
		pos=<x= 2, y=-10, z=-7>, vel=<x= 0, y= 0, z= 0>
		pos=<x= 4, y= -8, z= 8>, vel=<x= 0, y= 0, z= 0>
		pos=<x= 3, y=  5, z=-1>, vel=<x= 0, y= 0, z= 0>
		*/
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 0, 0, 0, 0, 0, TestName = "Simulate.A Step(0) Body(0) Velocity=0,0,0")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 0, 1, 0, 0, 0, TestName = "Simulate.A Step(0) Body(1) Velocity=0,0,0)")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 0, 2, 0, 0, 0, TestName = "Simulate.A Step(0) Body(2) Velocity=0,0,0)")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 0, 3, 0, 0, 0, TestName = "Simulate.A Step(0) Body(3) Velocity=0,0,0)")]
        /*
		After 1 step:
		pos=<x= 2, y=-1, z= 1>, vel=<x= 3, y=-1, z=-1>
		pos=<x= 3, y=-7, z=-4>, vel=<x= 1, y= 3, z= 3>
		pos=<x= 1, y=-7, z= 5>, vel=<x=-3, y= 1, z=-3>
		pos=<x= 2, y= 2, z= 0>, vel=<x=-1, y=-3, z= 1>
		*/
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 1, 0, 3, -1, -1, TestName = "Simulate.A Step(1) Body(0) Velocity=3,-1,-1")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 1, 1, 1, 3, 3, TestName = "Simulate.A Step(1) Body(1) Velocity=1,3,3)")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 1, 2, -3, 1, -3, TestName = "Simulate.A Step(1) Body(2) Velocity=-3,1,-3)")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 1, 3, -1, -3, 1, TestName = "Simulate.A Step(1) Body(3) Velocity=-1,-3,1)")]
        /*
		After 2 steps:
		pos=<x= 5, y=-3, z=-1>, vel=<x= 3, y=-2, z=-2>
		pos=<x= 1, y=-2, z= 2>, vel=<x=-2, y= 5, z= 6>
		pos=<x= 1, y=-4, z=-1>, vel=<x= 0, y= 3, z=-6>
		pos=<x= 1, y=-4, z= 2>, vel=<x=-1, y=-6, z= 2>
		*/
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 2, 0, 3, -2, -2, TestName = "Simulate.A Step(2) Body(0) Velocity=3,-2,-2")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 2, 1, -2, 5, 6, TestName = "Simulate.A Step(2) Body(1) Velocity=-2,5,6)")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 2, 2, 0, 3, -6, TestName = "Simulate.A Step(2) Body(2) Velocity=0,3,-6)")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 2, 3, -1, -6, 2, TestName = "Simulate.A Step(2) Body(3) Velocity=-1,-6,2)")]
        /*
		After 3 steps:
		pos=<x= 5, y=-6, z=-1>, vel=<x= 0, y=-3, z= 0>
		pos=<x= 0, y= 0, z= 6>, vel=<x=-1, y= 2, z= 4>
		pos=<x= 2, y= 1, z=-5>, vel=<x= 1, y= 5, z=-4>
		pos=<x= 1, y=-8, z= 2>, vel=<x= 0, y=-4, z= 0>
		*/
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 3, 0, 0, -3, 0, TestName = "Simulate.A Step(3) Body(0) Velocity=0,-3,0")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 3, 1, -1, 2, 4, TestName = "Simulate.A Step(3) Body(1) Velocity=-1,2,4)")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 3, 2, 1, 5, -4, TestName = "Simulate.A Step(3) Body(2) Velocity=1,5,-4)")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 3, 3, 0, -4, 0, TestName = "Simulate.A Step(3) Body(3) Velocity=0,-4,0)")]
        /*
		After 4 steps:
		pos=<x= 2, y=-8, z= 0>, vel=<x=-3, y=-2, z= 1>
		pos=<x= 2, y= 1, z= 7>, vel=<x= 2, y= 1, z= 1>
		pos=<x= 2, y= 3, z=-6>, vel=<x= 0, y= 2, z=-1>
		pos=<x= 2, y=-9, z= 1>, vel=<x= 1, y=-1, z=-1>
		*/
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 4, 0, -3, -2, 1, TestName = "Simulate.A Step(4) Body(0) Velocity=-3,-2,1")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 4, 1, 2, 1, 1, TestName = "Simulate.A Step(4) Body(1) Velocity=2,1,1)")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 4, 2, 0, 2, -1, TestName = "Simulate.A Step(4) Body(2) Velocity=0,2,-1)")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 4, 3, 1, -1, -1, TestName = "Simulate.A Step(4) Body(3) Velocity=1,-1,-1)")]
        /*
		After 5 steps:
		pos=<x=-1, y=-9, z= 2>, vel=<x=-3, y=-1, z= 2>
		pos=<x= 4, y= 1, z= 5>, vel=<x= 2, y= 0, z=-2>
		pos=<x= 2, y= 2, z=-4>, vel=<x= 0, y=-1, z= 2>
		pos=<x= 3, y=-7, z=-1>, vel=<x= 1, y= 2, z=-2>
		*/
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 5, 0, -3, -1, 2, TestName = "Simulate.A Step(5) Body(0) Velocity=-3,-1,2")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 5, 1, 2, 0, -2, TestName = "Simulate.A Step(5) Body(1) Velocity=2,0,-2)")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 5, 2, 0, -1, 2, TestName = "Simulate.A Step(5) Body(2) Velocity=0,-1,2)")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 5, 3, 1, 2, -2, TestName = "Simulate.A Step(5) Body(3) Velocity=1,2,-2)")]
        /*
		After 6 steps:
		pos=<x=-1, y=-7, z= 3>, vel=<x= 0, y= 2, z= 1>
		pos=<x= 3, y= 0, z= 0>, vel=<x=-1, y=-1, z=-5>
		pos=<x= 3, y=-2, z= 1>, vel=<x= 1, y=-4, z= 5>
		pos=<x= 3, y=-4, z=-2>, vel=<x= 0, y= 3, z=-1>
		*/
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 6, 0, 0, 2, 1, TestName = "Simulate.A Step(6) Body(0) Velocity=0,2,1")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 6, 1, -1, -1, -5, TestName = "Simulate.A Step(6) Body(1) Velocity=-1,-1,-5)")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 6, 2, 1, -4, 5, TestName = "Simulate.A Step(6) Body(2) Velocity=1,-4,5)")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 6, 3, 0, 3, -1, TestName = "Simulate.A Step(6) Body(3) Velocity=0,3,-1)")]
        /*
		After 7 steps:
		pos=<x= 2, y=-2, z= 1>, vel=<x= 3, y= 5, z=-2>
		pos=<x= 1, y=-4, z=-4>, vel=<x=-2, y=-4, z=-4>
		pos=<x= 3, y=-7, z= 5>, vel=<x= 0, y=-5, z= 4>
		pos=<x= 2, y= 0, z= 0>, vel=<x=-1, y= 4, z= 2>
		*/
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 7, 0, 3, 5, -2, TestName = "Simulate.A Step(7) Body(0) Velocity=3,5,-2")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 7, 1, -2, -4, -4, TestName = "Simulate.A Step(7) Body(1) Velocity=-2,-4,-4)")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 7, 2, 0, -5, 4, TestName = "Simulate.A Step(7) Body(2) Velocity=0,-5,4)")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 7, 3, -1, 4, 2, TestName = "Simulate.A Step(7) Body(3) Velocity=-1,4,2)")]
        /*
		After 8 steps:
		pos=<x= 5, y= 2, z=-2>, vel=<x= 3, y= 4, z=-3>
		pos=<x= 2, y=-7, z=-5>, vel=<x= 1, y=-3, z=-1>
		pos=<x= 0, y=-9, z= 6>, vel=<x=-3, y=-2, z= 1>
		pos=<x= 1, y= 1, z= 3>, vel=<x=-1, y= 1, z= 3>
		*/
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 8, 0, 3, 4, -3, TestName = "Simulate.A Step(8) Body(0) Velocity=3,4,-3")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 8, 1, 1, -3, -1, TestName = "Simulate.A Step(8) Body(1) Velocity=1,-3,-1)")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 8, 2, -3, -2, 1, TestName = "Simulate.A Step(8) Body(2) Velocity=-3,-2,1)")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 8, 3, -1, 1, 3, TestName = "Simulate.A Step(8) Body(3) Velocity=-1,1,3)")]
        /*
		After 9 steps:
		pos=<x= 5, y= 3, z=-4>, vel=<x= 0, y= 1, z=-2>
		pos=<x= 2, y=-9, z=-3>, vel=<x= 0, y=-2, z= 2>
		pos=<x= 0, y=-8, z= 4>, vel=<x= 0, y= 1, z=-2>
		pos=<x= 1, y= 1, z= 5>, vel=<x= 0, y= 0, z= 2>
		*/
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 9, 0, 0, 1, -2, TestName = "Simulate.A Step(9) Body(0) Velocity=0,1,-2")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 9, 1, 0, -2, 2, TestName = "Simulate.A Step(9) Body(1) Velocity=0,-2,2)")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 9, 2, 0, 1, -2, TestName = "Simulate.A Step(9) Body(2) Velocity=0,1,-2)")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 9, 3, 0, 0, 2, TestName = "Simulate.A Step(9) Body(3) Velocity=0,0,2)")]
        /*
		After 10 steps:
		pos=<x= 2, y= 1, z=-3>, vel=<x=-3, y=-2, z= 1>
		pos=<x= 1, y=-8, z= 0>, vel=<x=-1, y= 1, z= 3>
		pos=<x= 3, y=-6, z= 1>, vel=<x= 3, y= 2, z=-3>
		pos=<x= 2, y= 0, z= 4>, vel=<x= 1, y=-1, z=-1>
		*/
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 10, 0, -3, -2, 1, TestName = "Simulate.A Step(10) Body(0) Velocity=-3,-2,1")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 10, 1, -1, 1, 3, TestName = "Simulate.A Step(10) Body(1) Velocity=-1,1,3)")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 10, 2, 3, 2, -3, TestName = "Simulate.A Step(10) Body(2) Velocity=3,2,-3)")]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 10, 3, 1, -1, -1, TestName = "Simulate.A Step(10) Body(3) Velocity=1,-1,-1)")]
        /*
		<x=-8, y=-10, z=0>
		<x=5, y=5, z=10>
		<x=2, y=-7, z=3>
		<x=9, y=-8, z=-3>
		*/
        /*
		After 0 steps:
		pos=<x= -8, y=-10, z=  0>, vel=<x=  0, y=  0, z=  0>
		pos=<x=  5, y=  5, z= 10>, vel=<x=  0, y=  0, z=  0>
		pos=<x=  2, y= -7, z=  3>, vel=<x=  0, y=  0, z=  0>
		pos=<x=  9, y= -8, z= -3>, vel=<x=  0, y=  0, z=  0>
        */
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 0, 0, 0, 0, 0, TestName = "Simulate.B Step(0) Body(0) Velocity=0,0,0")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 0, 1, 0, 0, 0, TestName = "Simulate.B Step(0) Body(1) Velocity=0,0,0")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 0, 2, 0, 0, 0, TestName = "Simulate.B Step(0) Body(2) Velocity=0,0,0")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 0, 3, 0, 0, 0, TestName = "Simulate.B Step(0) Body(3) Velocity=0,0,0")]
        /*
		After 10 steps:
		pos=<x= -9, y=-10, z=  1>, vel=<x= -2, y= -2, z= -1>
		pos=<x=  4, y= 10, z=  9>, vel=<x= -3, y=  7, z= -2>
		pos=<x=  8, y=-10, z= -3>, vel=<x=  5, y= -1, z= -2>
		pos=<x=  5, y=-10, z=  3>, vel=<x=  0, y= -4, z=  5>
        */
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 10, 0, -2, -2, -1, TestName = "Simulate.B Step(10) Body(0) Velocity=-2,-2,-1")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 10, 1, -3, 7, -2, TestName = "Simulate.B Step(10) Body(1) Velocity=-3,7,-2")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 10, 2, 5, -1, -2, TestName = "Simulate.B Step(10) Body(2) Velocity=5,-1,-2")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 10, 3, 0, -4, 5, TestName = "Simulate.B Step(10) Body(3) Velocity=0,-4,5")]
        /*
		After 20 steps:
		pos=<x=-10, y=  3, z= -4>, vel=<x= -5, y=  2, z=  0>
		pos=<x=  5, y=-25, z=  6>, vel=<x=  1, y=  1, z= -4>
		pos=<x= 13, y=  1, z=  1>, vel=<x=  5, y= -2, z=  2>
		pos=<x=  0, y=  1, z=  7>, vel=<x= -1, y= -1, z=  2>
        */
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 20, 0, -5, 2, 0, TestName = "Simulate.B Step(20) Body(0) Velocity=-5,2,0")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 20, 1, 1, 1, -4, TestName = "Simulate.B Step(20) Body(1) Velocity=1,1,-4")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 20, 2, 5, -2, 2, TestName = "Simulate.B Step(20) Body(2) Velocity=5,-2,2")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 20, 3, -1, -1, 2, TestName = "Simulate.B Step(20) Body(3) Velocity=-1,-1,2")]
        /*
		After 30 steps:
		pos=<x= 15, y= -6, z= -9>, vel=<x= -5, y=  4, z=  0>
		pos=<x= -4, y=-11, z=  3>, vel=<x= -3, y=-10, z=  0>
		pos=<x=  0, y= -1, z= 11>, vel=<x=  7, y=  4, z=  3>
		pos=<x= -3, y= -2, z=  5>, vel=<x=  1, y=  2, z= -3>
        */
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 30, 0, -5, 4, 0, TestName = "Simulate.B Step(30) Body(0) Velocity=-5,4,0")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 30, 1, -3, -10, 0, TestName = "Simulate.B Step(30) Body(1) Velocity=-3,-10,0")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 30, 2, 7, 4, 3, TestName = "Simulate.B Step(30) Body(2) Velocity=7,4,3")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 30, 3, 1, 2, -3, TestName = "Simulate.B Step(30) Body(3) Velocity=1,2,-3")]
        /*
		After 40 steps:
		pos=<x= 14, y=-12, z= -4>, vel=<x= 11, y=  3, z=  0>
		pos=<x= -1, y= 18, z=  8>, vel=<x= -5, y=  2, z=  3>
		pos=<x= -5, y=-14, z=  8>, vel=<x=  1, y= -2, z=  0>
		pos=<x=  0, y=-12, z= -2>, vel=<x= -7, y= -3, z= -3>
        */
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 40, 0, 11, 3, 0, TestName = "Simulate.B Step(40) Body(0) Velocity=11,3,0")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 40, 1, -5, 2, 3, TestName = "Simulate.B Step(40) Body(1) Velocity=-5,2,3")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 40, 2, 1, -2, 0, TestName = "Simulate.B Step(40) Body(2) Velocity=1,-2,0")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 40, 3, -7, -3, -3, TestName = "Simulate.B Step(40) Body(3) Velocity=-7,-3,-3")]
        /*
		After 50 steps:
		pos=<x=-23, y=  4, z=  1>, vel=<x= -7, y= -1, z=  2>
		pos=<x= 20, y=-31, z= 13>, vel=<x=  5, y=  3, z=  4>
		pos=<x= -4, y=  6, z=  1>, vel=<x= -1, y=  1, z= -3>
		pos=<x= 15, y=  1, z= -5>, vel=<x=  3, y= -3, z= -3>
        */
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 50, 0, -7, -1, 2, TestName = "Simulate.B Step(50) Body(0) Velocity=-7,-1,2")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 50, 1, 5, 3, 4, TestName = "Simulate.B Step(50) Body(1) Velocity=5,3,4")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 50, 2, -1, 1, -3, TestName = "Simulate.B Step(50) Body(2) Velocity=-1,1,-3")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 50, 3, 3, -3, -3, TestName = "Simulate.B Step(50) Body(3) Velocity=3,-3,-3")]
        /*
		After 60 steps:
		pos=<x= 36, y=-10, z=  6>, vel=<x=  5, y=  0, z=  3>
		pos=<x=-18, y= 10, z=  9>, vel=<x= -3, y= -7, z=  5>
		pos=<x=  8, y=-12, z= -3>, vel=<x= -2, y=  1, z= -7>
		pos=<x=-18, y= -8, z= -2>, vel=<x=  0, y=  6, z= -1>
        */
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 60, 0, 5, 0, 3, TestName = "Simulate.B Step(60) Body(0) Velocity=5,0,3")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 60, 1, -3, -7, 5, TestName = "Simulate.B Step(60) Body(1) Velocity=-3,-7,5")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 60, 2, -2, 1, -7, TestName = "Simulate.B Step(60) Body(2) Velocity=-2,1,-7")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 60, 3, 0, 6, -1, TestName = "Simulate.B Step(60) Body(3) Velocity=0,6,-1")]
        /*
		After 70 steps:
		pos=<x=-33, y= -6, z=  5>, vel=<x= -5, y= -4, z=  7>
		pos=<x= 13, y= -9, z=  2>, vel=<x= -2, y= 11, z=  3>
		pos=<x= 11, y= -8, z=  2>, vel=<x=  8, y= -6, z= -7>
		pos=<x= 17, y=  3, z=  1>, vel=<x= -1, y= -1, z= -3>
        */
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 70, 0, -5, -4, 7, TestName = "Simulate.B Step(70) Body(0) Velocity=-5,-4,7")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 70, 1, -2, 11, 3, TestName = "Simulate.B Step(70) Body(1) Velocity=-2,11,3")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 70, 2, 8, -6, -7, TestName = "Simulate.B Step(70) Body(2) Velocity=8,-6,-7")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 70, 3, -1, -1, -3, TestName = "Simulate.B Step(70) Body(3) Velocity=-1,-1,-3")]
        /*
		After 80 steps:
		pos=<x= 30, y= -8, z=  3>, vel=<x=  3, y=  3, z=  0>
		pos=<x= -2, y= -4, z=  0>, vel=<x=  4, y=-13, z=  2>
		pos=<x=-18, y= -7, z= 15>, vel=<x= -8, y=  2, z= -2>
		pos=<x= -2, y= -1, z= -8>, vel=<x=  1, y=  8, z=  0>
        */
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 80, 0, 3, 3, 0, TestName = "Simulate.B Step(80) Body(0) Velocity=3,3,0")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 80, 1, 4, -13, 2, TestName = "Simulate.B Step(80) Body(1) Velocity=4,-13,2")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 80, 2, -8, 2, -2, TestName = "Simulate.B Step(80) Body(2) Velocity=-8,2,-2")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 80, 3, 1, 8, 0, TestName = "Simulate.B Step(80) Body(3) Velocity=1,8,0")]
        /*
		After 90 steps:
		pos=<x=-25, y= -1, z=  4>, vel=<x=  1, y= -3, z=  4>
		pos=<x=  2, y= -9, z=  0>, vel=<x= -3, y= 13, z= -1>
		pos=<x= 32, y= -8, z= 14>, vel=<x=  5, y= -4, z=  6>
		pos=<x= -1, y= -2, z= -8>, vel=<x= -3, y= -6, z= -9>
        */
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 90, 0, 1, -3, 4, TestName = "Simulate.B Step(90) Body(0) Velocity=1,-3,4")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 90, 1, -3, 13, -1, TestName = "Simulate.B Step(90) Body(1) Velocity=-3,13,-1")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 90, 2, 5, -4, 6, TestName = "Simulate.B Step(90) Body(2) Velocity=5,-4,6")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 90, 3, -3, -6, -9, TestName = "Simulate.B Step(90) Body(3) Velocity=-3,-6,-9")]
        /*
		After 100 steps:
		pos=<x=  8, y=-12, z= -9>, vel=<x= -7, y=  3, z=  0>
		pos=<x= 13, y= 16, z= -3>, vel=<x=  3, y=-11, z= -5>
		pos=<x=-29, y=-11, z= -1>, vel=<x= -3, y=  7, z=  4>
		pos=<x= 16, y=-13, z= 23>, vel=<x=  7, y=  1, z=  1>
        */
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 100, 0, -7, 3, 0, TestName = "Simulate.B Step(100) Body(0) Velocity=-7,3,0")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 100, 1, 3, -11, -5, TestName = "Simulate.B Step(100) Body(1) Velocity=3,11,-5")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 100, 2, -3, 7, 4, TestName = "Simulate.B Step(100) Body(2) Velocity=-3,7,4")]
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 100, 3, 7, 1, 1, TestName = "Simulate.B Step(100) Body(3) Velocity=7,1,1")]
        public void SimulateVelocity(string[] lines, int steps, int body, int expectedVelX, int expectedVelY, int expectedVelZ)
        {
            Program.ParseInput(lines);
            Program.Simulate(steps);
            Assert.That(Program.GetBody(body).VelX, Is.EqualTo(expectedVelX));
            Assert.That(Program.GetBody(body).VelY, Is.EqualTo(expectedVelY));
            Assert.That(Program.GetBody(body).VelZ, Is.EqualTo(expectedVelZ));
        }

        /*
		<x=-1, y=0, z=2>
		<x=2, y=-10, z=-7>
		<x=4, y=-8, z=8>
		<x=3, y=5, z=-1>
		Energy after 10 steps:
		pot: 2 + 1 + 3 =  6;   kin: 3 + 2 + 1 = 6;   total:  6 * 6 = 36
		pot: 1 + 8 + 0 =  9;   kin: 1 + 1 + 3 = 5;   total:  9 * 5 = 45
		pot: 3 + 6 + 1 = 10;   kin: 3 + 2 + 3 = 8;   total: 10 * 8 = 80
		pot: 2 + 0 + 4 =  6;   kin: 1 + 1 + 1 = 3;   total:  6 * 3 = 18
		Sum of total energy: 36 + 45 + 80 + 18 = 179
		In the above example, adding together the total energy for all moons after 10 steps produces the total energy in the system, 179.
		*/
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 10, 179, TestName = "Energy.A Step(10) 179")]
        /*
		<x=-8, y=-10, z=0>
		<x=5, y=5, z=10>
		<x=2, y=-7, z=3>
		<x=9, y=-8, z=-3>
		Energy after 100 steps:
		pot:  8 + 12 +  9 = 29;   kin: 7 +  3 + 0 = 10;   total: 29 * 10 = 290
		pot: 13 + 16 +  3 = 32;   kin: 3 + 11 + 5 = 19;   total: 32 * 19 = 608
		pot: 29 + 11 +  1 = 41;   kin: 3 +  7 + 4 = 14;   total: 41 * 14 = 574
		pot: 16 + 13 + 23 = 52;   kin: 7 +  1 + 1 =  9;   total: 52 *  9 = 468
		Sum of total energy: 290 + 608 + 574 + 468 = 1940
		*/
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 100, 1940, TestName = "Energy.B Step(100) 1940")]
        public void Energy(string[] lines, int steps, int expectedEnergy)
        {
            Program.ParseInput(lines);
            Assert.That(Program.Simulate(steps), Is.EqualTo(expectedEnergy));
        }

        /*
		pos=<x= -1, y=  0, z=  2>, vel=<x=  0, y=  0, z=  0>
		pos=<x=  2, y=-10, z= -7>, vel=<x=  0, y=  0, z=  0>
		pos=<x=  4, y= -8, z=  8>, vel=<x=  0, y=  0, z=  0>
		pos=<x=  3, y=  5, z= -1>, vel=<x=  0, y=  0, z=  0>

		After 2772 steps:
		pos=<x= -1, y=  0, z=  2>, vel=<x=  0, y=  0, z=  0>
		pos=<x=  2, y=-10, z= -7>, vel=<x=  0, y=  0, z=  0>
		pos=<x=  4, y= -8, z=  8>, vel=<x=  0, y=  0, z=  0>
		pos=<x=  3, y=  5, z= -1>, vel=<x=  0, y=  0, z=  0>
        */
        [Test]
        [TestCase(new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" }, 2772, TestName = "FindLoopPoint.A 2772")]
        /*
		<x=-8, y=-10, z=0>
		<x=5, y=5, z=10>
		<x=2, y=-7, z=3>
		<x=9, y=-8, z=-3>
		This set of initial positions takes 4686774924 steps before it repeats a previous state!
		*/
        [TestCase(new string[] { "<x=-8, y=-10, z=0>", "<x=5, y=5, z=10>", "<x=2, y=-7, z=3>", "<x=9, y=-8, z=-3>" }, 4686774924, TestName = "FindLoopPoint.B 4686774924")]
        public void FindLoopPoint(string[] lines, Int64 expectedLoop)
        {
            Program.ParseInput(lines);
            Assert.That(Program.FindLoopPoint(), Is.EqualTo(expectedLoop));
        }
    }
}
