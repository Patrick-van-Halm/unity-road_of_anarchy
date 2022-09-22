using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public static class ListExtensions
{
    public static T Random<T>(this IEnumerable<T> enumerator)
    {
        return enumerator.ElementAt(UnityEngine.Random.Range(0, enumerator.Count()));
    }
}
