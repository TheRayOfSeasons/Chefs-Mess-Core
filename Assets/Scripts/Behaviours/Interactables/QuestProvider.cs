using System;
using QuestManagement;

public class QuestProvider : Interactable
{
    public string questKey;
    public BGM questBGM;

    protected virtual void RunCompletedAlready(Quest quest)
    {
        UIManager.Instance.questDoneModal.SetTitle(quest.name);
        UIManager.Instance.questDoneModal.Toggle(true);
    }

    protected virtual void RunInaccesibleEvent(Quest quest)
    {
        UIManager.Instance.questLockedModal.SetTitle(quest.name);
        UIManager.Instance.questLockedModal.Toggle(true);
    }

    protected virtual void RunIntro(Quest quest) {}

    public void RunQuestIntro(Quest quest)
    {
        if(!quest.isAccessible)
        {
            this.RunInaccesibleEvent(quest);
            return;
        }

        if(quest.isCompleted)
        {
            this.RunCompletedAlready(quest);
        }
        else
        {
            this.RunIntro(quest);
        }
    }

    public override void OnInteract()
    {
        base.OnInteract();
        if(this.questKey == null)
        {
            throw new ArgumentException("questKey must be defined.");
        }
        AudioManager.Instance.ChangeBackgroundMusic(this.questBGM);
        Quest quest = GameManager.Instance.questDefinitions.GetQuest(questKey: this.questKey);
        this.RunQuestIntro(quest);
    }

    public override void OnExitInteraction()
    {
        base.OnExitInteraction();
    }
}
