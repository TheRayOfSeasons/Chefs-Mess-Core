using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UIComponents;

public class QuestLockedModal : Modal
{
    public Text titleComponent;

    public void SetTitle(string title)
    {
        this.titleComponent.text = title;
    }
}
