using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TyperTutorial : Tutorial
{
    [SerializeField] private GameObject typingGame;

    private void ToggleGame(bool toggle)
    {
        this.typingGame.SetActive(toggle);
    }

    public override void OnExitEvent()
    {
        base.OnExitEvent();
        this.ToggleGame(true);
        Typer.Instance.Reset();
        Typer.Instance.TriggerGameStart();
    }
}
