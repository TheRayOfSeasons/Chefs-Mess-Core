using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UIComponents;

public class QuestDoneModal : Modal
{
    public Text titleComponent;

    public void SetTitle(string title)
    {
        this.titleComponent.text = title;
    }
}
