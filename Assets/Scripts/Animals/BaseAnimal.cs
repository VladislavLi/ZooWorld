using UnityEngine;
using VContainer;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public abstract class BaseAnimal : MonoBehaviour, IAnimal
{
    [Inject] protected SignalBus Bus;
    [Inject] protected FoodChainService Food;
    [Inject] protected BoundaryService Bounds;
    [SerializeField] private float preyBumpImpulse = 2.5f; 
    
    protected IMovementBehavior move;
    protected Rigidbody rb;
    protected bool dead;

    public abstract AnimalKind Kind { get; }
    public int InstanceId => GetInstanceID();
    public Transform Transform => transform;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    protected virtual void Start()
    {
        move?.Init(rb, Bounds);
    }

    protected virtual void Update()
    {
        if (dead) return;
        move?.Tick();
        // Face movement direction
        Vector3 dir = move?.CurrentDirection ?? Vector3.zero;
        if (dir.sqrMagnitude > 0.001f)
        {
            // Flatten to XZ plane so they don't tilt upward
            dir.y = 0;
            Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 5f);
        }
    }

    protected virtual void OnCollisionEnter(Collision c)
    {
        if (dead) return;
        IAnimal other = c.collider.GetComponentInParent<IAnimal>();
        if (other == null || ReferenceEquals(other, this)) return;

        // Process each pair once: only the higher InstanceId resolves rules
        if (other.InstanceId > this.InstanceId) return;
        
        // Prey vs Prey: push apart with an impulse
        if (this.Kind == AnimalKind.Prey && other.Kind == AnimalKind.Prey)
        {
            // Contact normal points away from *this* collider
            Vector3 n = c.GetContact(0).normal;

            // Push this animal along its surface normal
            if (rb != null)
                rb.AddForce(n * preyBumpImpulse, ForceMode.VelocityChange);

            // Push the other animal in the opposite direction (if it has a Rigidbody)
            Rigidbody otherRb = (other as Component)?.GetComponent<Rigidbody>();
            if (otherRb != null)
                otherRb.AddForce(-n * preyBumpImpulse, ForceMode.VelocityChange);

            return;
        }

        // All other cases are governed by the food chain rules
        Food.Resolve(this, other);
    }

    public virtual void Die()
    {
        if (dead) return;
        dead = true;
        Bus.Fire(new AnimalDied(Kind));
        Destroy(gameObject);
    }
}
