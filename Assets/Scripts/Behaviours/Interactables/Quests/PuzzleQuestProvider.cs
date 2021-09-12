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

    private void UpdateStress()
    {
        Constants.Difficulty difficulty = GameManager.Instance.GetCurrentDifficulty();
        float stress = TilePuzzleMeta.stress[difficulty];
        GameManager.Instance.stress.Add(stress);
    }

    private void SetupQuest()
    {
        Quest quest = new Quest(
            name: "Puzzle Quest",
            questGroups: new List<QuestGroup>() { QuestGroups.MAIN },
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
                Debug.Log("Puzzle Quest completed!");
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
