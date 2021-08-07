using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimerUtils;

public class Spawner : MonoBehaviour
{
    public float interval = 5f;
    [SerializeField] public Vector3 offsetPosition;
    [SerializeField] private GameObject prefab;

    private TimedAction timedAction;

    void Start()
    {
        this.timedAction = new TimedAction(
            maxTime: this.interval,
            action: () => {
                Vector3 position = new Vector3(
                    this.transform.position.x + this.offsetPosition.x,
                    this.transform.position.y + this.offsetPosition.y,
                    this.transform.position.z + this.offsetPosition.z
                );
                Instantiate(this.prefab, position, Quaternion.identity);
            }
        );
    }


    void Update()
    {
        if(JumperGame.Instance.IsOngoing)
        {
            this.timedAction.Run(Time.deltaTime);
        }
    }
}
