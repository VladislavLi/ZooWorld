using UnityEngine;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;

public class TastyLabelService
{
    private readonly SignalBus bus;
    private readonly GameObject labelPrefab;

    public TastyLabelService(SignalBus bus)
    {
        this.bus = bus;
        labelPrefab = new GameObject("TastyLabel", typeof(TextMesh));
        var tm = labelPrefab.GetComponent<TextMesh>();
        tm.text = "Tasty!";
        tm.characterSize = 0.2f;
        tm.fontSize = 48;
        tm.alignment = TextAlignment.Center;
        tm.anchor = TextAnchor.MiddleCenter;
        tm.color = Color.yellow;

        bus.Subscribe<PredatorAte>(s => SpawnLabel(s.Predator).Forget());
    }

    private async UniTaskVoid SpawnLabel(Transform predator)
    {
        GameObject go = Object.Instantiate(labelPrefab,
            predator.position + Vector3.up * 0.5f,
            Quaternion.identity);

        TextMesh tm = go.GetComponent<TextMesh>();
        float t = 0f;
        
        Camera cam = Camera.main; // cache reference to camera
        while (t < 1.2f) // visible for ~1.2s
        {
            if (go == null) break;
            t += Time.deltaTime;
            // Float upward
            go.transform.position += Vector3.up * (Time.deltaTime * 0.5f);
            
            // Billboard towards camera
            if (cam != null)
                go.transform.rotation = Quaternion.LookRotation(cam.transform.forward, Vector3.up);

            //fade
            tm.color = new Color(1f, 0.9f, 0.2f, 1f - t / 1.2f);
            await UniTask.Yield();
        }

        if (go != null)
            Object.Destroy(go);
    }
}
