using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIComponents;

public class PauseModal : Modal
{
    public override void OnModalDisplay()
    {
        GameManager.Instance.Pause();
    }

    public override void OnModalExit()
    {
        GameManager.Instance.Resume();
    }
}
