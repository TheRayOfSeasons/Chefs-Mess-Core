using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UIComponents;

public class QuestModal : Modal
{
    public Text titleComponent;
    public Text descriptionComponent;
    public Button startButtonComponent;

    public void SetContents(string title, string description, UnityAction startEvent)
    {
        this.SetTitle(title);
        this.SetDescription(description);
        this.SetStartEvent(startEvent);
    }

    public void SetTitle(string title)
    {
        this.titleComponent.text = title;
    }

    public void SetDescription(string description)
    {
        this.descriptionComponent.text = description;
    }

    public void SetStartEvent(UnityAction startEvent)
    {
        this.startButtonComponent.onClick.RemoveAllListeners();
        this.startButtonComponent.onClick.AddListener(() => {
            this.ToggleToNonHub(false);
            AudioManager.Instance.PlayTaskAudio(TaskAudio.ACCEPTED);
            startEvent();
        });
    }
}
