using System;
using System.Collections;
using JellyTools.Coroutines.Interfaces;

namespace JellyTools.Coroutines.Yields
{
    /// <summary>
    /// Jelly YieldInstruction is used for all specialized coroutines
    /// </summary>
    public abstract class JBYieldInstruction : IJellyYieldInstruction
    {
        #region State
        /// <summary>
        /// The state of the yield instruction
        /// </summary>
        public enum YieldStateType
        {
            NotRunning,
            Running,
            Paused,
            Finished,
            Stopped,
            CaughtException
        }

        private YieldStateType _state;

        /// <summary>
        /// Current state of this yield instruction
        /// </summary>
        public YieldStateType State
        {
            get { return _state; }
            private set
            {
                if (_state == value)
                {
                    return;
                }
                _state = value;

                // If we have any child coroutines currently yielding, we set their state as well
                var yieldInstruction = Coroutine.Current as JBYieldInstruction;
                if (yieldInstruction != null)
                {
                    yieldInstruction.State = State;
                }

                // Call state changed callback
                if (StateChanged != null)
                {
                    StateChanged(this, _state);
                }
            }
        }

        /// <summary>
        /// Called every time the YieldStatement's state has changed
        /// </summary>
        public event Action<JBYieldInstruction, YieldStateType> StateChanged;

        /// <summary>
        /// Called once the YieldStatement completed succesfully
        /// </summary>
        public event Action Completed;

        /// <summary>
        /// Called once the YieldStatement caught an exception
        /// </summary>
        public event Action CaughtException;
        #endregion

        /// <summary>
        /// Set this coroutine to be the one you want to yield on
        /// </summary>
        protected IEnumerator Coroutine;

        protected JBYieldInstruction()
        {
            State = YieldStateType.NotRunning;
        }

        /// <summary>
        /// This basically has to return the coroutine you want to use in your YieldStatement implementation
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerator GetCoroutineFunction();

        /// <summary>
        /// If we caught an exception within the coroutine it would be stored here
        /// </summary>
        public Exception Exception { get; private set; }

        public virtual bool MoveNext()
        {
            // Sanity checks removed for performance tuning

            // Get coroutine and MoveNext on everything
            var yieldInstruction = Coroutine.Current as JBYieldInstruction;
            try
            {
                if (yieldInstruction != null)
                {
                    if (yieldInstruction.MoveNext())
                    {
                        return true;
                    }
                }
                if (Coroutine.MoveNext())
                {
                    return true;
                }
            }
            catch (Exception exception)
            {
                // Caught an exception
                Exception = exception;
                State = YieldStateType.CaughtException;
                if (CaughtException != null)
                {
                    CaughtException();
                }
                return false;
            }

            // Finished running
            State = YieldStateType.Finished;
            if (Completed != null)
            {
                Completed();
            }
            return false;
        }

        #region Yield State Modifiers
        /// <summary>
        /// Starts a coroutine, can only be called from NotRunning state
        /// </summary>
        public void Start()
        {
            if (State != YieldStateType.NotRunning)
            {
                return;
            }
            Coroutine = GetCoroutineFunction();
            State = YieldStateType.Running;
        }

        /// <summary>
        /// Stops the coroutine completely (Can't be resumed)
        /// </summary>
        public void Stop()
        {
            if (State != YieldStateType.Paused && State != YieldStateType.Running)
            {
                return;
            }
            State = YieldStateType.Stopped;
        }

        /// <summary>
        /// Resumes a paused coroutine
        /// </summary>
        public void Resume()
        {
            if (State != YieldStateType.Paused)
            {
                return;
            }
            State = YieldStateType.Running;
        }

        /// <summary>
        /// Pauses a running coroutine
        /// </summary>
        public void Pause()
        {
            if (State != YieldStateType.Running)
            {
                return;
            }
            State = YieldStateType.Paused;
        }
        #endregion
    }

    /// <summary>
    /// Generic YieldInstruction used to return a value
    /// </summary>
    /// <typeparam name="T">The type of the value you want to return</typeparam>
    public abstract class JBYieldInstruction<T> : JBYieldInstruction
    {
        private readonly Type _returnType;

        private T _value;

        protected JBYieldInstruction()
        {
            _returnType = typeof(T);
        }

        /// <summary>
        /// The returned value, if we caught an exception while running it will be thrown here
        /// </summary>
        public T Value
        {
            get
            {
                if (Exception != null)
                {
                    throw Exception;
                }
                return _value;
            }
        }

        /// <summary>
        /// Called when we got a value from the YieldStatement
        /// </summary>
        public event Action<T> GotValue;

        public override bool MoveNext()
        {
            // First let the coroutine run
            bool result = base.MoveNext();
            if (result)
            {
                return true;
            }

            // We finished? Check if we got a return value of the type we're looking for
            if (Coroutine.Current != null && Coroutine.Current.GetType() == _returnType)
            {
                _value = (T)Coroutine.Current;
                if (GotValue != null)
                {
                    GotValue(_value);
                }
            }

            return false;
        }
    }
}