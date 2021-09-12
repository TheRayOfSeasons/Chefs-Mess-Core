using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestManagement;

public class JumperGameQuestProvider : QuestProvider
{
    [SerializeField] private GameObject jumperGame;

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
        GameManager.Instance.AddActiveQuest(
            questKey: this.questKey,
            quest: new Quest(
                name: "Jumper Game",
                questGroups: new List<QuestGroup>() { QuestGroups.MAIN, QuestGroups.PRIMARY },
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
                                        this.jumperGame.SetActive(false);
                                        this.UpdateStress();
                                    }
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
            )
        );
    }

    protected override void RunIntro(Quest quest)
    {
        base.RunIntro(quest);
        UIManager.Instance.questModal.SetContents(
            title: quest.name,
            description: quest.description,
            startEvent: () => {
                GameManager.Instance.ToggleHubMode(false);
                this.jumperGame.SetActive(true);
                JumperGame.Instance.TriggerGameStart();
            }
        );
        UIManager.Instance.questModal.Toggle(true);
    }
}
