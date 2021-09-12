using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestManagement;
using StressManagement;

public delegate void InteractabilityReceptor(GameObject interactableObject);

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance
    {
        get { return instance; }
    }

    public List<InteractabilityReceptor> interactionSubscriptions = new List<InteractabilityReceptor>();
    public StressController stress;
    public GameObject hub;
    private int interactionsDone = 0;
    private bool isHubMode = true;
    public bool IsHubMode { get { return this.isHubMode; } }
    private Constants.Difficulty difficulty = Constants.Difficulty.EASY;
    private Dictionary<int, Constants.Difficulty> difficultyMapByDay = new Dictionary<int, Constants.Difficulty> {
        {1, Constants.Difficulty.EASY},
        {2, Constants.Difficulty.MEDIUM},
        {3, Constants.Difficulty.HARD}
    };
    private int currentDay = 1;

    void Awake()
    {
        instance = this;
        this.SetupQuests();
        this.SetupStressController();
    }

    private void SetupQuests()
    {
        this.questDefinitions = new QuestDefinitions();
        this.AddActiveQuest("overall", new Quest(
            name: "Finish the game.",
            description: "Finish all quests",
            mainObjectives: new Dictionary<string, MainObjective>() {
                {
                    "finish-all-quests",
                    new MainObjective(
                        name: "Finish all quests.",
                        description: "",
                        onComplete: () => {},
                        onFail: () => {
                            GameManager.Instance.HandleOverallLose();
                        }
                    )
                }
            },
            optionalObjectives: new Dictionary<string, OptionalObjective>(),
            onComplete: () => {
                GameManager.Instance.HandleOverallWin();
            }
        ));
    }

    private void SetupStressController()
    {
        this.stress = new StressController(
            onStressAdd: _stress => {
                UIManager.Instance.hubGUI.UpdateStress(_stress);
            },
            onStressRelief: _stress => {
                UIManager.Instance.hubGUI.UpdateStress(_stress);
            },
            onStressMax: () => {
                this.questDefinitions.FailMainObjective("overall", "finish-all-quests");
            }
        );
        UIManager.Instance.hubGUI.SetupStressSlider(this.stress.Meta.maxStress);
    }

    public void HandleOverallWin()
    {
        Debug.Log("Game has been won!");
    }

    public void HandleOverallLose()
    {
        Debug.Log("Game has been lost");
    }

    public void SetDifficulty(Constants.Difficulty difficulty)
    {
        this.difficulty = difficulty;
    }

    public int GetCurrentDay()
    {
        return this.currentDay;
    }

    public int PushToNextDay()
    {
        this.currentDay++;
        bool hasDifficulty = this.difficultyMapByDay.ContainsKey(this.currentDay);
        Constants.Difficulty defaultDifficulty = this.currentDay > 3
            ? Constants.Difficulty.HARD
            : Constants.Difficulty.EASY;
        Constants.Difficulty newDifficulty = hasDifficulty
            ? this.difficultyMapByDay[this.currentDay]
            : defaultDifficulty;
        return this.currentDay;
    }

    public Constants.Difficulty GetCurrentDifficulty()
    {
        return this.difficulty;
    }

    public bool ToggleHubMode()
    {
        this.isHubMode = !this.isHubMode;
        this.hub.SetActive(this.isHubMode);
        UIManager.Instance.hubGUI.gameObject.SetActive(this.isHubMode);
        return this.isHubMode;
    }

    public bool ToggleHubMode(bool toggle)
    {
        this.isHubMode = toggle;
        this.hub.SetActive(this.isHubMode);
        UIManager.Instance.hubGUI.gameObject.SetActive(this.isHubMode);
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
