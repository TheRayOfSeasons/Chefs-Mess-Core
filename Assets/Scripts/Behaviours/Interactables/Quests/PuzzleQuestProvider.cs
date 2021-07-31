using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestManagement;

public class PuzzleQuestProvider : QuestProvider
{
    [SerializeField] private GameObject tilePuzzle;

    public override void Initialize()
    {
        base.Initialize();
        this.questKey = "puzzle";
        this.SetupQuest();
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
                                Debug.Log("Puzzle objective completed!");
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
        UIManager.Instance.questModal.SetContents(
            title: quest.name,
            description: quest.description,
            startEvent: () => {
                UIManager.Instance.questModal.Toggle(false);
                this.tilePuzzle.SetActive(!this.tilePuzzle.activeSelf);
            }
        );
        UIManager.Instance.questModal.Toggle(true);
    }
}
