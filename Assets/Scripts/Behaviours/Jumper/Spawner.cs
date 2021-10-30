using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimerUtils;

public class Spawner : MonoBehaviour
{
    public float interval = 3f;
    [SerializeField] public Vector3 offsetPosition;
    [SerializeField] private List<GameObject> prefabArsenal;

    private TimedAction timedAction;

    void Awake()
    {
        if(this.prefabArsenal.Count == 0)
            throw new NullReferenceException("\"prefabArsenal\" field must have at least 1 item.");
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
                int index = UnityEngine.Random.Range(0, this.prefabArsenal.Count);
                GameObject prefab = this.prefabArsenal[index];
                GameObject child = Instantiate(prefab, position, Quaternion.identity);
                child.transform.SetParent(this.transform);
            },
            triggerOnInitial: true
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
