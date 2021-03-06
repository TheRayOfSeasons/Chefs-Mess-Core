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

    [Header("Hub")]
    public HubGUI hubGUI;

    [Header("Quests")]
    // for quest prompts
    public QuestModal questModal;
    // for quest prompts that were already completed
    public QuestDoneModal questDoneModal;
    // for locked quests
    public QuestLockedModal questLockedModal;
    // for after completing a quest
    public TaskCompleteModal taskCompleteModal;
    // for after failing a quest
    public TaskFailedModal taskFailedModal;

    [Header("Game")]
    public GameOverModal gameOverModal;
    public GameOverModal gameWinModal;

    [Header("Bed")]
    public BedModal bedModal;
    public CantSleepModal cantSleepModal;

    [Header("Countdown")]
    public CountdownSignal countdownSignal;

    void Awake()
    {
        instance = this;
    }
}
