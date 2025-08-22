using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

public class JumpMovement : IMovementBehavior
{
    private Rigidbody rb; 
    private BoundaryService bounds;
    private Vector3 dir; 
    private readonly float jumpImpulse; 
    private readonly float period;
    private CancellationTokenSource cts;

    // Avoidance params
    private readonly float castRadius = 0.35f;
    private readonly float lookAhead = 1.5f;
    private readonly float turnStrength = 0.25f; // 0..1 blend per Tick
    private LayerMask obstacleMask;

    public JumpMovement(float jumpImpulse, float period)
    {
        this.jumpImpulse = jumpImpulse;
        this.period = period;
    }

    public void Init(Rigidbody rb, BoundaryService b)
    {
        this.rb = rb; 
        bounds = b;
        obstacleMask = LayerMask.GetMask("Obstacle");
        dir = Random.onUnitSphere; 
        dir.y = 0f; 
        dir.Normalize();
        
        //Auto-cancel when the Rigidbody/GameObject is destroyed
        var token = rb.GetCancellationTokenOnDestroy();
        Loop(token).Forget();
    }

    public void Tick()
    {
        if (bounds.IsOutside(rb.transform))
            dir = bounds.ReflectTowardsCenter(rb.transform, dir);
        // steer away if obstacle ahead
        dir = ObstacleAvoidance.Steer(dir, rb.position, castRadius, lookAhead, turnStrength, obstacleMask);
    }

    public Vector3 CurrentDirection => dir;

    private async UniTask Loop(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            if (rb == null) break;
            // steer just before jumping as well
            dir = ObstacleAvoidance.Steer(dir, rb.position, castRadius, lookAhead, turnStrength, obstacleMask);

            rb.AddForce(dir * jumpImpulse, ForceMode.VelocityChange);
            await UniTask.Delay(System.TimeSpan.FromSeconds(period), cancellationToken: token);
            // small random turn
            dir = (dir + new Vector3(Random.Range(-0.5f,0.5f), 0, Random.Range(-0.5f,0.5f))).normalized;
        }
    }
}
