using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance {
        get { return instance; }
    }

    void Awake()
    {
        instance = this;
    }

    public QuestDefinitions questDefinitions { get; protected set; }

    void Start()
    {
        this.questDefinitions = new QuestDefinitions();
    }
}
