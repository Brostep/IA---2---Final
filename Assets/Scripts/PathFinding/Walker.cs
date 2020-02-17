using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Walker : MonoBehaviour
{
    
    public float runSpeed { get { return _runSpeed; } set { _runSpeed = value; _posOffset = 0.4f * value; } }
    public Vector3 direction { get { return _dir; } }
    public bool ReachDestionation
    {
        get {
            var value = _reachDestionation;
            _reachDestionation = false;
            return value;
        }
    }

    [Header("Obstacle Avoidance Variables")]
    public bool obstacleAvoidance;
    [Range(0,1)]
    public float avoidWeight;
    public float avoidDistance;
    public LayerMask obstacleLayer;
    [Range(0, 1)]
    public float rotationSpeed;
    //[HideInInspector]
    public bool isStopped;

    public NodeGrid grid;
    private List<Node> _path;
    private Node actualNode;
    private int nodeIndex;
    private float _posOffset;
    private float _runSpeed;
    private Vector3 _finalDestionation;
    private Vector3 _dir;
    private bool _reachDestionation;

    private int _positionCount;
    private float _lastDist;

    private Collider[] obstacles;

	void Start ()
    {
        if(!grid)
            grid = FindObjectOfType<NodeGrid>();
        _path = new List<Node>();
    }
	
	void Update ()
    {
        if (!isStopped)
        {
            var obstacle = ObstaclesInSight();
            if (obstacleAvoidance)
            {
                if(obstacle != null)
                {
                    var avoidDir = (transform.position - obstacle.transform.position);
                    avoidDir.y = 0;
                    transform.position += avoidDir * _runSpeed * avoidWeight / 10 * Time.deltaTime;
                }
            }

            if (actualNode != null)
            {
                var offset = obstacleAvoidance && obstacle != null ? _posOffset + avoidDistance : _posOffset;
                if ((transform.position - actualNode.position).sqrMagnitude < offset && nodeIndex < _path.Count - 1)
                {
                    nodeIndex++;
                    actualNode = _path[nodeIndex];
                }
                var distToNode = (transform.position - _finalDestionation).sqrMagnitude;
                if(Mathf.Abs(_lastDist - distToNode) < 0.1f)
                {
                    _positionCount++;
                }
                else
                {
                    _positionCount = 0;
                }
                if (distToNode > _posOffset && _positionCount < 4)
                {
                    _dir = (actualNode.position - transform.position).normalized;
                    _dir.y = 0;
                    transform.position += _dir * _runSpeed * Time.deltaTime;
                    transform.forward = Vector3.Slerp(transform.forward, _dir, rotationSpeed);
                }
                else
                {
                    isStopped = true;
                    _reachDestionation = true;
                    _positionCount = 0;
                }

                _lastDist = distToNode;
            }
        }

        
	}

    private Transform ObstaclesInSight()
    {
        var obstacles = new List<Collider>() ;
        obstacles.AddRange(Physics.OverlapSphere(transform.position, avoidDistance, obstacleLayer));
        if(obstacles.Count > 0)
        {
            var closerObstacle = obstacles.OrderBy(x => Vector3.Distance(x.transform.position, transform.position)).First().transform;
            return closerObstacle;
        }
        return null;
    }

    public void SetDestination(Vector3 destination)
    {
        isStopped = false;
        _finalDestionation = destination;
        _path = PathFinding.GetAStarPath(transform.position, destination, grid);
        nodeIndex = 0;
        if(_path != null && _path.Count>0)
            actualNode = _path[0];
    }

    public void Stop()
    {
        isStopped = true;
        _finalDestionation = transform.position;
    }

    private void OnDrawGizmos()
    {
        if(_path != null)
        {
            Gizmos.color = Color.green;
            foreach (var node in _path)
            {
                Gizmos.DrawSphere(node.position, grid.nodeRadius);
            }
        }
        if(avoidDistance > 0)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, avoidDistance);
        }
    }
}
