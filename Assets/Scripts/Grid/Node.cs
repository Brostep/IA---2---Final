using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node  {

    public int gridX;
    public int gridY;

    public bool isObstacle;
    public Vector3 position;

    public Node previous;

    public float gCost;
    public float hCost;

    public float fCost { get { return gCost + hCost; } }

    public Node(bool isObs, Vector3 pos, int gX, int gY)
    {
        isObstacle = isObs;
        position = pos;
        gridX = gX;
        gridY = gY;
    }

    public void ResetNode()
    {
        gCost = 0;
        hCost = 0;
    }
}
