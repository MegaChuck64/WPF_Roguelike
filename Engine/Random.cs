
using RogueSharp.Random;

namespace Engine;

public static class Random
{
    private static IRandom random;

    public static int GetInt(int max, int min = 0)
    {
        if (random == null) random = new DotNetRandom();

        return random.Next(min, max);
    }
}

