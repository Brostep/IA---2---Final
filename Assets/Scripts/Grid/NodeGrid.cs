using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGrid : MonoBehaviour
{
    public LayerMask obstacleMask;
    public Vector2 gridSize;
    public float nodeRadius;
    public float distance;
    public float h;

    Node[,] grid;
    public List<Node> finalPath;

    float nodeDiameter;
    int gridSizeX, gridSizeY;


    private void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridSize.y / nodeDiameter);

        CreateGrid();
    }

    private void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];

        var bottomLeft = transform.position - Vector3.right * gridSize.x / 2 - Vector3.forward * gridSize.y / 2;
        for (int y = 0; y < gridSizeY; y++)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                var worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius) + Vector3.up * h;
                var obstacle = true;

                if(Physics.CheckSphere(worldPoint, nodeRadius, obstacleMask))
                {
                    obstacle = false;
                }

                grid[x, y] = new Node(obstacle, worldPoint, x, y);
            }
        }

    }

    public Node NodeFromWorldPosition(Vector3 worldPosition)
    {
        float xPoint = ((worldPosition.x + gridSize.x / 2) / gridSize.x);
        float yPoint = ((worldPosition.z + gridSize.y / 2) / gridSize.y);

        xPoint = Mathf.Clamp01(xPoint);
        yPoint = Mathf.Clamp01(yPoint);

        int x = Mathf.RoundToInt((gridSizeX - 1) * xPoint);
        int y = Mathf.RoundToInt((gridSizeY - 1) * yPoint);

        return grid[x, y];
    }

    public List<Node> GetNeighborNodes(Node current)
    {
        List<Node> neighborNodes = new List<Node>();
        int xCheck;
        int yCheck;

        //Right Side
        xCheck = current.gridX + 1;
        yCheck = current.gridY;
        if(xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighborNodes.Add(grid[xCheck, yCheck]);
            }
        }

        //Left Side
        xCheck = current.gridX - 1;
        yCheck = current.gridY;
        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighborNodes.Add(grid[xCheck, yCheck]);
            }
        }

        //Top Side
        xCheck = current.gridX;
        yCheck = current.gridY + 1;
        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighborNodes.Add(grid[xCheck, yCheck]);
            }
        }

        //Bot Side
        xCheck = current.gridX;
        yCheck = current.gridY - 1;
        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighborNodes.Add(grid[xCheck, yCheck]);
            }
        }

        //Bot-left Side
        xCheck = current.gridX - 1;
        yCheck = current.gridY - 1;
        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighborNodes.Add(grid[xCheck, yCheck]);
            }
        }
        //Bot-right Side
        xCheck = current.gridX + 1;
        yCheck = current.gridY - 1;
        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighborNodes.Add(grid[xCheck, yCheck]);
            }
        }
        //Top-left Side
        xCheck = current.gridX - 1;
        yCheck = current.gridY + 1;
        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighborNodes.Add(grid[xCheck, yCheck]);
            }
        }
        //Top-right Side
        xCheck = current.gridX + 1;
        yCheck = current.gridY + 1;
        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighborNodes.Add(grid[xCheck, yCheck]);
            }
        }

        return neighborNodes;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridSize.x, 1, gridSize.y));

        if(grid != null)
        {
            foreach(Node node in grid)
            {
                Gizmos.color = node.isObstacle ? Color.white : Color.red;

                if(finalPath != null)
                {
                    Gizmos.color = Color.green;
                }

                Gizmos.DrawWireSphere(node.position, nodeRadius);
            }
        }
    }
}
