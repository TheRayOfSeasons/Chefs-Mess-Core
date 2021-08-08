using UnityEngine;

public class Follower2D : MonoBehaviour
{
    public GameObject target;
    public Vector2 defaultPosition = new Vector2(0, 0);

    void Update()
    {
        if(target.activeInHierarchy)
        {
            this.transform.position = new Vector3(
                this.target.transform.position.x,
                this.target.transform.position.y,
                this.transform.position.z
            );
        }
        else
        {
            this.transform.position = new Vector3(
                this.defaultPosition.x,
                this.defaultPosition.y,
                this.transform.position.z
            );
        }
    }
}
