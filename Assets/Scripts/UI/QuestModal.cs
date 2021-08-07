using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


[RequireComponent(typeof(RectTransform))]
public class QuestModal : MonoBehaviour
{
    public GameObject body;
    public Text titleComponent;
    public Text descriptionComponent;
    public Button startButtonComponent;
    public Button exitButtonComponent;

    void Start()
    {
        this.Toggle(false);
        this.exitButtonComponent.onClick.AddListener(() => {
            this.Toggle(false);
        });
    }

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
            this.Toggle(false);
            startEvent();
        });
    }

    public void Toggle(bool toggle)
    {
        this.body.SetActive(toggle);
    }
}
