public abstract class State
{
    public virtual void Update() { }
    
    public virtual void Enter() { }

    public virtual void Exit() { }

    public void CallMethod(string name)
    {
        var method = GetType().GetMethod(name);
        if (method != null) method.Invoke(this, new object[0]);
    }
}

public abstract class State<T> : State where T : State, new()
{
    public static T GetInstance()
    {
        return Singleton<T>.GetInstance();
    }
}