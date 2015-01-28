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