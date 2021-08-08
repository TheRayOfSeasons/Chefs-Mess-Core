using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{
    public bool isControllable = true;
    public float speed = 5.0f;

    private Rigidbody2D rb;

    void Start()
    {
        this.rb = this.GetComponent<Rigidbody2D>();
        this.rb.gravityScale = 0;
    }

    public void Move()
    {
        float directionX = 0;
        float directionY = 0;

        if(KeyMaps.IsAnyKeyPressed(KeyMaps.LEFT_MOVE_KEYS))
        {
            directionX = -1;
        }
        else if(KeyMaps.IsAnyKeyPressed(KeyMaps.RIGHT_MOVE_KEYS))
        {
            directionX = 1;
        }

        if(KeyMaps.IsAnyKeyPressed(KeyMaps.UP_MOVE_KEYS))
        {
            directionY = 1;
        }
        else if(KeyMaps.IsAnyKeyPressed(KeyMaps.DOWN_MOVE_KEYS))
        {
            directionY = -1;
        }

        Vector2 movement = new Vector2(directionX * speed, directionY * speed);
        this.rb.velocity = movement;
    }

    void FixedUpdate()
    {
        Debug.Log($"is controllable: {this.isControllable} | ishubmode: {GameManager.Instance.IsHubMode}");
        if(!this.isControllable && !GameManager.Instance.IsHubMode)
        {
            this.rb.velocity = new Vector2(0, 0);
            return;
        }

        Move();
    }
}
