using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UIComponents;

public class TaskCompleteModal : Modal
{
    public Text descriptionComponent;
    private UnityAction onExit;

    public override void OnModalExit()
    {
        base.OnModalExit();
        if(this.onExit != null)
        {
            this.onExit();
        }
    }

    public void SetContents(string description, UnityAction exitEvent)
    {
        this.SetDescription(description);
        this.SetOnExitEvent(exitEvent);
    }

    public void SetDescription(string description)
    {
        this.descriptionComponent.text = description;
    }

    public void SetOnExitEvent(UnityAction exitEvent)
    {
        this.onExit = exitEvent;
    }
}
