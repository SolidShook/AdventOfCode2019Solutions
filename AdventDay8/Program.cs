using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Microsoft.VisualBasic;

namespace AdventDay8
{
    class Layer
    {
        //this could be in bytes if efficiency is wanted
        public char[] Data;
        public int Height;
        public int Width;

        public char GetValueAtIndex(int x, int y)
        {
            return Data[x + y * Width];
        }

        public int GetNumberOfZeros()
        {
            int zeroCount = 0;

            foreach (char value in Data)
            {
                if (value - '0' == 0)
                {
                    zeroCount++;
                }
            }

            return zeroCount;
        }

        public int OnesMultipliedByTwos()
        {
            int oneCount = 0;
            int twoCount = 0;

            foreach (int value in Data)
            {
                if (value - '0' == 1)
                {
                    oneCount++;
                }

                if (value - '0' == 2)
                {
                    twoCount++;
                }
            }

            return oneCount * twoCount;
        }

        public char[][] GetJaggedArray()
        {
            List<char[]> rows = new List<char[]>();

            for (int y = 0; y < Height; y++)
            {
                char[] row = Data.Skip(y * Width).Take(Width).ToArray();
                rows.Add(row);
            }

            return rows.ToArray();
        }

        public Layer(char[] data, int width, int height)
        {
            Data = data;
            Height = height;
            Width = width;
        }
    }

    class ImageReader
    {
        public Layer[] Layers;

        private int FindLayerWithFewestZeros()
        {
            int? leastZeroCount = null;
            int leastAddress = 0;

            for (int x = 0; x < Layers.Length; x++)
            {
                int zeroCount = Layers[x].GetNumberOfZeros();

                if (leastZeroCount > zeroCount || leastZeroCount == null)
                {
                    leastZeroCount = zeroCount;
                    leastAddress = x;
                }
            }

            return leastAddress;
        }

        private void SetLayers(string str, int width, int height)
        {
            int layerLength = width * height;
            int numberOfLayers = str.Length / layerLength;

            List<Layer> layers = new List<Layer>();
            for (int x = 0; x < numberOfLayers; x++)
            {
                layers.Add(new Layer(str.Skip(x * layerLength).Take(layerLength).ToArray(), width, height));
            }

            Layers = layers.ToArray();
        }

        public int Process()
        {
            return Layers[FindLayerWithFewestZeros()].OnesMultipliedByTwos();
        }

        public Layer FindCode()
        {
            int width = Layers[0].Width;
            int height = Layers[0].Height;

            char[] data = new char[height * width];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    char finalRowValue = 'f';

                    foreach (Layer layer in Layers)
                    {
                        char rowValue = layer.GetValueAtIndex(x, y);

                        if (rowValue == '0')
                        {
                            finalRowValue = ' ';
                            break;
                        }

                        if (rowValue == '1')
                        {
                            finalRowValue = (char) 380;
                            break;
                        }
                    }

                    data[x + y * width] = finalRowValue;
                }
            }

            return new Layer(data, width, height);
        }

        public void GetAnswer2()
        {
            Layer layer = FindCode();

            char[][] jagged = layer.GetJaggedArray();

            foreach (char[] thing in jagged)
            {
                Console.WriteLine(new string(thing));
            }
        }

        public ImageReader(string str, int width, int height)
        {
            SetLayers(str, width, height);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            System.IO.StreamReader file =
                new System.IO.StreamReader("../../../data.txt");

            string str = file.ReadToEnd();
            file.Close();

            while (true)
            {
                ImageReader imageReader = new ImageReader(str, 25, 6);
                int answer = imageReader.Process();

                imageReader.GetAnswer2();
                Console.WriteLine("question 1 answer {0}", answer);
                Console.ReadLine();
            }
        }
    }
}
