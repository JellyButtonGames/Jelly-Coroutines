/*************************************************************************
 * 
 *  Jelly Coroutines 
 *  By Ron Rejwan (Jan 2015) - ronr@jellybtn.com
 *  Jelly Button Games LTD. (http://www.jellybtn.com)
 *  -----------------------------------------------------------------
 *
 *  Feel free to use these coroutines in your project, please just leave
 *  these comments in the file and let me know if I helped you out.
 *  
 *  Also make sure to commit any fixes/improvement to our repository:
 *  https://github.com/rejwan/Jelly-Coroutines
 * 
 *************************************************************************/

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