using NUnit.Framework;

namespace Day21
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void WalkingLogic([Values()]bool A, [Values()]bool B, [Values()]bool C, [Values()]bool D)
        {
            bool T = false;
            bool J = false;

            /* "NOT A T", */
            T = !A;
            /* "NOT T T", */
            T = !T;
            /* "AND B T", */
            T &= B;
            /* "AND C T", */
            T &= C;
            /* "NOT T J", */
            J = !T;
            /* "AND D J", */
            J &= D;

            // What we want it to do
            //                A B C D
            //@...# -> JUMP : 0 0 0 1
            //@..## -> JUMP : 0 0 1 1
            //@.#.# -> JUMP : 0 1 0 1
            //@.### -> JUMP : 0 1 1 1 

            //@#..# -> JUMP : 1 0 0 1
            //@#.## -> JUMP : 1 0 1 1

            //@##.# -> JUMP : 1 1 0 1

            //@#### -> WALK : 1 1 1 1
            //@???. -> WALK : ? ? ? 0
            bool expected = D & !(A & B & C);
            Assert.That(J, Is.EqualTo(expected));
        }

        [Test]
        public void RunningLogic([Values()]bool A, [Values()]bool B, [Values()]bool C, [Values()]bool D,
                                [Values()]bool E, [Values()]bool F, [Values()]bool G, [Values()]bool H,
                                [Values()]bool I)
        {
            bool T = false;
            bool J = false;

            // ABCDEFGHI
            //@.???????? : JUMP : !A
            //@?.?#.???? : JUMP : !B & !E
            //@??.##???? : JUMP : !C & D & E
            //@??.#???#? : JUMP : !C & D & H

            // J = !C & D & (H | E)
            //"NOT C J",
            J = !C;
            //"AND D J",
            J &= D;
            //"NOT E T",
            T = !E;
            //"NOT T T",
            T = !T;
            //"OR H T",
            T |= H;
            //"AND T J",
            // J = !C & D & (H | E)
            J &= T;

            // T = !(B | E)
            //"NOT B T",
            T = !B;
            //"NOT T T",
            T = !T;
            //"OR E T",
            T |= E;
            //"NOT T T",
            T = !T;
            // J = (!C & D & (H | E)) | (!B & !E)
            //"OR T J",
            J |= T;

            // T = !A
            //"NOT A T",
            T = !A;
            //"OR T J",
            // J = (!C & D & (H | E)) | (!B & !E) | !A
            J |= T;

            bool expected = (!C & D & (H | E)) | (!B & !E) | !A;
            Assert.That(J, Is.EqualTo(expected));
        }
    }
}
