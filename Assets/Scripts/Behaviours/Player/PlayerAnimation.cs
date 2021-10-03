using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator animator;

    private Vector3 originalScale;

    void Start()
    {
        this.originalScale = this.transform.localScale;
    }

    public void Walk(Vector2 direction)
    {
        float magnitude = direction.normalized.magnitude;
        if(magnitude == 0f)
        {
            animator.Play("Idle");
        }
        else
        {
            animator.Play("Walk");
        }
        animator.SetInteger("magnitude", (int)Mathf.Ceil(magnitude));

        switch(direction.x)
        {
            case 1.0f:
                this.transform.localScale = new Vector2(this.originalScale.x * -1.0f, this.originalScale.y);
                break;
            case -1.0f:
                this.transform.localScale = this.originalScale;
                break;
        }
    }
}
