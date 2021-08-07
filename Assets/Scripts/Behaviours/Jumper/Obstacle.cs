using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(MonoTagAssigner))]
public class Obstacle : MonoBehaviour
{
    private Vector3 direction = Vector3.left;
    private float speed = 3f;

    void Awake()
    {
        this.GetComponent<MonoTagAssigner>().monoTag = Constants.MonoTag.JUMPER_GAME_OBSTACLE;
    }

    void Update()
    {
        if(!JumperGame.Instance.IsOngoing)
            return;

        Vector3 newPosition = this.direction * this.speed * Time.deltaTime;
        this.transform.Translate(newPosition);
    }
}
