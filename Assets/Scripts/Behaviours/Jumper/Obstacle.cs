using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(MonoTagAssigner))]
public class Obstacle : MonoBehaviour
{
    private Vector3 target;
    private Vector3 direction = Vector3.left;
    private float speed = 3f;
    private float recyclingThreshold = 0.5f;
    private Rigidbody2D rb;

    void Awake()
    {
        this.GetComponent<MonoTagAssigner>().monoTag = Constants.MonoTag.JUMPER_GAME_OBSTACLE;
    }

    void Start()
    {
        this.target = new Vector3(
            -9,
            this.transform.position.y,
            this.transform.position.z
        );
    }

    void Update()
    {
        if(!JumperGame.Instance.IsOngoing)
            return;

        Vector3 newPosition = this.direction * this.speed * Time.deltaTime;
        this.transform.Translate(newPosition);

        float distance = Vector2.Distance(this.transform.position, this.target);
        if(distance < this.recyclingThreshold)
        {
            Destroy(this.gameObject);
        }
    }
}
