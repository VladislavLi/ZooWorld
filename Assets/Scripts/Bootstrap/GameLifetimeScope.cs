using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [Header("Prefabs & Spawn Root")]
    [SerializeField] private Frog frogPrefab;
    [SerializeField] private Snake snakePrefab;
    [SerializeField] private Transform spawnRoot;
    
    [Header("UI Root (Canvas or parent)")]
    [SerializeField] private GameObject uiRoot; 
    
    protected override void Configure(IContainerBuilder builder)
    {
        // Build a settings object for the factory
        PrefabFactory.Settings settings = new PrefabFactory.Settings
        {
            frogPrefab = frogPrefab,
            snakePrefab = snakePrefab,
            spawnRoot = spawnRoot
        };

        // Core services
        builder.Register<SignalBus>(Lifetime.Singleton);
        builder.Register<ScoreService>(Lifetime.Singleton);
        builder.Register<BoundaryService>(Lifetime.Singleton);
        builder.Register<FoodChainService>(Lifetime.Singleton);
        builder.Register<TastyLabelService>(Lifetime.Singleton);

        // Factory
        builder.RegisterInstance(settings);
        builder.Register<PrefabFactory>(Lifetime.Singleton).As<IEntityFactory>();

        // Gameplay
        builder.Register<AnimalSpawner>(Lifetime.Singleton);

        // UI
        builder.Register<HudViewModel>(Lifetime.Singleton);
    }

    private void Start()
    {
        if (uiRoot != null)
            Container.InjectGameObject(uiRoot); 
        
        Container.Resolve<TastyLabelService>();
        Container.Resolve<AnimalSpawner>().Begin();
    }
}
