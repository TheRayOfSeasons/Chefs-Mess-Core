using UnityEngine;

public class Follower2D : MonoBehaviour
{
    public GameObject target;

    void Update()
    {
        this.transform.position = new Vector3(
            this.target.transform.position.x,
            this.target.transform.position.y,
            this.transform.position.z
        );
    }
}
