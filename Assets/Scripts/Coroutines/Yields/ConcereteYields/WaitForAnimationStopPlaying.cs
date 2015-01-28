using System;
using System.Collections;
using UnityEngine;

namespace JellyTools.Coroutines.Yields.ConcereteYields
{
    /// <summary>
    /// Yields until given animation has finished playing, sampling <see cref="Animation.isPlaying"/>
    /// </summary>
    public class WaitForAnimationStopPlaying : JBYieldInstruction
    {
        private readonly Animation _animation;

        public WaitForAnimationStopPlaying(Animation animation)
        {
            _animation = animation;

            Coroutine = CheckAnimationPlaying();
        }

        private IEnumerator CheckAnimationPlaying()
        {
            while (_animation.isPlaying)
            {
                yield return true;
            }
        }

        protected override IEnumerator GetCoroutineFunction()
        {
            return CheckAnimationPlaying();
        }
    }
}