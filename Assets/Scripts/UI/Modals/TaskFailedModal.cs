using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UIComponents;

public class TaskFailedModal : Modal
{
    public Text descriptionComponent;
    public Button tryAgainButtonComponent;
    private UnityAction onExit;

    public override void OnModalExit()
    {
        base.OnModalExit();
        if(this.onExit != null)
        {
            this.onExit();
        }
    }

    public void SetContents(string description, UnityAction retryEvent, UnityAction exitEvent)
    {
        this.SetDescription(description);
        this.SetRetryEvent(retryEvent);
        this.SetOnExitEvent(exitEvent);
    }

    public void SetDescription(string description)
    {
        this.descriptionComponent.text = description;
    }

    public void SetRetryEvent(UnityAction retryEvent)
    {
        this.tryAgainButtonComponent.onClick.RemoveAllListeners();
        this.tryAgainButtonComponent.onClick.AddListener(() => {
            this.ToggleToNonHub(false);
            retryEvent();
        });
    }

    public void SetOnExitEvent(UnityAction exitEvent)
    {
        this.onExit = exitEvent;
    }
}
