using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class EnumerableUtils
{
    public static IEnumerable<T> Generate<T>(T seed, Func<T, T> mutate)
    {
        T accum = seed;
        while (true)
        {
            yield return accum;
            accum = mutate(accum);
        }
    }

    public static IEnumerable<int> CountForever()
    {
        return Generate(0, a => a + 1);
    }

    public static IEnumerable<Node> GeneratePath<Node>(Node end, Dictionary<Node, Node> parents)
    {
        var last = end;
        yield return last;

        while (parents.ContainsKey(last))
        {
            last = parents[last];
            yield return last;
        }
    }

    public static T Log<T>(T obj, string pre = "", string sub = "")
    {
        Debug.Log(pre + obj.ToString() + sub);
        return obj;
    }

}
