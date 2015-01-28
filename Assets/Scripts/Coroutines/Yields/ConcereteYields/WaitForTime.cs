using System;
using System.Collections;
using UnityEngine;

namespace JellyTools.Coroutines.Yields.ConcereteYields
{
    /// <summary>
    /// Yields for a specified amount of time
    /// </summary>
    public class WaitForTime : JBYieldInstruction
    {
        private readonly float _finishedTime;

        public WaitForTime(float seconds)
        {
            if (seconds < 0)
            {
                throw new Exception("WaitForSeconds must receive a positive number.");
            }
            _finishedTime = Time.time + seconds;

            Coroutine = Count();
        }

        private IEnumerator Count()
        {
            while (Time.time < _finishedTime)
            {
                yield return true;
            }
        }

        protected override IEnumerator GetCoroutineFunction()
        {
            return Count();
        }
    }
}