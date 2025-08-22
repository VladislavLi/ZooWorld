using UnityEngine;

public interface IAnimal
{
    AnimalKind Kind { get; }
    int InstanceId { get; }
    Transform Transform { get; }
    void Die();
}
