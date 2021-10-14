using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePuzzleTutorial : Tutorial
{
    // NOTE: This code sucks. It's not DRY enough.
    // Only made this way due to time constraint.
    [SerializeField] private GameObject tilePuzzle;

    private void ToggleGame(bool toggle)
    {
        this.tilePuzzle.SetActive(toggle);
    }

    public override void OnExitEvent()
    {
        base.OnExitEvent();
        this.tilePuzzle.SetActive(true);
        TilePuzzle.Instance.Reset();
    }
}
