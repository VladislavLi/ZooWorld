using UnityEngine;

public class Snake : BaseAnimal
{
    public float speed = 2.5f;

    public override AnimalKind Kind => AnimalKind.Predator;

    protected override void Start()
    {
        move = new LinearMovement(speed);
        base.Start();
        if (GetComponentInChildren<MeshRenderer>() == null)
        {
            GameObject prim = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            prim.transform.SetParent(transform, false);
            prim.transform.localScale = new Vector3(0.5f, 0.5f, 1.2f);
        }
    }
}
