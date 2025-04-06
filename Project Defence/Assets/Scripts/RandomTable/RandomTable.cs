using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomTable
{
    private static int randomPointer = 0;
    private static float[] randomFloatTable = {
    -0.56f, 0.87f, -0.23f, 0.45f, -0.91f, 0.12f, 0.78f, -0.34f, 0.67f, -0.89f,
    0.25f, -0.73f, 0.92f, -0.15f, 0.58f, -0.47f, 0.31f, -0.62f, 0.84f, -0.99f,
    0.41f, -0.12f, 0.76f, -0.29f, 0.53f, -0.68f, 0.19f, -0.81f, 0.62f, -0.37f
};
    public static float GetRandomFloatBetweenMinusOneToOne()
    {
        float randomFloat = randomFloatTable[randomPointer];
        randomPointer = (randomPointer + 1) % (randomFloatTable.Length - 1);
        return randomFloat;
    }
    public static float[] GetRandomFloatsBetweenMinusOneToOne(int amount)
    {
        float[] randomFloats = new float[amount];
        for (int i = 0; i < amount; i++)
        {
            randomFloats[i] = randomFloatTable[randomPointer];
            randomPointer = (randomPointer + 1) % (randomFloatTable.Length - 1);
        }
        return randomFloats;

    }
}
