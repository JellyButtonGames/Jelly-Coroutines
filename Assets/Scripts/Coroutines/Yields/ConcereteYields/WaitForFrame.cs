namespace JellyTools.Coroutines.Yields.ConcereteYields
{
    /// <summary>
    /// Yields for exactly 1 frame
    /// </summary>
    public class WaitForFrame : WaitForFrameCount
    {
        public WaitForFrame() : base(1)
        {
        }
    }
}