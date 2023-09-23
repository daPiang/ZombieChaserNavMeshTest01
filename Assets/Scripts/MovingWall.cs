using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingWall : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private PatrolPoints patrolPoints;
    
    private Transform currentPoint;
    private Vector3 wallFlat;
    private Vector3 waypointFlat;
    
    private void Start() {
        currentPoint = patrolPoints.GetNextWaypoint(currentPoint);
    }

    private void Update() {
        GetFlats();

        if(currentPoint != null)
        {   
            transform.position = Vector3.MoveTowards(transform.position, waypointFlat, moveSpeed * Time.deltaTime);
        }

        if(wallFlat == waypointFlat)
        {
            currentPoint = patrolPoints.GetNextWaypoint(currentPoint);
        }
    }

    private void GetFlats()
    {
        wallFlat = new(
            transform.position.x,
            transform.position.y,
            transform.position.z
        );

        waypointFlat = new(
            currentPoint.position.x,
            transform.position.y,
            currentPoint.position.z
        );
    }
}
