public class FoodChainService
{
    private readonly SignalBus _bus;
    public FoodChainService(SignalBus bus) { _bus = bus; }

    public void Resolve(IAnimal a, IAnimal b)
    {
        // Prey vs Prey: nothing special
        if (a.Kind == AnimalKind.Prey && b.Kind == AnimalKind.Prey) return;

        // Predator vs Prey
        if (a.Kind == AnimalKind.Predator && b.Kind == AnimalKind.Prey)
        {
            b.Die();
            _bus.Fire(new PredatorAte(a.Transform));
            return;
        }
        if (a.Kind == AnimalKind.Prey && b.Kind == AnimalKind.Predator)
        {
            a.Die();
            _bus.Fire(new PredatorAte(b.Transform));
            return;
        }

        // Predator vs Predator: deterministic winner by InstanceId
        if (a.Kind == AnimalKind.Predator && b.Kind == AnimalKind.Predator)
        {
            var winner = a.InstanceId > b.InstanceId ? a : b;
            var loser  = a.InstanceId > b.InstanceId ? b : a;
            loser.Die();
            _bus.Fire(new PredatorAte(winner.Transform));
        }
    }
}
