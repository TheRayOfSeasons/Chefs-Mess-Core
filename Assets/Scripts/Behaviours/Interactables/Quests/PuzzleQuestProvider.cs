using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestManagement;

public class PuzzleQuestProvider : QuestProvider
{
    [SerializeField] private GameObject tilePuzzle;
    [SerializeField] private GameObject tileGUI;

    public override void Initialize()
    {
        base.Initialize();
        this.questKey = "puzzle";
        this.SetupQuest();
    }

    private void ToggleGame(bool toggle)
    {
        this.tileGUI.SetActive(toggle);
        this.tilePuzzle.SetActive(toggle);
    }

    private void SetupQuest()
    {
        GameManager.Instance.AddActiveQuest(
            questKey: this.questKey,
            quest: new Quest(
                name: "Puzzle Quest",
                description: "Solve a puzzle",
                mainObjectives: new Dictionary<string, MainObjective>() {
                    {
                        "solve-the-puzzle",
                        new MainObjective(
                            name: "Solve the puzzle.",
                            description: "Solve the 3x3 puzzle",
                            onComplete: () => {
                                UIManager.Instance.taskCompleteModal.SetContents(
                                    description: "Nice! You solved the puzzle!",
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
                                        TilePuzzle.Instance.Reset();
                                    },
                                    exitEvent: () => {
                                        TilePuzzle.Instance.Reset();
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
                    Debug.Log("Puzzle Quest completed!");
                }
            )
        );
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
                    this.ToggleGame(true);
                    TilePuzzle.Instance.Reset();
                }
            );
            UIManager.Instance.questModal.Toggle(true);
        }
    }
}
