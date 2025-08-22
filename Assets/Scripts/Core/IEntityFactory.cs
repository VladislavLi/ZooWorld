using UnityEngine;

public interface IEntityFactory
{
    T Create<T>(T prefab, Vector3 pos, Quaternion rot) where T : Component;
}
