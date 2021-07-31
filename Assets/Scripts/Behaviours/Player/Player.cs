using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(MonoTagAssigner))]
public class Player : MonoBehaviour
{
    public bool isLocalPlayer = true;
    public float interactabilityRadius = 0.5f;
    private PlayerController playerController;
    private CircleCollider2D circleCollider2D;

    private List<Interactable> interactables = new List<Interactable>();

    void Awake()
    {
        this.GetComponent<MonoTagAssigner>().monoTag = Constants.MonoTag.PLAYER;
    }

    void Start()
    {
        this.playerController = this.GetComponent<PlayerController>();
        this.playerController.isControllable = this.isLocalPlayer;

        this.circleCollider2D = this.GetComponent<CircleCollider2D>();
        this.circleCollider2D.isTrigger = true;
        this.circleCollider2D.radius = this.interactabilityRadius;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject collidedObject = collider.gameObject;
        if(!collidedObject.GetComponent<MonoTagAssigner>())
            return;

        MonoTagAssigner tag = collidedObject.GetComponent<MonoTagAssigner>();
        if(tag.monoTag != Constants.MonoTag.INTERACTABLE)
            return;

        Interactable interactable = collidedObject.GetComponent<Interactable>();
        this.interactables.Add(interactable);
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        GameObject collidedObject = collider.gameObject;
        if(!collidedObject.GetComponent<MonoTagAssigner>())
            return;

        MonoTagAssigner tag = collidedObject.GetComponent<MonoTagAssigner>();
        if(tag.monoTag != Constants.MonoTag.INTERACTABLE)
            return;

        Interactable interactable = collidedObject.GetComponent<Interactable>();
        this.interactables.Remove(interactable);
    }

    void RunInteractionControls()
    {
        if(Input.GetKeyDown(KeyMaps.INTERACT))
        {
            Interactable interactable = this.interactables.Last();
            if(!interactable)
                return;

            interactable.Interact();
        }
    }

    void Update()
    {
        if(this.interactables.Count > 0)
        {
            RunInteractionControls();
        }
    }
}
