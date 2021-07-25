﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestManagement;

public class QuestDefinitions
{
    public Dictionary<string, Quest> activeQuests = new Dictionary<string, Quest>();

    public QuestDefinitions()
    {
        Quest puzzleQuest = this.GetPuzzleQuest();
        this.activeQuests.Add("puzzle", puzzleQuest);
    }

    private Quest GetPuzzleQuest()
    {
        return new Quest(
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
        );
    }

    private bool ValidateQuestCall(string questkey)
    {
        bool hasKey = this.activeQuests.ContainsKey(questkey);
        if(!hasKey)
        {
            Debug.Log("Quest is not included in active quests.");
        }
        return hasKey;
    }

    private bool ValidateMainObjectiveCall(Quest quest, string objectiveKey)
    {
        bool hasKey = quest.mainObjectives.ContainsKey(objectiveKey);
        if(!hasKey)
        {
            Debug.Log($"Objective is not included among the main objectives of quest {quest.name}.");
        }
        return hasKey;
    }

    private bool ValidateOptionalObjectiveCall(Quest quest, string objectiveKey)
    {
        bool hasKey = quest.optionalObjectives.ContainsKey(objectiveKey);
        if(!hasKey)
        {
            Debug.Log($"Objective is not included among the optional objectives of quest {quest.name}.");
        }
        return hasKey;
    }

    public void ClearMainObjective(string questKey, string objectiveKey)
    {
        if(!ValidateQuestCall(questKey))
            return;
        Quest quest = this.activeQuests[questKey];
        if(!ValidateMainObjectiveCall(quest, objectiveKey))
            return;
        this.activeQuests[questKey].mainObjectives[objectiveKey].OverrideCompletion();
        this.CheckCompletions();
    }

    public void ClearOptionalObjective(string questKey, string objectiveKey)
    {
        if(!ValidateQuestCall(questKey))
            return;
        Quest quest = this.activeQuests[questKey];
        if(!ValidateOptionalObjectiveCall(quest, objectiveKey))
            return;
        quest.optionalObjectives[objectiveKey].OverrideCompletion();
        this.CheckCompletions();
    }

    public void CheckCompletions()
    {
        foreach(KeyValuePair<string, Quest> questObject in activeQuests)
        {
            Quest quest = questObject.Value;
            quest.Update();

            foreach(KeyValuePair<string, MainObjective> objectiveObject in quest.mainObjectives)
            {
                MainObjective objective = objectiveObject.Value;
                objective.Update();
            }

            foreach(KeyValuePair<string, OptionalObjective> objectiveObject in quest.optionalObjectives)
            {
                OptionalObjective objective = objectiveObject.Value;
                objective.Update();
            }
        }
    }
}