using System.Runtime.InteropServices;
using UnityEngine.TextCore.Text;
public abstract class BaseState<T> : IState {

    protected T subject;

    public BaseState(T subject) {
        this.subject = subject;
    }
    public virtual void FixedUpdate() { }

    public virtual void OnEnter() { }

    public virtual void OnExit() { }

    public virtual void Update() { }
}
