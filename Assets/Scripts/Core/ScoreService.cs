public class ScoreService
{
    public int DeadPrey { get; private set; }
    public int DeadPredators { get; private set; }

    public ScoreService(SignalBus bus)
    {
        bus.Subscribe<AnimalDied>(s =>
        {
            if (s.Kind == AnimalKind.Prey) DeadPrey++;
            else DeadPredators++;
        });
    }
}
