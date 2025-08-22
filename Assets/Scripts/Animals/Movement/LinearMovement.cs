using UnityEngine;

public class LinearMovement : IMovementBehavior
{
    private Rigidbody rb; 
    private BoundaryService bounds;
    private Vector3 dir; 
    private readonly float speed;

    private LayerMask obstacleMask;
    private readonly float castRadius = 0.35f;
    private readonly float lookAhead = 1.2f;
    private readonly float turnStrength = 0.35f;

    public LinearMovement(float speed)
    {
        this.speed = speed;
    }

    public void Init(Rigidbody rb, BoundaryService b)
    {
        this.rb = rb;
        bounds = b;
        obstacleMask = LayerMask.GetMask("Obstacle");
        dir = Random.onUnitSphere;
        dir.y = 0f;
        dir.Normalize();
    }

    public void Tick()
    {
        if (bounds.IsOutside(rb.transform))
            dir = bounds.ReflectTowardsCenter(rb.transform, dir);
        dir = ObstacleAvoidance.Steer(dir, rb.position, castRadius, lookAhead, turnStrength, obstacleMask);
        rb.linearVelocity = dir * speed;
    }

    public Vector3 CurrentDirection => dir;
}
