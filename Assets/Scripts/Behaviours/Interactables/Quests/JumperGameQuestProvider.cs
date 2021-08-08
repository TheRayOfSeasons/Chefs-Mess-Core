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
                                Debug.Log("Jumper game completed!");
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

    public override void RunQuestIntro()
    {
        base.RunQuestIntro();
        Quest quest = GameManager.Instance.questDefinitions.GetQuest(questKey: questKey);
        UIManager.Instance.questModal.SetContents(
            title: quest.name,
            description: quest.description,
            startEvent: () => {
                GameManager.Instance.ToggleHubMode(false);
                this.jumperGame.SetActive(!this.jumperGame.activeSelf);
                JumperGame.Instance.TriggerGameStart();
            }
        );
        UIManager.Instance.questModal.Toggle(true);
    }
}
