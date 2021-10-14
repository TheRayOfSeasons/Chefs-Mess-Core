using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestManagement;

public class TypingGameQuestProvider : QuestProvider
{
    [SerializeField] private GameObject typingGame;
    [SerializeField] private GameObject gui;
    [SerializeField] private GameObject cutscene;
    [SerializeField] private GameObject tutorial;

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

    private void UpdateStress()
    {
        Constants.Difficulty difficulty = GameManager.Instance.GetCurrentDifficulty();
        float stress = TyperMeta.stress[difficulty];
        GameManager.Instance.stress.Add(stress);
    }

    private void SetupQuest()
    {
        GameManager.Instance.AddActiveQuest(
            questKey: this.questKey,
            quest: new Quest(
                name: "Typing Game",
                questGroups: new List<QuestGroup>() { QuestGroups.MAIN, QuestGroups.PRIMARY },
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
                                    },
                                    exitEvent: () => {
                                        Typer.Instance.Reset();
                                        this.ToggleGame(false);
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
                    Debug.Log("Typing Game Completed");
                }
            )
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
                this.tutorial.SetActive(true);
            }
        );
        UIManager.Instance.questModal.Toggle(true);
    }
}
