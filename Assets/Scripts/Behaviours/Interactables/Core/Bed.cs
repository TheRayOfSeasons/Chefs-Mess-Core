using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Deprecated
public class Bed : Interactable
{
    public override void OnInteract()
    {
        base.OnInteract();
        if(GameManager.Instance.AllowedToSleep())
        {
            UIManager.Instance.bedModal.Toggle(true);
        }
        else
        {
            UIManager.Instance.cantSleepModal.Toggle(true);
        }
    }
}
