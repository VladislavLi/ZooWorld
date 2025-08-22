using R3;

public class HudViewModel
{
    public readonly ReactiveProperty<int> DeadPrey = new(0);
    public readonly ReactiveProperty<int> DeadPredators = new(0);

    public HudViewModel(ScoreService score, SignalBus bus)
    {
        bus.Subscribe<AnimalDied>(_ =>
        {
            DeadPrey.Value = score.DeadPrey;
            DeadPredators.Value = score.DeadPredators;
        });
    }
}
