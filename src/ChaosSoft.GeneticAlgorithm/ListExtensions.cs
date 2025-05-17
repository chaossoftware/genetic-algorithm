using System.Collections.Generic;
using System;

namespace ChaosSoft.GeneticAlgorithm;

internal static class ListExtensions
{
    private static readonly Random Random = new();

    internal static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
