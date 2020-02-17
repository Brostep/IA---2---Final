using System.Collections.Generic;
using System;
using UnityEngine;

public class LazyAStar
{
    public static IEnumerable<Tuple<bool, Node, IEnumerable<Node>>> Run<Node>(
        Node start,
        Func<Node, bool> satisfies,
        Func<Node,List<Tuple<Node,float>>> expand,
        Func<Node,Node,float> heuristic,
        Func<Node,Node,bool> equal)
    {
        var open = new PriorityQueue<Node>();
        open.Enqueue(start, 0);

        var closed = new HashSet<Node>();

        var parents = new Dictionary<Node, Node>();

        var costs = new Dictionary<Node, float>();
        costs[start] = 0;

        var watchdog = 0;
        while (!open.IsEmpty)
        {
            var current = open.Dequeue();

            var eq = false;
            foreach (var item in closed)
            {
                if (equal(item, current)) eq = true;
            }
            if (eq)
            {
                if (satisfies(current))
                {
                    yield return Tuple.Create(satisfies(current),
                                            current,
                                            EnumerableUtils.GeneratePath(current, parents)
                                        );
                    break;
                }
                else
                    continue;
            }

            var currentCost = costs[current];
            closed.Add(current);

            var neighbours = expand(current);
            foreach(var childPair in neighbours)
            {
                var child = childPair.Item1;
                var childCost = childPair.Item2;

                var tentativeCost = currentCost + childCost;
                if (costs.ContainsKey(child) && tentativeCost > costs[child])
                {
                    Debug.Log("GCost: " + costs[child] + "| HCost: " + heuristic(child, current) + "| FCost: " + tentativeCost + heuristic(child, current));
                    continue;
                }

                parents[child] = current;
                costs[child] = tentativeCost;
                open.Enqueue(child, tentativeCost + heuristic(child, current));
                //Debug.Log("GCost: " + costs[child] + "| HCost: " + heuristic(child, current) + "| FCost: " + (tentativeCost + heuristic(child, current)));
            }

            yield return Tuple.Create(satisfies(current),
                                            current,
                                            EnumerableUtils.GeneratePath(current, parents)
                                        );
            if (satisfies(current))
            {
                break;
            }
            watchdog++;
            if(watchdog >= 10000)
            {
                Debug.Log("watchdog break");
                break;
            }
        }
    }
}
