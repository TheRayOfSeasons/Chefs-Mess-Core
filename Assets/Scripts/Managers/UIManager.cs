using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance
    {
        get { return instance; }
    }

    [Header("Quests")]
    // for quest prompts
    public QuestModal questModal;
    // for quest prompts that were already completed
    public QuestDoneModal questDoneModal;
    // for after completing a quest
    public TaskCompleteModal taskCompleteModal;
    // for after failing a quest
    public TaskFailedModal taskFailedModal;

    void Awake()
    {
        instance = this;
    }
}
