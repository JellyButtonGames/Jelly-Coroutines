using System.Collections;
using JellyTools.Coroutines.Yields;

namespace JellyTools.Coroutines
{
    public class JellyCoroutine : JBYieldInstruction
    {
        public JellyCoroutine(IEnumerator coroutine)
        {
            Coroutine = coroutine;
        }

        protected override IEnumerator GetCoroutineFunction()
        {
            return Coroutine;
        }
    }

    public class JellyCoroutine<T> : JBYieldInstruction<T>
    {
        public JellyCoroutine(IEnumerator coroutine)
        {
            Coroutine = coroutine;
        }

        protected override IEnumerator GetCoroutineFunction()
        {
            return Coroutine;
        }
    }
}