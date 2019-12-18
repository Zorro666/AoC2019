using NUnit.Framework;

namespace Day08
{
    [TestFixture]
    public class Tests
    {
        [Test]
        [TestCase("123456789012", 3, 2, 2)]
        public void NumLayers(string source, int width, int height, int expected)
        {
            var layers = Program.ParseLayers(source, width, height);
            Assert.That(layers.Length, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("123456789012", 3, 2, 1)]
        public void CountZeros(string source, int width, int height, int expected)
        {
            var layers = Program.ParseLayers(source, width, height);
            var numZeroes = 0;
            foreach (var image in layers)
            {
                numZeroes += Program.CountZeroes(image, width, height);
            }
            Assert.That(numZeroes, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("123456789012", 3, 2, 0)]
        [TestCase("100456789012", 3, 2, 1)]
        public void LayerWithLowestZeroes(string source, int width, int height, int expected)
        {
            var layers = Program.ParseLayers(source, width, height);
            var bestLayer = Program.FindLayerWithLowestZeroes(layers, width, height);
            Assert.That(bestLayer, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("123456789012", 3, 2, 0, 1)]
        [TestCase("122456789012", 3, 2, 0, 2)]
        [TestCase("122156789012", 3, 2, 0, 4)]
        [TestCase("100456789012", 3, 2, 1, 1)]
        public void CountNumOnesAndTwosTotal(string source, int width, int height, int expectedLayer, int expectedTotal)
        {
            var layers = Program.ParseLayers(source, width, height);
            var bestLayer = Program.FindLayerWithLowestZeroes(layers, width, height);
            Assume.That(bestLayer, Is.EqualTo(expectedLayer));
            var total = Program.CountNumOnesAndTwosTotal(layers[bestLayer], width, height);
            Assert.That(total, Is.EqualTo(expectedTotal));
        }

        [Test]
        [TestCase("0222112222120000", 2, 2, 0, 0, '0')]
        [TestCase("0222112222120000", 2, 2, 1, 0, '1')]
        [TestCase("0222112222120000", 2, 2, 0, 1, '1')]
        [TestCase("0222112222120000", 2, 2, 1, 1, '0')]
        [TestCase("021220112220221122000000", 3, 2, 0, 0, '0')]
        [TestCase("021220112220221122000000", 3, 2, 1, 0, '1')]
        [TestCase("021220112220221122000000", 3, 2, 2, 0, '1')]
        [TestCase("021220112220221122000000", 3, 2, 0, 1, '1')]
        [TestCase("021220112220221122000000", 3, 2, 1, 1, '0')]
        [TestCase("021220112220221122000000", 3, 2, 2, 1, '0')]
        public void GetFinalPixel(string source, int width, int height, int x, int y, char expected)
        {
            var layers = Program.ParseLayers(source, width, height);
            var finalImage = Program.GetFinalImage(layers, width, height);
            Assert.That(finalImage[x, y], Is.EqualTo(expected));
        }
    }
}
