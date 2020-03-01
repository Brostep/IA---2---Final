using System.Collections.Generic;
using UnityEngine;

public class NodeGrid : MonoBehaviour {
    public LayerMask obstacleMask;
    public Vector2 gridSize;
    public float nodeRadius;
    public float distance;
    public float h;

    private Node[,] _grid;
    public List<Node> finalPath;

    private float _nodeDiameter;
    private int _gridSizeX;
    private int _gridSizeY;
    
    private void Awake() {
        _nodeDiameter = nodeRadius * 2;
        _gridSizeX = Mathf.RoundToInt(gridSize.x / _nodeDiameter);
        _gridSizeY = Mathf.RoundToInt(gridSize.y / _nodeDiameter);
        CreateGrid();
    }

    private void CreateGrid() {
        _grid = new Node[_gridSizeX, _gridSizeY];

        var bottomLeft = transform.position - Vector3.right * gridSize.x / 2 - Vector3.forward * gridSize.y / 2;

        for (int y = 0; y < _gridSizeY; y++) {
            for (int x = 0; x < _gridSizeX; x++) {
                var worldPoint = bottomLeft + Vector3.right * (x * _nodeDiameter + nodeRadius) + Vector3.forward * (y * _nodeDiameter + nodeRadius) + Vector3.up * h;
                var obstacle = true;

                if(Physics.CheckSphere(worldPoint, nodeRadius, obstacleMask))
                    obstacle = false;
                
                _grid[x, y] = new Node(obstacle, worldPoint, x, y);
            }
        }
    }

    public Node NodeFromWorldPosition(Vector3 worldPosition) {
        float xPoint = ((worldPosition.x + gridSize.x / 2) / gridSize.x);
        float yPoint = ((worldPosition.z + gridSize.y / 2) / gridSize.y);

        xPoint = Mathf.Clamp01(xPoint);
        yPoint = Mathf.Clamp01(yPoint);

        int x = Mathf.RoundToInt((_gridSizeX - 1) * xPoint);
        int y = Mathf.RoundToInt((_gridSizeY - 1) * yPoint);

        return _grid[x, y];
    }

    public List<Node> GetNeighborNodes(Node current) {
        List<Node> neighborNodes = new List<Node>();
        int xCheck;
        int yCheck;

        xCheck = current.gridX + 1;
        yCheck = current.gridY;

        if (xCheck >= 0 && xCheck < _gridSizeX)
            if (yCheck >= 0 && yCheck < _gridSizeY)
                neighborNodes.Add(_grid[xCheck, yCheck]);
            
        xCheck = current.gridX - 1;
        yCheck = current.gridY;

        if (xCheck >= 0 && xCheck < _gridSizeX)
            if (yCheck >= 0 && yCheck < _gridSizeY)
                neighborNodes.Add(_grid[xCheck, yCheck]);
            
        xCheck = current.gridX;
        yCheck = current.gridY + 1;

        if (xCheck >= 0 && xCheck < _gridSizeX)
            if (yCheck >= 0 && yCheck < _gridSizeY)
                neighborNodes.Add(_grid[xCheck, yCheck]);
            
        xCheck = current.gridX;
        yCheck = current.gridY - 1;

        if (xCheck >= 0 && xCheck < _gridSizeX)
            if (yCheck >= 0 && yCheck < _gridSizeY)
                neighborNodes.Add(_grid[xCheck, yCheck]);
           
        xCheck = current.gridX - 1;
        yCheck = current.gridY - 1;

        if (xCheck >= 0 && xCheck < _gridSizeX)
            if (yCheck >= 0 && yCheck < _gridSizeY)
                neighborNodes.Add(_grid[xCheck, yCheck]);
            
        xCheck = current.gridX + 1;
        yCheck = current.gridY - 1;

        if (xCheck >= 0 && xCheck < _gridSizeX)
            if (yCheck >= 0 && yCheck < _gridSizeY)
                neighborNodes.Add(_grid[xCheck, yCheck]);
          
        
        xCheck = current.gridX - 1;
        yCheck = current.gridY + 1;

        if (xCheck >= 0 && xCheck < _gridSizeX)
            if (yCheck >= 0 && yCheck < _gridSizeY)
                neighborNodes.Add(_grid[xCheck, yCheck]);
            
        xCheck = current.gridX + 1;
        yCheck = current.gridY + 1;

        if (xCheck >= 0 && xCheck < _gridSizeX)
            if (yCheck >= 0 && yCheck < _gridSizeY)
                neighborNodes.Add(_grid[xCheck, yCheck]);
            
        return neighborNodes;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridSize.x, 1, gridSize.y));

        if (_grid != null) {
            foreach (Node node in _grid) {
                Gizmos.color = node.isObstacle ? Color.white : Color.red;

                if(finalPath != null)
                    Gizmos.color = Color.green;
                
                Gizmos.DrawWireSphere(node.position, nodeRadius);
            }
        }
    }
}
