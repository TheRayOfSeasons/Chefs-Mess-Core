using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestManagement;

public delegate void InteractabilityReceptor(GameObject interactableObject);

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance
    {
        get { return instance; }
    }

    public List<InteractabilityReceptor> interactionSubscriptions = new List<InteractabilityReceptor>();
    public GameObject hub;
    private int interactionsDone = 0;
    private bool isHubMode = true;
    public bool IsHubMode { get { return this.isHubMode; } }

    void Awake()
    {
        instance = this;
        this.questDefinitions = new QuestDefinitions();
    }

    public bool ToggleHubMode()
    {
        this.isHubMode = !this.isHubMode;
        this.hub.SetActive(this.isHubMode);
        return this.isHubMode;
    }

    public bool ToggleHubMode(bool toggle)
    {
        this.isHubMode = toggle;
        this.hub.SetActive(this.isHubMode);
        return this.isHubMode;
    }

    public QuestDefinitions questDefinitions { get; protected set; }

    public void AddActiveQuest(string questKey, Quest quest)
    {
        this.questDefinitions.activeQuests.Add(questKey, quest);
    }

    public void SubscribeToGlobalInteractions(InteractabilityReceptor receptor)
    {
        this.interactionSubscriptions.Add(receptor);
    }

    public void UnsubscribeToGlobalInteractions(InteractabilityReceptor receptor)
    {
        this.interactionSubscriptions.Remove(receptor);
    }

    public void CallInteractionSubscriptions(GameObject interactableObject)
    {
        this.interactionsDone++;
        foreach(InteractabilityReceptor receptor in this.interactionSubscriptions)
        {
            receptor(interactableObject);
        }
    }

}
