using UnityEngine;
using VContainer;
using VContainer.Unity;

public class PrefabFactory : IEntityFactory
{
    [System.Serializable]
    public class Settings
    {
        public Frog frogPrefab;
        public Snake snakePrefab;
        public Transform spawnRoot;
    }

    private readonly Settings settings;
    private readonly IObjectResolver resolver;
    
    public PrefabFactory(Settings settings, IObjectResolver resolver)
    {
        this.settings = settings;
        this.resolver = resolver;
    }

    public T Create<T>(T prefab, Vector3 pos, Quaternion rot) where T : Component
    {
        T instance = Object.Instantiate(prefab, pos, rot, settings.spawnRoot);
        // ⬇️ This line performs [Inject] on all components in the hierarchy
        resolver.InjectGameObject(instance.gameObject);
        return instance;
    }

    public Settings GetSettings() => settings;
}
