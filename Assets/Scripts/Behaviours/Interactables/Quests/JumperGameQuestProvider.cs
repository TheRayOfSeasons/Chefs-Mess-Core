using System.Collections.Generic;
using UnityEngine;
using QuestManagement;

public class JumperGameQuestProvider : QuestProvider
{
    [SerializeField] private GameObject gui;
    [SerializeField] private GameObject jumperGame;
    [SerializeField] private GameObject tutorial;
    [SerializeField] private GameObject cutscene;

    public override void Initialize()
    {
        base.Initialize();
        this.questKey = "jumper";
        this.SetupQuest();
    }

    private void UpdateStress()
    {
        Constants.Difficulty difficulty = GameManager.Instance.GetCurrentDifficulty();
        float stress = JumperMeta.stress[difficulty];
        GameManager.Instance.stress.Add(stress);
    }

    private void SetupQuest()
    {
        Quest quest = new Quest(
            name: "Jumper Game",
            questGroups: new List<QuestGroup>() { QuestGroups.MAIN },
            description: "Run run run!",
            mainObjectives: new Dictionary<string, MainObjective>() {
                {
                    "arrive-at-finish-line",
                    new MainObjective(
                        name: "Survive for 60 seconds.",
                        description: "Press space to jump. Avoid all obstacles for 60 seconds.",
                        onComplete: () => {
                            UIManager.Instance.taskCompleteModal.SetContents(
                                description: "Hooray! You got to the finish line!",
                                exitEvent: () => {
                                    JumperGame.Instance.Cleanup();
                                    this.jumperGame.SetActive(false);
                                }
                            );
                            UIManager.Instance.taskCompleteModal.ToggleToNonHub(true);
                        },
                        onFail: () => {
                            UIManager.Instance.taskFailedModal.SetContents(
                                description: "Oops! You bumped into something.",
                                retryEvent: () => {
                                    JumperGame.Instance.Reset();
                                    JumperGame.Instance.TriggerGameStart();
                                },
                                exitEvent: () => {
                                    JumperGame.Instance.Cleanup();
                                    GameManager.Instance.SleepToNextDay();
                                    this.gui.SetActive(false);
                                    this.jumperGame.SetActive(false);
                                    this.UpdateStress();
                                },
                                enableRetry: false
                            );
                            UIManager.Instance.taskFailedModal.ToggleToNonHub(true);
                        }
                    )
                }
            },
            optionalObjectives: new Dictionary<string, OptionalObjective>(),
            onComplete: () => {
                Debug.Log("Jumper game completed!");
            }
        );
        /// locked initially
        quest.Lock();
        GameManager.Instance.AddActiveQuest(
            questKey: this.questKey,
            quest: quest
        );
    }

    protected override void RunIntro(Quest quest)
    {
        base.RunIntro(quest);
        this.cutscene.SetActive(true);
        UIManager.Instance.questModal.SetContents(
            title: quest.name,
            description: quest.description,
            startEvent: () => {
                GameManager.Instance.ToggleHubMode(false);
                this.gui.SetActive(true);
                this.jumperGame.SetActive(true);
                this.tutorial.SetActive(true);
            }
        );
        UIManager.Instance.questModal.Toggle(true);
    }
}
