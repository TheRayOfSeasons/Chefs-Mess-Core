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

    private void SetupQuest()
    {
        GameManager.Instance.AddActiveQuest(
            questKey: this.questKey,
            quest: new Quest(
                name: "Jumper Game",
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
        GameManager.Instance.questDefinitions.ClearMainObjective("jumper", "arrive-at-finish-line");
    }

    public override void RunQuestIntro()
    {
        base.RunQuestIntro();
        Quest quest = GameManager.Instance.questDefinitions.GetQuest(questKey: questKey);

        if(quest.isCompleted)
        {
            UIManager.Instance.questDoneModal.SetTitle(quest.name);
            UIManager.Instance.questDoneModal.Toggle(true);
        }
        else
        {
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
}
