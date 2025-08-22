using System;
using System.Collections.Generic;
using UnityEngine;

// Minimal typed pub/sub
public sealed class SignalBus
{
    private readonly Dictionary<Type, object> map = new();

    public IDisposable Subscribe<T>(Action<T> handler)
    {
        Type t = typeof(T);
        if (!map.TryGetValue(t, out var obj))
            map[t] = obj = new List<Action<T>>();
        List<Action<T>> list = (List<Action<T>>)obj;
        list.Add(handler);
        return new Disp(() => list.Remove(handler));
    }

    public void Fire<T>(T signal)
    {
        if (map.TryGetValue(typeof(T), out var obj))
        {
            List<Action<T>> list = (List<Action<T>>)obj;
            // Copy to avoid mutation during enumeration
            Action<T>[] snapshot = list.ToArray();
            for (int i = 0; i < snapshot.Length; i++) snapshot[i](signal);
        }
    }

    private sealed class Disp : IDisposable
    {
        private readonly Action _a;
        public Disp(Action a) => _a = a;
        public void Dispose() => _a?.Invoke();
    }
}

public readonly struct AnimalDied
{
    public readonly AnimalKind Kind;
    public AnimalDied(AnimalKind kind) { Kind = kind; }
}

public readonly struct PredatorAte
{
    public readonly Transform Predator;
    public PredatorAte(Transform predator) { Predator = predator; }
}
