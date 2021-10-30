using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(MonoTagAssigner))]
public class Runner : MonoBehaviour
{
    public float jumpSpeed = 50f;
    [SerializeField] private Vector2 raycasterPosition = new Vector2(-9f, -1.9f);
    private Vector2 raycasterDirection = Vector2.right;
    private float raycasterDistance = 50f;
    private Rigidbody2D rb;
    private bool isJumpPressed = false;
    private bool canJump = false;
    private Animator animator;

    void Awake()
    {
        this.animator = this.GetComponent<Animator>();
        this.GetComponent<MonoTagAssigner>().monoTag = Constants.MonoTag.JUMPER_GAME_RUNNER;
    }

    public void Reset()
    {
        this.animator.Play("Running");
    }

    void Start()
    {
        this.rb = this.GetComponent<Rigidbody2D>();
        Debug.DrawRay(
            this.raycasterPosition,
            this.raycasterDirection,
            Color.yellow,
            this.raycasterDistance
        );
    }

    void Jump()
    {
        this.rb.AddForce(Vector2.up * this.jumpSpeed);
        this.canJump = false;
        this.animator.Play("Jumping");
    }

    void FixedUpdate()
    {
        if(!JumperGame.Instance.IsOngoing)
            return;

        this.animator.SetBool("isJumping", !this.canJump);
        RaycastHit2D[] hits = Physics2D.RaycastAll(
            this.raycasterPosition,
            this.raycasterDirection,
            this.raycasterDistance
        );
        foreach(RaycastHit2D hit in hits)
        {
            if(hit.collider.gameObject == this.gameObject)
            {
                this.canJump = true;
                break;
            }
            this.canJump = false;
        }

        this.isJumpPressed = Input.GetKey(KeyMaps.JUMPER_HOP);
        if(this.isJumpPressed && this.canJump)
        {
            this.Jump();
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(!JumperGame.Instance.IsOngoing)
            return;

        if(collider.GetComponent<MonoTagAssigner>())
        {
            MonoTagAssigner tag = collider.GetComponent<MonoTagAssigner>();
            if(tag.monoTag == Constants.MonoTag.JUMPER_GAME_OBSTACLE)
            {
                this.animator.Play("Hurt");
                JumperGame.Instance.HandleLose();
            }
        }
    }
}
