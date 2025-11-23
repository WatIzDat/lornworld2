using System;
using UnityEngine;

public static class MathHelper
{
    public static int MathematicalMod(int x, int m)
    {
        int r = x % m;

        return r < 0 ? r + m : r;
    }

    public static float WhiteNoise(int seed, int x, int y)
    {
        unchecked
        {
            int h = seed;

            h += x;
            h += h << 10;
            h ^= h >> 6;

            h += y;
            h += h << 10;
            h ^= h >> 6;

            h += h << 3;
            h ^= h >> 11;
            h += h << 15;

            System.Random random = new(h);

            return (float)random.NextDouble();
        }
    }
}
