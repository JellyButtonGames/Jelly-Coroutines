using System.Collections;
using JellyTools.Coroutines;
using JellyTools.Coroutines.Yields;
using UnityEngine;

public abstract class JellyBehaviour : MonoBehaviour
{
    private readonly JBCoroutineYielder _yielder = new JBCoroutineYielder();

    void Update()
    {
        _yielder.ProcessCoroutines();
        JellyUpdate();
    }

    public JellyCoroutine StartJellyCoroutine(IEnumerator coroutine)
    {
        return _yielder.StartCoroutine(coroutine);
    }

    public JellyCoroutine<T> StartJellyCoroutine<T>(IEnumerator coroutine)
    {
        return _yielder.StartCoroutine<T>(coroutine);
    }

    protected virtual void JellyUpdate(){}
}