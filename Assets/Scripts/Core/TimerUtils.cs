using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimerUtils
{
    /// <summary>
    /// A delegate method for timed actions.
    /// </summary>
    public delegate void TimerAction();

    /// <summary>
    /// A class that manages timed events.
    /// </summary>
    public class TimedAction
    {
        /// <summary>
        /// The maximum amount of time before the configured event triggers.
        /// </summary>
        public float maxTime;

        /// <summary>
        /// The elapsed time left before the event triggers.
        /// </summary>
        public float currentTime { get; private set; }

        /// <summary>
        /// The event that will trigger once time runs out.
        /// </summary>
        public TimerAction action;

        /// <summary>
        /// A private boolean field for handling the initial trigger.
        /// </summary>
        private bool triggerOnInitial;

        public TimedAction(float maxTime, TimerAction action)
        {
            this.maxTime = maxTime;
            this.currentTime = maxTime;
            this.action = action;
            this.triggerOnInitial = false;
        }

        public TimedAction(float maxTime, TimerAction action, bool triggerOnInitial = true)
        {
            this.maxTime = maxTime;
            this.currentTime = maxTime;
            this.action = action;
            this.triggerOnInitial = triggerOnInitial;
        }

        /// <summary>
        /// Runs the timed action.
        /// </summary>
        /// <param name="time">
        /// The timer's basis for the countdown.
        /// </param>
        public void Run(float time)
        {
            currentTime -= time;
            if(this.currentTime > 0 && !this.triggerOnInitial)
                return;
            this.action();
            if(this.triggerOnInitial)
                this.triggerOnInitial = false;
            this.currentTime = maxTime;
        }

        public void RunOnce(float time)
        {
            this.currentTime -= time;
            if(this.currentTime > 0 && !triggerOnInitial)
                return;
            this.action();
        }

        public void Reset()
        {
            this.currentTime = maxTime;
        }
    }
}
