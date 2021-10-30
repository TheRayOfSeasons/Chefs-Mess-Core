using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestManagement;

public class PuzzleQuestProvider : QuestProvider
{
    [SerializeField] private GameObject tilePuzzle;
    [SerializeField] private GameObject tileGUI;
    [SerializeField] private GameObject tutorial;
    [SerializeField] private GameObject cutscene;

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
            name: "Puzzle Recipe",
            questGroups: new List<QuestGroup>() { QuestGroups.PRIMARY },
            description: "Arrange a 3x3 tile image puzzle",
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
                                    TilePuzzle.Instance.ClearTiles();
                                }
                            );
                            UIManager.Instance.taskCompleteModal.ToggleToNonHub(true);
                        },
                        onFail: () => {
                            UIManager.Instance.taskFailedModal.SetContents(
                                description: "You ran out of time :(",
                                retryEvent: () => {
                                    TilePuzzle.Instance.Reset();
                                    this.UpdateStress();
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
                this.tileGUI.SetActive(true);
                this.tutorial.SetActive(true);
            }
        );
        UIManager.Instance.questModal.Toggle(true);
    }
}
