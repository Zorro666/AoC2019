using NUnit.Framework;

namespace Day21
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void JumpingLogic([Values()]bool A, [Values()]bool B, [Values()]bool C, [Values()]bool D)
        {
            bool T = false;
            bool J = false;

            /* "NOT B T", */
            T = !B;
            /* "OR T J", */
            J |= T;

            /* "NOT C T", */
            T = !C;
            /* "AND B T", */
            T &= B;
            /* "OR T J", */
            J |= T;

            /* "AND A J", */
            J &= A;

            /* "NOT A T", */
            T = !A;
            /* "OR T J", */
            J |= T;
            /* "AND D J" */
            J &= D;

            bool expected = false;
            // What we want it to do
            //                A B C D
            //@...# -> JUMP : 0 0 0 1
            //@..## -> JUMP : 0 0 1 1
            //@.#.# -> JUMP : 0 1 0 1
            //@.### -> JUMP : 0 1 1 1 

            //@#..# -> JUMP : 1 0 0 1
            //@#.## -> JUMP : 1 0 1 1

            //@##.# -> JUMP : 1 1 0 1
            (bool A, bool B, bool C, bool D)[] jumpConditions = {
                (false, false, false, true),
                (false, false, true, true),
                (false, true, false, true),
                (false, true, true, true),
                (true, false, false, true),
                (true, false, true, true),
                (true, true, false, true),
            };
            foreach (var jumpCondition in jumpConditions)
            {
                if ((A == jumpCondition.A) && (B == jumpCondition.B) && (C == jumpCondition.C) && (D == jumpCondition.D))
                {
                    expected = true;
                    break;
                }
            }
            Assert.That(J, Is.EqualTo(expected));
        }
    }
}
