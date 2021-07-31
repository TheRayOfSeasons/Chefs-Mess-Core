using System;

public class QuestProvider : Interactable
{
    public string questKey;

    public virtual void RunQuestIntro() {}

    public override void OnInteract()
    {
        base.OnInteract();
        if(this.questKey == null)
        {
            throw new ArgumentException("questKey must be defined.");
        }
        this.RunQuestIntro();
    }

    public override void OnExitInteraction()
    {
        base.OnExitInteraction();
    }
}
