using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticEvolution : MonoBehaviour
{
    public static float[,] IsingStep(float[,] mapData, float temperature)
    {
        System.Random rnd = new System.Random();
        int N = mapData.GetLength(0);
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                int a = rnd.Next(0, N);
                int b= rnd.Next(0, N);
                float s = mapData[a, b];
                try
                {
                    float nb = mapData[(a == N - 1) ? 0 : a + 1, b] + mapData[a, (b == N - 1) ? 0 : b + 1] + mapData[(a == 0) ? N - 1 : a - 1, b] + mapData[a, (b == 0) ? N - 1 : b - 1];
                    float cost = 2 * s * nb;
                    if (cost < 0)
                    {
                        s *= -1;
                    }
                    else if ((float)rnd.NextDouble() < Mathf.Exp(-cost / temperature))
                    {
                        s *= -1;
                    }
                    mapData[a, b] = s;
                }
                catch (System.IndexOutOfRangeException exception)
                {
                    Debug.Log(exception + "exception, indeces are:");
                }
            }
        }

        return mapData;
    }
}
