using UnityEngine;

public interface IMovementBehavior
{
    void Init(Rigidbody rb, BoundaryService bounds);
    void Tick();
    Vector3 CurrentDirection { get; }
}
