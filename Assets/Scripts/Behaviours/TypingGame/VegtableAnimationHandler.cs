using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class VegtableAnimationHandler : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        this.animator = this.GetComponent<Animator>();
    }

    public void Toggle(bool toggle)
    {
        this.gameObject.SetActive(toggle);
    }

    public void Reset()
    {
        this.animator.Play("Full");
    }

    public void Cut(int slice)
    {
        this.animator.Play($"Cut-{slice}");
    }
}
