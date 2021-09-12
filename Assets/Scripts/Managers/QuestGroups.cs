using System;
using QuestManagement;

public class QuestGroups
{
    public static QuestGroup MAIN = new QuestGroup("main", () => {
        if(!(GameManager.Instance.GetCurrentDay() >= 3))
            return;
        GameManager.Instance.questDefinitions.ClearMainObjective("overall", "finish-all-quests");
    });
    public static QuestGroup PRIMARY = new QuestGroup("primary", () => {
        Quest quest;
        try
        {
            quest = GameManager.Instance.questDefinitions.GetQuest("puzzle");
        }
        catch(NullReferenceException)
        {
            // fail silently
            return;
        }
        quest.Unlock();
    });
}
