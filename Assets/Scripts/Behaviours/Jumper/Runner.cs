using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(MonoTagAssigner))]
public class Runner : MonoBehaviour
{
    public float jumpSpeed = 400f;
    private Rigidbody2D rb;
    private bool isJumpPressed = false;

    void Awake()
    {
        this.GetComponent<MonoTagAssigner>().monoTag = Constants.MonoTag.JUMPER_GAME_RUNNER;
    }

    void Start()
    {
        this.rb = this.GetComponent<Rigidbody2D>();
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

        this.isJumpPressed = Input.GetKeyDown(KeyMaps.JUMPER_HOP);
        if(this.isJumpPressed)
        {
            this.Jump();
        }
    }
}
