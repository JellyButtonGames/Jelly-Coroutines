using System;
using System.Collections;
using JellyTools.Coroutines;
using JellyTools.Coroutines.Yields;
using JellyTools.Coroutines.Yields.ConcereteYields;
using UnityEngine;

public class TestScene : JellyBehaviour
{
    [SerializeField] private Animation _testAnimation;

    #region Return value tests
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

    public void TestNestedReturnValue()
    {
        var jellyRoutine = StartJellyCoroutine<int>(CalculateAnswerToLifeNested(3));
        jellyRoutine.GotValue += i => Debug.Log("Received value: " + i);
    }

    private IEnumerator CalculateAnswerToLifeNested(int secondsToWait)
    {
        Debug.Log("Parent: Coroutine started, waiting 1 second");
        yield return new WaitForTime(1);
        Debug.Log("Parent: Starting nested coroutine and waiting for it to finish");
        var innerCoroutine = StartJellyCoroutine<int>(SomeExpensiveCalculation(21));
        yield return innerCoroutine;
        Debug.Log("Parent: Nested coroutine finished");

        if (innerCoroutine.Exception != null)
        {
            Debug.Log(innerCoroutine.Exception);
            yield break;
        }

        yield return innerCoroutine.Value;
    }

    private IEnumerator SomeExpensiveCalculation(int value)
    {
        Debug.Log("Nested: Started, waiting 2 seconds");
        yield return new WaitForTime(2);
        Debug.Log("Nested: Finished waiting, returning value");
        yield return value * 2;
    }
    #endregion

    #region Unity vs Jelly coroutines
    public void TestJellyVsUnityCoroutine()
    {
        // These tests should run on the same frames (you should see both debug messages on the same frame)
        StartCoroutine(TestUnityCoroutine());
        StartJellyCoroutine(TestJellyCoroutine()).Completed += () => Debug.Log("Jelly coroutine completed");
    }

    private IEnumerator TestUnityCoroutine()
    {
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(0.2f);
            Debug.Log(string.Format("Unity coroutine - Frame {0}", Time.frameCount));
        }
    }

    private IEnumerator TestJellyCoroutine()
    {
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForTime(0.2f);
            Debug.Log(string.Format("Jelly coroutine - Frame {0}", Time.frameCount));
        }
    }
    #endregion

    #region Exceptions in JellyCoroutinues
    public void TestExceptionCoroutine()
    {
        var coroutine = StartJellyCoroutine<string>(ExceptionRoutine());
        coroutine.CaughtException += delegate
        {
            Debug.Log(string.Format("Caught exception in callback - Getting the value now should throw an exception in this function's scope.\n\rException: {0}",
                coroutine.Exception));
            Debug.Log(coroutine.Value);
        };
        coroutine.Completed += () => Debug.Log("This shouldn't complete");
    }

    private IEnumerator ExceptionRoutine()
    {
        Debug.Log("Started exception coroutine");
        yield return new WaitForTime(1);
        Debug.Log("Waited 1 second, throwing exception - rest of code shouldn't continue");
        throw new Exception("Some exception occured");
        yield return new WaitForTime(1);
        yield return "This shouldn't be called";
    }
    #endregion

    #region Infinite coroutine test (pause/resume/stop)
    private JellyCoroutine _infiniteCoroutine;

    public void PauseResumeInfiniteCoroutine()
    {
        if (_infiniteCoroutine != null)
        {
            switch (_infiniteCoroutine.State)
            {
                case JBYieldInstruction.YieldStateType.Running:
                    _infiniteCoroutine.Pause();
                    break;
                case JBYieldInstruction.YieldStateType.Paused:
                    _infiniteCoroutine.Resume();
                    break;
                case JBYieldInstruction.YieldStateType.Stopped:
                case JBYieldInstruction.YieldStateType.Finished:
                case JBYieldInstruction.YieldStateType.CaughtException:
                    throw new Exception("This should never happen on this coroutine test");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        else
        {
            Debug.Log("Started new infinite coroutine");
            _timeSinceLastPulse = Time.time;
            _infiniteCoroutine = StartJellyCoroutine(InfiniteCoroutine());
            _infiniteCoroutine.StateChanged +=
                (instruction, type) => Debug.Log(string.Format("Infinite coroutine new state: {0}", type));
        }
    }

    public void StopInfiniteCoroutine()
    {
        if (_infiniteCoroutine == null)
        {
            return;
        }
        _infiniteCoroutine.Stop();
        _infiniteCoroutine = null;
    }

    private float _timeSinceLastPulse;
    private IEnumerator InfiniteCoroutine()
    {
        while (true)
        {
            Debug.Log(string.Format("Infinite coroutine pulse, time from last pulse: {0}", (Time.time - _timeSinceLastPulse)));
            _timeSinceLastPulse = Time.time;
            yield return new WaitForTime(1);
        }
    }
    #endregion

    #region Custom WaitFor tests
    public void TestWaitForAnimationEnd()
    {
        StartJellyCoroutine(AnimationEndCoroutineTest()).Completed += () => Debug.Log("Animation complete");
    }

    private IEnumerator AnimationEndCoroutineTest()
    {
        Debug.Log("Starting animation, waiting for it to end");
        _testAnimation.Play();
        yield return new WaitForAnimationStopPlaying(_testAnimation);
    }
    #endregion

    protected override void JellyUpdate()
    {
        // Updating goes here
    }
}