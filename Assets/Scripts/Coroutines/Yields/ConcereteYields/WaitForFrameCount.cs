using System.Collections;

namespace JellyTools.Coroutines.Yields.ConcereteYields
{
    /// <summary>
    /// Yields for a specified amount of frames
    /// </summary>
    public class WaitForFrameCount : JBYieldInstruction
    {
        private int _count;

        public WaitForFrameCount(int count)
        {
            _count = count;

            Coroutine = Count();
        }

        private IEnumerator Count()
        {
            while (--_count >= 0)
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