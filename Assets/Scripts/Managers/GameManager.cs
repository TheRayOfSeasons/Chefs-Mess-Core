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
    public int interactionsDone = 0;

    void Awake()
    {
        instance = this;
        this.questDefinitions = new QuestDefinitions();
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
