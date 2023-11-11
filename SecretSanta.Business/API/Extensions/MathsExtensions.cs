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
    }
}
