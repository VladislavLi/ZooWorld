using UnityEngine;
using TMPro;
using VContainer;
using System;
using R3;

public class HudView : MonoBehaviour
{
    [Inject] private HudViewModel vm;
    [SerializeField] private TextMeshProUGUI preyText;
    [SerializeField] private TextMeshProUGUI predText;

    private IDisposable d1, d2;

    private void Start()
    {
        d1 = vm.DeadPrey.Subscribe(v => preyText.text = $"Prey: {v}");
        d2 = vm.DeadPredators.Subscribe(v => predText.text = $"Predators: {v}");
    }

    private void OnDestroy()
    {
        d1?.Dispose();
        d2?.Dispose();
    }
}
