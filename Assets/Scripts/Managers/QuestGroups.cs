using System;
using QuestManagement;

public class QuestGroups
{
    public static QuestGroup PRIMARY = new QuestGroup("primary", () => {
        if(GameManager.Instance.GetCurrentDay() >= 3) {
            // GameManager.Instance.questDefinitions.ClearMainObjective("overall", "finish-all-quests");
            GameManager.Instance.HandleOverallWin();
            return;
        }
        Quest quest;
        try
        {
            quest = GameManager.Instance.questDefinitions.GetQuest("jumper");
        }
        catch(NullReferenceException)
        {
            // fail silently
            return;
        }
        quest.Unlock();
    });
}
