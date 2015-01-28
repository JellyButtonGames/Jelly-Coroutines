using System;
using System.Collections;
using System.Collections.Generic;
using JellyTools.Coroutines.Interfaces;

namespace JellyTools.Coroutines.Yields
{
    /// <summary>
    /// This class implements the actual yielding on Jelly Coroutines
    /// </summary>
    public class JBCoroutineYielder
    {
        internal List<IJellyYieldInstruction> coroutines = new List<IJellyYieldInstruction>();

        /// <summary>
        ///     Starts a Jelly Coroutine that returns a value
        /// </summary>
        /// <param name="coroutine">
        ///     IEnumerator to run, to return a value 'yield return' it - Notice, you must use Jelly
        ///     YieldStatements, Unity's won't work here
        /// </param>
        /// <returns>A started & running coroutine</returns>
        public JellyCoroutine<T> StartCoroutine<T>(IEnumerator routine)
        {
            var coroutine = new JellyCoroutine<T>(routine);
            coroutine.StateChanged += CoroutineOnStateChanged;
            coroutine.Start();
            return coroutine;
        }

        /// <summary>
        ///     Starts a Jelly Coroutine that doesn't return a value
        /// </summary>
        /// <param name="coroutine">IEnumerator to run - Notice, you must use Jelly YieldStatements, Unity's won't work here</param>
        /// <returns>A started & running coroutine</returns>
        public JellyCoroutine StartCoroutine(IEnumerator routine)
        {
            var coroutine = new JellyCoroutine(routine);
            coroutine.StateChanged += CoroutineOnStateChanged;
            coroutine.Start();
            return coroutine;
        }

        private void CoroutineOnStateChanged(JBYieldInstruction coroutine, JBYieldInstruction.YieldStateType newState)
        {
            switch (newState)
            {
                case JBYieldInstruction.YieldStateType.Running:
                {
                    // If a coroutine returned to running state, readd it to process queue
                    coroutines.Add(coroutine);
                    break;
                }
                case JBYieldInstruction.YieldStateType.Paused:
                {
                    // If a coroutine was paused, remove from process queue
                    coroutines.Remove(coroutine);
                    break;
                }
                case JBYieldInstruction.YieldStateType.Finished:
                case JBYieldInstruction.YieldStateType.Stopped:
                case JBYieldInstruction.YieldStateType.CaughtException:
                {
                    // If a coroutine got a hard stop, remove from process queue and stop listening to state changes
                    coroutines.Remove(coroutine);
                    coroutine.StateChanged -= CoroutineOnStateChanged;
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException("newState");
            }
        }

        /// <summary>
        ///     Processes every coroutine currently running, should be called once every Update
        /// </summary>
        public void ProcessCoroutines()
        {
            for (int i = 0; i < coroutines.Count;)
            {
                if (coroutines[i].MoveNext())
                {
                    ++i;
                }
            }
        }
    }
}