using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Util {

    /// <summary>
    /// Simple timer class to provide a timer for counting seconds.
    /// This is a cheap timer, only holds start timer as data.
    /// </summary>
    class SimpleTimer {

        /// <summary>
        /// Stores the time the timer started
        /// </summary>
        private float startTime = 0L;

        /// <summary>
        /// Starts the timer.
        /// After a start, GetTime will return how much time has passed, in Seconds
        /// since the call to start.
        /// </summary>
        public void Start() {
            startTime = Time.realtimeSinceStartup;
        }

        /// <summary>
        /// Starts the timer.
        /// After a start, GetTime will return how much time has passed, in Seconds
        /// since the time passed.
        /// </summary>
        /// <param name="since"> The time to start from. </param>
        public void Start(float since) {
            startTime = since;
        }

        /// <summary>
        /// Returns how much time has passed since start of the timer.
        /// </summary>
        /// <returns> A float that is how much time passed since timer started. </returns>
        public float GetTime() {
            return Time.realtimeSinceStartup - startTime;
        }

        /// <summary>
        /// Resets the timer to 0.
        /// This is the same as measuing delta time directly.
        /// A timer after a reset will return Time.deltaTime.
        /// </summary>
        public void Reset() {
            startTime = Time.realtimeSinceStartup;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class EventTimer {
        #region Event Declaration
        public delegate void TimerEventHandler();
        public event TimerEventHandler TimerReady;
        #endregion

        //How much time needs to pass to run the event
        public float timeout { get; set; }

        //If the timer loops when it reaches the end, or just finishes
        private bool _looping { get; set; }

        //The simple timer to keep track of time
        private SimpleTimer innerTimer = new SimpleTimer();

        //The behavior that runs this timer
        private MonoBehaviour owner;

        //Coroutine
        Coroutine updater;

        //Store if the timer is running
        private bool started;
        public bool Started { get { return started; } }

        //Main constructor
        public EventTimer(MonoBehaviour _owner) {
            owner = _owner;
            _looping = false;
        }

        //Ctor 2
        public EventTimer(MonoBehaviour _owner, float timeout) {
            owner = _owner;
            _looping = false;
            this.timeout = timeout;
        }

        //Ctor 3
        public EventTimer(MonoBehaviour _owner, float timeout, bool looping) {
            owner = _owner;
            _looping = looping;
            this.timeout = timeout;

            //Debug
            //Debug.Log("Timeout: " + timeout);
        }

        /// <summary>
        /// Sets the timer looping behaviour
        /// </summary>
        /// <param name="loop"></param>
        public void SetLooping(bool loop) {
            _looping = loop;
        }

        /// <summary>
        /// Starts the timer.
        /// </summary>
        public void Start() {
            innerTimer.Start();

            //Debug
            //Debug.Log("Timer started.");

            if(updater != null && owner != null)
                owner.StopCoroutine(updater);
            if(owner != null) updater = owner.StartCoroutine(Updater());

            started = true;
        }

        /// <summary>
        /// Resets the time to 0.
        /// Won't stop the timer.
        /// </summary>
        public void Reset() {
            //Set the inner timer to 0
            innerTimer.Reset();
        }

        /// <summary>
        /// Gets the elapsed time from the start of this timer.
        /// </summary>
        /// <returns></returns>
        public float GetElapsedTime() {
            return innerTimer.GetTime();
        }

        /// <summary>
        /// Returns how much time is left until the next event call.
        /// </summary>
        /// <returns></returns>
        public float GetRemainingTime() {
            return timeout - innerTimer.GetTime();
        }

        //This is like a co-update method for the owning behavior
        IEnumerator Updater() {
            while (true) {
                //Debug
                //Debug.Log("Update: "+ GetElapsedTime());

                //If the time passed
                if (GetRemainingTime() <= 0f) {
                    //Notify listeners
                    OnTimerReady();
                    break;
                }

                //Wait next frame
                yield return null;
            }

            //Reset for another run
            if(_looping) {
                Reset();
                Start();
            }
        }

        /// <summary>
        /// Stops the timer execution.
        /// Time is set to 0.
        /// </summary>
        public void Stop() {
            //Reset the inner timer
            innerTimer.Reset();

            if(updater != null)
                owner.StopCoroutine(updater);

            started = false;
        }

        //Notifies the timer ready event to all subscribers
        protected virtual void OnTimerReady() {
            //If we have subscribers (event subscribers)
            if (TimerReady != null) {
                //Call them
                TimerReady();
            }
        }
    }
}
