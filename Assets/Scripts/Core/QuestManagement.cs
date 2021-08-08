using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestManagement
{
    public delegate void CompletionEvent();
    public delegate void FailureEvent();

    public class CompletionTracker
    {
        protected bool isCompletionEventTriggered = false;
        public bool isCompleted { get; protected set; }
        public CompletionEvent onComplete { get; protected set; }

        public virtual void RunOnComplete()
        {
            if(!this.isCompletionEventTriggered)
            {
                this.onComplete();
                this.isCompletionEventTriggered = true;
            }
        }

        public virtual void OverrideCompletion()
        {
            this.isCompleted = true;
            this.RunOnComplete();
        }

        protected virtual bool CheckCompletion()
        {
            return true;
        }

        public virtual void Update()
        {
            this.isCompleted = this.CheckCompletion();
            if(this.isCompleted)
            {
                this.RunOnComplete();
            }
        }
    }

    public class TaskEntity : CompletionTracker
    {
        public string name { get; protected set; }
        public string description { get; protected set; }

        public TaskEntity(string name, string description)
        {
            this.isCompleted = false;
            this.name = name;
            this.description = description;
        }
    }

    /// <summary>
    /// Defines an objective for a quest.
    /// </summary>
    public class Objective : TaskEntity
    {
        public FailureEvent onFail { get; protected set; }

        public Objective(
                string name,
                string description,
                CompletionEvent onComplete,
                FailureEvent onFail
            ) : base(name, description)
        {
            this.onComplete = onComplete;
            this.onFail = onFail;
        }

        protected override bool CheckCompletion()
        {
            return this.isCompleted;
        }
    }

    /// <summary>
    /// Defines an objective that is a prerequisite to complete a quest.
    /// </summary>
    public class MainObjective : Objective
    {
        public MainObjective(
                string name,
                string description,
                CompletionEvent onComplete,
                FailureEvent onFail
            ): base(name, description, onComplete, onFail)
        {
        }
    }

    /// <summary>
    /// Defines an optional objective that the player may do to complete a quest.
    /// </summary>
    public class OptionalObjective : Objective
    {
        public OptionalObjective(
                string name,
                string description,
                CompletionEvent onComplete,
                FailureEvent onFail
            ): base(name, description, onComplete, onFail)
        {
        }
    }

    public class Quest : TaskEntity
    {
        public bool isAccessible { get; protected set; }
        public Dictionary<string, MainObjective> mainObjectives { get; protected set; }
        public Dictionary<string, OptionalObjective> optionalObjectives { get; protected set; }

        public Quest(
                string name,
                string description,
                Dictionary<string, MainObjective> mainObjectives,
                Dictionary<string, OptionalObjective> optionalObjectives,
                CompletionEvent onComplete
            ) : base(name, description)
        {
            this.isAccessible = true;
            this.mainObjectives = mainObjectives;
            this.optionalObjectives = optionalObjectives;
            this.onComplete = onComplete;
        }

        public void Lock()
        {
            this.isAccessible = false;
        }

        public void Unlock()
        {
            this.isAccessible = true;
        }

        public void ToggleLock()
        {
            this.isAccessible = !this.isAccessible;
        }

        protected override bool CheckCompletion()
        {
            foreach(KeyValuePair<string, MainObjective> mainObjective in this.mainObjectives)
            {
                if(!mainObjective.Value.isCompleted)
                    return false;
            }
            return true;
        }
    }
}
