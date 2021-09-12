using System;
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

    void Awake()
    {
        if(this.prefab == null)
            throw new NullReferenceException("\"prefab\" field must be assigned to \"Spawner\" component.");
    }

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
                GameObject child = Instantiate(this.prefab, position, Quaternion.identity);
                child.transform.SetParent(this.transform);
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
