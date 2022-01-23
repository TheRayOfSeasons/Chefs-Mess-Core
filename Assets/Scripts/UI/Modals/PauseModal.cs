using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UIComponents;

public class PauseModal : Modal
{
    public Button giveUpButtonComponent;

    public override void Initialize()
    {
        base.Initialize();
        this.giveUpButtonComponent.onClick.AddListener(() => {
            GameManager.Instance.GiveUpQuest();
            this.Toggle(false);
        });
        GameManager.Instance.AddMiniGameHook(isInMiniGame => {
            this.giveUpButtonComponent.gameObject.SetActive(isInMiniGame);
        });
    }

    public override void OnModalDisplay()
    {
        GameManager.Instance.Pause();
    }

    public override void OnModalExit()
    {
        GameManager.Instance.Resume();
    }
}
