using System;
using System.IO;

/*

--- Day 8: Space Image Format ---

The Elves' spirits are lifted when they realize you have an opportunity to reboot one of their Mars rovers, and so they are curious if you would spend a brief sojourn on Mars. You land your ship near the rover.

When you reach the rover, you discover that it's already in the process of rebooting! It's just waiting for someone to enter a BIOS password. The Elf responsible for the rover takes a picture of the password (your puzzle input) and sends it to you via the Digital Sending Network.

Unfortunately, images sent via the Digital Sending Network aren't encoded with any normal encoding; instead, they're encoded in a special Space Image Format. None of the Elves seem to remember why this is the case. They send you the instructions to decode it.

Images are sent as a series of digits that each represent the color of a single pixel. The digits fill each row of the image left-to-right, then move downward to the next row, filling rows top-to-bottom until every pixel of the image is filled.

Each image actually consists of a series of identically-sized layers that are filled in this way. So, the first digit corresponds to the top-left pixel of the first layer, the second digit corresponds to the pixel to the right of that on the same layer, and so on until the last digit, which corresponds to the bottom-right pixel of the last layer.

For example, given an image 3 pixels wide and 2 pixels tall, the image data 123456789012 corresponds to the following image layers:

Layer 1: 123
         456

Layer 2: 789
         012
The image you received is 25 pixels wide and 6 pixels tall.

To make sure the image wasn't corrupted during transmission, the Elves would like you to find the layer that contains the fewest 0 digits. On that layer, what is the number of 1 digits multiplied by the number of 2 digits?

--- Part Two ---

Now you're ready to decode the image. The image is rendered by stacking the layers and aligning the pixels with the same positions in each layer. The digits indicate the color of the corresponding pixel: 0 is black, 1 is white, and 2 is transparent.

The layers are rendered with the first layer in front and the last layer in back. So, if a given position has a transparent pixel in the first and second layers, a black pixel in the third layer, and a white pixel in the fourth layer, the final image would have a black pixel at that position.

For example, given an image 2 pixels wide and 2 pixels tall, the image data 0222112222120000 corresponds to the following image layers:

Layer 1: 02
         22

Layer 2: 11
         22

Layer 3: 22
         12

Layer 4: 00
         00
Then, the full image can be found by determining the top visible pixel in each position:

The top-left pixel is black because the top layer is 0.
The top-right pixel is white because the top layer is 2 (transparent), but the second layer is 1.
The bottom-left pixel is white because the top two layers are 2, but the third layer is 1.
The bottom-right pixel is black because the only visible pixel in that position is 0 (from layer 4).
So, the final image looks like this:

01
10

What message is produced after decoding your image?

*/

namespace Day08
{
    class Program
    {
        private Program(string inputFile, bool part1)
        {
            var imageSource = ReadProgram(inputFile);
            var width = 25;
            var height = 6;
            var layers = ParseLayers(imageSource, width, height);
            if (part1)
            {
                var bestLayer = FindLayerWithLowestZeroes(layers, width, height);
                if ((bestLayer < 0) || (bestLayer > layers.Length))
                {
                    throw new InvalidDataException($"Invalid bestLayer {bestLayer} 0-{layers.Length}");
                }
                var total = CountNumOnesAndTwosTotal(layers[bestLayer], width, height);
                Console.WriteLine($"Day08 : Result1 {total}");
            }
            else
            {
                var finalImage = GetFinalImage(layers, width, height);
                for (var y = 0; y < height; ++y)
                {
                    var line = "";
                    for (var x = 0; x < width; ++x)
                    {
                        char pixel = finalImage[x, y];
                        if (pixel == '1')
                        {
                            line += '#';
                        }
                        else
                        {
                            line += ' ';
                        }
                    }
                    Console.WriteLine($"Day08 : Result2 {line}");
                }
            }
        }

        private string ReadProgram(string inputFile)
        {
            var imageSource = File.ReadAllText(inputFile);
            return imageSource;
        }

        public static int CountNumOnesAndTwosTotal(char[,] image, int width, int height)
        {
            var numOnes = 0;
            var numTwos = 0;
            for (var x = 0; x < width; ++x)
            {
                for (var y = 0; y < height; ++y)
                {
                    char pixel = image[x, y];
                    if (pixel == '1')
                    {
                        ++numOnes;
                    }
                    if (pixel == '2')
                    {
                        ++numTwos;
                    }
                }
            }
            var total = numOnes * numTwos;
            return total;
        }

        public static int FindLayerWithLowestZeroes(char[][,] layers, int width, int height)
        {
            int bestLayer = int.MaxValue;
            int lowestZeros = int.MaxValue;
            var numLayers = layers.Length;
            for (var layer = 0; layer < numLayers; ++layer)
            {
                var image = layers[layer];
                int numZeros = CountZeroes(image, width, height);
                if (numZeros < lowestZeros)
                {
                    lowestZeros = numZeros;
                    bestLayer = layer;
                }
            }
            return bestLayer;
        }

        public static int CountZeroes(char[,] image, int width, int height)
        {
            var numZeroes = 0;
            for (var x = 0; x < width; ++x)
            {
                for (var y = 0; y < height; ++y)
                {
                    char pixel = image[x, y];
                    if (pixel == '0')
                    {
                        ++numZeroes;
                    }
                }
            }
            return numZeroes;
        }

        public static char[,] GetFinalImage(char[][,] layers, int width, int height)
        {
            var finalImage = new char[width, height];
            var numLayers = layers.Length;
            for (var y = 0; y < height; ++y)
            {
                for (var x = 0; x < width; ++x)
                {
                    char finalPixel = '2';
                    for (var layer = 0; layer < numLayers; ++layer)
                    {
                        var image = layers[layer];
                        char pixel = image[x, y];
                        if (finalPixel == '2')
                        {
                            if (pixel != '2')
                            {
                                finalPixel = pixel;
                            }
                        }
                    }
                    finalImage[x, y] = finalPixel;
                }
            }
            return finalImage;
        }

        public static char[][,] ParseLayers(string imageSource, int width, int height)
        {
            var imageSize = width * height;
            var numLayers = imageSource.Length / imageSize;
            char[][,] layers = new char[numLayers][,];
            for (var layer = 0; layer < numLayers; ++layer)
            {
                var index = layer * imageSize;
                var image = ParseImage(imageSource, index, width, height);
                layers[layer] = image;
            }

            if (layers.Length != numLayers)
            {
                throw new InvalidDataException($"Bad numLayers {layers.Length} expecting {numLayers}");
            }
            return layers;
        }

        static char[,] ParseImage(string imageSource, int startIndex, int width, int height)
        {
            var image = new char[width, height];
            var index = startIndex;
            for (var y = 0; y < height; ++y)
            {
                for (var x = 0; x < width; ++x)
                {
                    char pixel = imageSource[index];
                    image[x, y] = pixel;
                    //Console.WriteLine($"{x} {y} {pixel} {index}");
                    ++index;
                }
            }
            return image;
        }

        public static void Run()
        {
            Console.WriteLine("Day08 : Start");
            _ = new Program("Day08/input.txt", true);
            _ = new Program("Day08/input.txt", false);
            Console.WriteLine("Day08 : End");
        }
    }
}
