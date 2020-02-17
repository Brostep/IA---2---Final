using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding
{
    private static NodeGrid _grid;
    private const float OFFSET = 0.1f;

    public static List<Node> GetAStarPath(Vector3 startPosition, Vector3 endPosition, NodeGrid grid)
    {
        _grid = grid;
        var startNode = _grid.NodeFromWorldPosition(startPosition);
        var endNode = _grid.NodeFromWorldPosition(endPosition);

        if(grid == null || startPosition == endPosition || startNode == null || endNode == null)
        {
            return null;
        }

        var openList = new List<Node>();
        var closedList = new HashSet<Node>();

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            var currentNode = openList[0];
            for(int i = 1; i< openList.Count; i++)
            {
                if(openList[i].fCost <= currentNode.fCost && openList[i].hCost < currentNode.hCost)
                {
                    currentNode = openList[i];
                }
            }
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if(currentNode == endNode)
            {
                break;
            }
            var n = _grid.GetNeighborNodes(currentNode);
            foreach (var neighbor in n)
            {
                if(!neighbor.isObstacle || closedList.Contains(neighbor))
                {
                    continue;
                }
                var moveCost = currentNode.gCost + GetDistance(currentNode, neighbor);
                if(moveCost < neighbor.gCost || !openList.Contains(neighbor))
                {
                    neighbor.gCost = moveCost;
                    neighbor.hCost = GetDistance(neighbor, endNode);
                    neighbor.previous = currentNode;

                    if (!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                    }
                }
            }
        }

        return ThetaStarPath(GetFinalPath(startNode, endNode));
    }

    private static float GetDistance(Node currentNode, Node neighbor)
    {
        var ix = Mathf.Abs(currentNode.gridX - neighbor.gridX);
        var iy = Mathf.Abs(currentNode.gridY - neighbor.gridY);

        return Mathf.Sqrt(ix * ix + iy * iy);
    }

    private static List<Node> GetFinalPath(Node startNode, Node endNode)
    {
        var finalPath = new List<Node>();
        var currentNode = endNode;
        while(currentNode != startNode)
        {
            finalPath.Add(currentNode);
            currentNode = currentNode.previous;
        }
        finalPath.Reverse();
        return finalPath;
    }

    private static List<Node> ThetaStarPath(List<Node> aStarPath)
    {
        for (int i = 0; i < aStarPath.Count; i++)
        {
            for (int j = aStarPath.Count -1; j > i; j--)
            {
                RaycastHit hit;
                var origin = aStarPath[i].position;
                var dir = aStarPath[j].position - aStarPath[i].position;
                var distance = Mathf.Abs(dir.magnitude);
                if (!Physics.Raycast(origin,dir, out hit, distance + OFFSET))
                {
                    var newDistance = aStarPath[i].gCost + distance;
                    if(newDistance < aStarPath[j].gCost)
                    {
                        aStarPath[j].previous = aStarPath[i];
                        aStarPath[j].gCost = newDistance;
                        if(j == aStarPath.Count - 1)
                        {
                            return GetFinalPath(aStarPath[0], aStarPath[aStarPath.Count - 1]);
                        }

                        for (int k = i+j; k < aStarPath.Count; k++)
                        {
                            aStarPath[k].gCost = aStarPath[k].previous.gCost + Vector3.Distance(aStarPath[k].position, aStarPath[k].previous.position);
                        }
                    }
                }
            }
        }
        return GetFinalPath(aStarPath[0], aStarPath[aStarPath.Count - 1]);
    }
}
