using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Business.API.Extensions
{
    public static class MathsExtensions
    {
        public static int Modulo(this int x, int modulo)
        {
            int rest = x % modulo;
            return rest < 0 ? rest + modulo : rest;
        }

        public static void Shuffle<T>(this IList<T> list, Random rng)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
