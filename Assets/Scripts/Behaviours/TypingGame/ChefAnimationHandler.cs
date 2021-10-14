using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ChefAnimationHandler : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        this.animator = this.GetComponent<Animator>();
    }

    public void Chop()
    {
        this.animator.Play("Chopping");
    }

    public void ThumbsUp()
    {
        this.animator.Play("ThumbsUp");
    }

    public void UnpausePlayerInput()
    {
        Typer.Instance.ToggleRoundPause(false);
        Typer.Instance.RenderWord();
    }
}
