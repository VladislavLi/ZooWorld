using UnityEngine;

public class Frog : BaseAnimal
{
    public float jumpImpulse = 4f;
    public float jumpPeriod = 1.2f;

    public override AnimalKind Kind => AnimalKind.Prey;

    protected override void Start()
    {
        move = new JumpMovement(jumpImpulse, jumpPeriod);
        base.Start();
        if (GetComponentInChildren<MeshRenderer>() == null)
        {
            GameObject prim = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            prim.transform.SetParent(transform, false);
            prim.transform.localScale = new Vector3(0.8f, 0.5f, 0.8f);
        }
    }
}
