﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    private float currentTimeScale = 1f;

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

    public void Pause()
    {
        this.currentTimeScale = Time.timeScale;
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        Time.timeScale = this.currentTimeScale;
    }

    private void SetupStressController()
    {
        this.stress = new StressController(
            onStressAdd: (currentStress, maxStress) => {
                UIManager.Instance.hubGUI.UpdateStress(currentStress, maxStress);
            },
            onStressRelief:( currentStress, maxStress) => {
                UIManager.Instance.hubGUI.UpdateStress(currentStress, maxStress);
            },
            onStressMax: () => {
                this.questDefinitions.FailMainObjective("overall", "finish-all-quests");
            }
        );
        UIManager.Instance.hubGUI.UpdateStress(this.stress.currentStress, this.stress.Meta.maxStress);
    }

    public void HandleOverallWin()
    {
        UIManager.Instance.gameWinModal.Toggle(true);
    }

    public void HandleOverallLose()
    {
        UIManager.Instance.gameOverModal.Toggle(true);
    }

    public void SetDifficulty(Constants.Difficulty difficulty)
    {
        this.difficulty = difficulty;
    }

    public int GetCurrentDay()
    {
        return this.currentDay;
    }

    // Deprecated
    public bool AllowedToSleep()
    {
        return QuestGroups.PRIMARY.isCompleted;
    }

    public void SleepToNextDay()
    {
        // TODO: add delay if we want to add animation in between sleeps
        this.ResetQuests();
        this.PushToNextDay();
    }

    public void ResetQuests()
    {
        QuestGroups.PRIMARY.Reset();
        this.questDefinitions.GetQuest("jumper").Lock();
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
        this.difficulty = newDifficulty;
        UIManager.Instance.hubGUI.UpdateDay(this.currentDay);
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

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
