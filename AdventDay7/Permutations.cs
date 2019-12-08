using System;
using System.Collections.Generic;
using System.Text;

namespace AdventDay7
{
    class Permutations
    {
        public static string Swap(string a, int i, int j)
        {
            char temp;
            char[] charArray = a.ToCharArray();
            temp = charArray[i];
            charArray[i] = charArray[j];
            charArray[j] = temp;
            string s = new string(charArray);
            return s;
        }

        private static void Permute(List<string> results, string str, int l, int r)
        {
            if (l == r)
                 results.Add(str);
            else
            {
                for (int i = l; i <= r; i++)
                {
                    str = Swap(str, l, i);
                    Permute(results, str, l + 1, r);
                    str = Swap(str, l, i);
                }
            }
        }

        public static List<string> GetPermutations(string str)
        {
            List<string> results = new List<string>();
            Permute(results, str, 0, str.Length - 1);

            return results;
        }
    }
}