using System;
using System.Reflection;
using UnityEngine;
using RSG;

public abstract class TimingState : State
{
    protected readonly PromiseTimer promiseTimer = new PromiseTimer();

    public override void Update()
    {
        promiseTimer.Update(Time.deltaTime);
    }
}

public abstract class TimingState<T> : TimingState where T : TimingState, new()
{
    public static T GetInstance()
    {
        return Singleton<T>.GetInstance();
    }
}
