using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic;

namespace AdventDay8
{
    class Layer
    {
        public char[] Data;


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

        public Layer(char[] data)
        {
            Data = data;
        }
    }

    class ImageReader
    {
        private int FindLayerWithFewestZeros(Layer[] layers)
        {
            int? leastZeroCount = null;
            int leastAddress = 0;

            for (int x = 0; x < layers.Length; x++)
            {
                int zeroCount = layers[x].GetNumberOfZeros();

                if (leastZeroCount > zeroCount || leastZeroCount == null)
                {
                    leastZeroCount = zeroCount;
                    leastAddress = x;
                }
            }

            return leastAddress;
        }

        private Layer[] GetLayers(string str, int width, int height)
        {
            int layerLength = width * height;
            int numberOfLayers = str.Length / layerLength;

            List<Layer> layers = new List<Layer>();
            for (int x = 0; x < numberOfLayers; x++)
            {
                layers.Add(new Layer(str.Skip(x * layerLength).Take(layerLength).ToArray()));
            }

            return layers.ToArray();
        }

        public int Process(string str, int width, int height)
        {
            Layer[] layers = GetLayers(str, width, height);

            return layers[FindLayerWithFewestZeros(layers)].OnesMultipliedByTwos();
        }

        public ImageReader()
        {

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
                ImageReader imageReader = new ImageReader();
                int answer = imageReader.Process(str, 25, 6);

                Console.WriteLine("question 1 answer {0}", answer);
                Console.ReadLine();
            }
        }
    }
}
