using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnappingDetector : MonoBehaviour
{
    public bool occupied = false;
    public int snapIndex;

    void Start()
    {
        Debug.DrawRay(this.transform.position, Vector2.left, Color.green, 200f);
        Debug.DrawRay(this.transform.position, Vector2.up, Color.green, 200f);
        Debug.DrawRay(this.transform.position, Vector2.right, Color.green, 200f);
        Debug.DrawRay(this.transform.position, Vector2.down, Color.green, 200f);
    }
}
