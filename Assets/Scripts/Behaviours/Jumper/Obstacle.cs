using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(MonoTagAssigner))]
public class Obstacle : MonoBehaviour
{
    void Awake()
    {
        this.GetComponent<MonoTagAssigner>().monoTag = Constants.MonoTag.JUMPER_GAME_OBSTACLE;
    }

    void Start()
    {

    }

    void Update()
    {
        if(!JumperGame.Instance.IsOngoing)
            return;

        // float speed = JumperGame.Instance.GetCurrentSpeed() * 0.1f;
        this.transform.position = Vector2.Lerp(this.transform.position, new Vector2(-9, this.transform.position.y), 0.1f);
    }
}
