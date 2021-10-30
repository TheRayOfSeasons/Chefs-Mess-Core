using System;
using UnityEngine;

public class JumperGameTutorial : Tutorial
{
    [SerializeField] private GameObject jumperGame;

    private void ToggleGame(bool toggle)
    {
        this.jumperGame.SetActive(toggle);
    }

    public override void OnExitEvent()
    {
        base.OnExitEvent();
        this.ToggleGame(true);
        try
        {
            JumperGame.Instance.Reset();
        }
        catch(NullReferenceException)
        {
            /*
                Do nothing. In this case, the JumperGame
                wasn't initialized yet. The start function
                will handle it.
            */
        }
        JumperGame.Instance.TriggerGameStart();
    }
}
