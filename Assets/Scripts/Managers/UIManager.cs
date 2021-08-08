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
    public QuestModal questModal;
    public TaskCompleteModal taskCompleteModal;
    public TaskFailedModal taskFailedModal;

    void Awake()
    {
        instance = this;
    }
}
