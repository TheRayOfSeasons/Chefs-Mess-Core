using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CutsceneController : MonoBehaviour
{
    public Animator animator;

    void Awake()
    {
        this.animator = this.GetComponent<Animator>();
    }

    void OnEnable()
    {
        this.animator.Play("Intro");
    }

    public void EndAnimation()
    {
        this.gameObject.SetActive(false);
    }
}
