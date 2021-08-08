using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Awake()
    {
        this.GetComponent<MonoTagAssigner>().monoTag = Constants.MonoTag.JUMPER_GAME_RUNNER;
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
        this.isJumpPressed = false;
    }

    void FixedUpdate()
    {
        if(!JumperGame.Instance.IsOngoing)
            return;

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
        Debug.Log(this.canJump);

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
                JumperGame.Instance.HandleLose();
            }
        }
    }
}
