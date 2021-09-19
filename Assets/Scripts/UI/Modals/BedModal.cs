using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UIComponents;

public class BedModal : Modal
{
    public Button actionButtonComponent;

    public override void Initialize()
    {
        base.Initialize();
        this.actionButtonComponent.onClick.AddListener(() => {
            GameManager.Instance.SleepToNextDay();
            this.Toggle(false);
        });
    }
}
