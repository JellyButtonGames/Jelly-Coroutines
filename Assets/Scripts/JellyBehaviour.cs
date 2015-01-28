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
using JellyTools.Coroutines;
using JellyTools.Coroutines.Yields;
using UnityEngine;

public abstract class JellyBehaviour : MonoBehaviour
{
    private readonly JBCoroutineYielder _yielder = new JBCoroutineYielder();

    void Update()
    {
        _yielder.ProcessCoroutines();
        JellyUpdate();
    }

    public JellyCoroutine StartJellyCoroutine(IEnumerator coroutine)
    {
        return _yielder.StartCoroutine(coroutine);
    }

    public JellyCoroutine<T> StartJellyCoroutine<T>(IEnumerator coroutine)
    {
        return _yielder.StartCoroutine<T>(coroutine);
    }

    protected virtual void JellyUpdate(){}
}