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