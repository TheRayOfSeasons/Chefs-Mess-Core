using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class KnifeAnimationHandler : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        this.animator = this.GetComponent<Animator>();
    }

    public void Reset()
    {
        this.Toggle(false);
    }

    public void Toggle(bool toggle)
    {
        this.gameObject.SetActive(toggle);
    }

    public void Move()
    {
        try
        {
            this.animator.Play("Move");
        }
        catch(NullReferenceException)
        {
            // Do nothing
        }
    }
}
