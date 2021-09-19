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

        public virtual void Reset()
        {
            this.isCompleted = false;
            this.isCompletionEventTriggered = false;
        }
    }

    public class TaskEntity : CompletionTracker
    {
        public string name { get; protected set; }
        public string description { get; protected set; }

        public TaskEntity(string name)
        {
            this.isCompleted = false;
            this.name = name;
            this.description = "";
        }

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


    public class QuestGroup : TaskEntity
    {
        public List<Quest> quests = new List<Quest>();

        public QuestGroup(
                string name,
                CompletionEvent onComplete
            ) : base(name)
        {
            this.onComplete = onComplete;
        }

        protected override bool CheckCompletion()
        {
            foreach(Quest quest in this.quests)
            {
                if(!quest.isCompleted)
                    return false;
            }
            return true;
        }

        public void AddQuest(Quest quest)
        {
            this.quests.Add(quest);
        }

        public override void Reset()
        {
            base.Reset();
            Debug.Log($"Resetting quest group {this.name}");
            foreach(Quest quest in this.quests)
                quest.Reset();
        }
    }

    public class Quest : TaskEntity
    {
        public bool isAccessible { get; protected set; }
        public Dictionary<string, MainObjective> mainObjectives { get; protected set; }
        public Dictionary<string, OptionalObjective> optionalObjectives { get; protected set; }
        public List<QuestGroup> questGroups;

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

        public Quest(
                string name,
                string description,
                Dictionary<string, MainObjective> mainObjectives,
                Dictionary<string, OptionalObjective> optionalObjectives,
                CompletionEvent onComplete,
                List<QuestGroup> questGroups
            ) : base(name, description)
        {
            this.isAccessible = true;
            this.mainObjectives = mainObjectives;
            this.optionalObjectives = optionalObjectives;
            this.questGroups = questGroups;
            this.onComplete = () => {
                onComplete();
                foreach(QuestGroup group in this.questGroups)
                    group.Update();
            };
            foreach(QuestGroup group in this.questGroups)
                group.AddQuest(this);
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

        public override void Reset()
        {
            base.Reset();
            foreach(KeyValuePair<string, MainObjective> mainObjective in this.mainObjectives)
                mainObjective.Value.Reset();
            foreach(KeyValuePair<string, OptionalObjective> optionalObjective in this.optionalObjectives)
                optionalObjective.Value.Reset();
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
