using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int correctIndex;
    public int currentIndex;

    public bool isPlacedCorrectly
    {
        get { return correctIndex == currentIndex; }
    }
}
