using UnityEngine;
using VContainer;
using Cysharp.Threading.Tasks;
using System.Threading;

public class AnimalSpawner
{
    private readonly IEntityFactory factory;
    private readonly PrefabFactory.Settings settings;
    private CancellationTokenSource cts;

    [Inject]
    public AnimalSpawner(IEntityFactory factory)
    {
        this.factory = factory;
        this.settings = (factory as PrefabFactory).GetSettings();
    }

    public void Begin()
    {
        cts?.Cancel();
        cts = new CancellationTokenSource();
        Loop(cts.Token).Forget();
    }

    private async UniTask Loop(CancellationToken token)
    {
        Camera cam = Camera.main;
        float planeY = 0f;

        while (!token.IsCancellationRequested)
        {
            float delay = Random.Range(1f, 2f);
            await UniTask.Delay(System.TimeSpan.FromSeconds(delay), cancellationToken: token);

            Vector3 vp = new Vector3(Random.value, Random.value, Mathf.Abs(cam.transform.position.y - planeY));
            Vector3 pos = cam.ViewportToWorldPoint(vp);
            pos.y = planeY;

            bool spawnSnake = Random.value < 0.5f;
            if (spawnSnake) factory.Create(settings.snakePrefab, pos, Quaternion.identity);
            else factory.Create(settings.frogPrefab, pos, Quaternion.identity);
        }
    }
}
