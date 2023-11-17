using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrollerBehaviour : EnemyBehaviour
{
    [Header("Patroller Data")]
    [SerializeField] private List<GameObject> waypoints;
    private Vector2 originWaypoint;
    private Vector2 destination;
    private Vector2 destinationDirection;
    public int waypointIndex = 1;

    private Rigidbody2D rb;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originWaypoint = transform.position;
        UpdateWaypoint();
    }

    private void GetLocation(Vector2 point)
    {
        Vector2 position2d = transform.position;
        destinationDirection = (point - position2d).normalized;
    }

    private void UpdateWaypoint()
    {
        if (waypointIndex > waypoints.Count)
        {
            waypointIndex = 0;
            destination = originWaypoint;
        }

        if (waypointIndex != 0)
        {
            destination = waypoints[waypointIndex - 1].transform.position;
        }
        GetLocation(destination);
    }

    public override void ResetBehaviour()
    {

    }

    public override void UpdatePhysicsBehaviour()
    {

    }

    public override void UpdateLogicBehaviour()
    {

        UpdateWaypoint();
        if ((destination - (Vector2)transform.position).magnitude < 0.1f)
        {
            rb.velocity = Vector2.zero;
            waypointIndex++;
            UpdateWaypoint();
        }
        //transform.rotation = Quaternion.LookRotation(destinationDirection);
        //transform.LookAt(destinationDirection);
        rb.velocity = destinationDirection;
    }

    private void RotateTowards(Vector2 target)
    {        
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; 
        var offset = 90f;
        transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));
    }
}
