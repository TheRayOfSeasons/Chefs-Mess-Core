using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestManagement;

public class TypingGameQuestProvider : QuestProvider
{
    [SerializeField] private GameObject typingGame;
    [SerializeField] private GameObject gui;

    public override void Initialize()
    {
        base.Initialize();
        this.questKey = "typing-game";
        this.SetupQuest();
    }

    private void ToggleGame(bool toggle)
    {
        this.typingGame.SetActive(toggle);
        this.gui.SetActive(toggle);
    }

    private void SetupQuest()
    {
        GameManager.Instance.AddActiveQuest(
            questKey: this.questKey,
            quest: new Quest(
                name: "Typing Game",
                description: "Type each word to cut the vegetables.",
                mainObjectives: new Dictionary<string, MainObjective>() {
                    {
                        "type-in-all-words",
                        new MainObjective(
                            name: "Type in all words that appear.",
                            description: "",
                            onComplete: () => {
                                UIManager.Instance.taskCompleteModal.SetContents(
                                    description: "Nice! You inputted all the words.",
                                    exitEvent: () => {
                                        this.ToggleGame(false);
                                    }
                                );
                                UIManager.Instance.taskCompleteModal.ToggleToNonHub(true);
                            },
                            onFail: () => {
                                UIManager.Instance.taskFailedModal.SetContents(
                                    description: "You ran out of time :(",
                                    retryEvent: () => {
                                        Typer.Instance.Reset();
                                        Typer.Instance.TriggerGameStart();
                                        this.ToggleGame(false);
                                    },
                                    exitEvent: () => {
                                        Typer.Instance.Reset();
                                        this.ToggleGame(false);
                                    }
                                );
                                UIManager.Instance.taskFailedModal.ToggleToNonHub(true);
                            }
                        )
                    }
                },
                optionalObjectives: new Dictionary<string, OptionalObjective>(),
                onComplete: () => {
                    Debug.Log("Typing Game Completed");
                }
            )
        );
    }

    public override void RunQuestIntro()
    {
        base.RunQuestIntro();
        Quest quest = GameManager.Instance.questDefinitions.GetQuest(questKey: this.questKey);
        Debug.Log("typing quest");
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
                    this.ToggleGame(true);
                    Typer.Instance.Reset();
                    Typer.Instance.TriggerGameStart();
                }
            );
            UIManager.Instance.questModal.Toggle(true);
        }
    }
}
