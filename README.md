# Jelly-Coroutines
Jelly Button Games implementation of specialized coroutines for Unity3d - Written by Ron Rejwan

If you use this code please make sure to include us in the credits :)

http://www.jellybtn.com

## Features
* Return values from coroutines
* Catch exceptions within coroutine object safetly
* Extend the "WaitFor" pattern (ie. wait for amount of frames, wait for animation to end, etc)
* Start/pause/stop coroutines
* Callbacks for every state change within coroutine

## How to use
Full test scene with source code included (`TestScene.unity`).
* Open project in Unity (Built with Unity3d v4.6.1p4)
* Create a new MonoBehaviour
* Change component inheritence to `JellyBehaviour`
* Call `StartJellyCoroutine` or `StartJellyCoroutine<T>` (for return types) to start an `IEnumerator` coroutine
* Wait for result (either by yielding on coroutine or adding a callback)
* Use result

### Special notes
* Make sure to use the `JBYieldStatement` implementations and not Unity's `YieldStatement` ones (for example, use `WaitForTime` instead of `WaitForSeconds`)
* **_Never mix between JellyCoroutines and Unity's coroutines_**
* Tested on Windows, Android & iOS
* In general Unity's coroutine mecanism doesn't mix at all with Jelly's coroutine mecanism - yielding on JellyCoroutines from a standard coroutine won't work (and vice-versa)

###Benchmarks
Running 10,000 coroutines which in turn run a new `WaitForTime` coroutine every 0.1 seconds - compared to the exact same test using Unity's coroutines.

We found the following:
* JellyCoroutines uses about _10% less_ RAM than Unity's coroutines
* While creating new coroutines, JellyCoroutines take about _50% less_ CPU
* However, Unity's `WaitForSeconds` uses almost no CPU while yielding, while our `WaitForTime` uses a minor amount of CPU (about 10ms with 10,000 concurrent coroutines)
* Jelly coroutines give out much more flexibility and ease of use than Unity's coroutines

**Note: This benchmark was profiled on a Windows machine - benchmark results might vary on different platforms**
## Sample class
```
public class TestScene : JellyBehaviour
{
    public void TestReturnValue()
    {
        var jellyRoutine = StartJellyCoroutine<int>(CalculateAnswerToLife(3));
        jellyRoutine.GotValue += i => Debug.Log("Received value: " + i);
    }

    private IEnumerator CalculateAnswerToLife(int secondsToWait)
    {
        for (int i = 0; i < secondsToWait; i++)
        {
            // Notice we have to wait on our own YieldStatements, Unity's won't work here
            yield return new WaitForTime(1);
            Debug.Log(string.Format("{0}: Waited 1 second", Time.time));
        }

        yield return 42;
    }
}
```

### Licensing
This project is under the [*GPL V3* license](LICENSE.md).