using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int correctIndex;
    public int currentIndex;
    public bool selected = false;
    public Vector2 autoTarget;

    public bool isPlacedCorrectly
    {
        get { return correctIndex == currentIndex; }
    }

    void Update()
    {
        if(this.autoTarget != null && !this.selected)
        {
            this.transform.position = Vector2.Lerp(
                this.transform.position,
                this.autoTarget,
                0.1f
            );
        }
    }
}
