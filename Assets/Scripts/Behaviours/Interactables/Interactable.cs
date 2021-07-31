using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(MonoTagAssigner))]
public class Interactable : MonoBehaviour
{
    public float interactabilityRadius = 0.5f;

    private Rigidbody2D rb;
    private CircleCollider2D circleCollider2D;

    void Awake()
    {
        this.GetComponent<MonoTagAssigner>().monoTag = Constants.MonoTag.INTERACTABLE;
    }

    void Start()
    {
        this.rb = this.GetComponent<Rigidbody2D>();
        this.rb.gravityScale = 0;

        this.circleCollider2D = this.GetComponent<CircleCollider2D>();
        this.circleCollider2D.isTrigger = true;
        this.circleCollider2D.radius = this.interactabilityRadius;

        this.Initialize();
    }

    public virtual void Initialize() {}

    public void Interact()
    {
        this.OnInteract();
    }

    public virtual void OnInteract() {}

    public virtual void OnExitInteraction() {}

    void OnTriggerEnter2D(Collider2D collider)
    {
        GameManager.Instance.CallInteractionSubscriptions(this.gameObject);
    }
}
