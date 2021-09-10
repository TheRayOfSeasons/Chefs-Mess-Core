using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TilePuzzleGUI : MonoBehaviour
{
    [SerializeField] private Slider timerSlider;

    public void SetupTimerSlider(float maxTime)
    {
        this.timerSlider.maxValue = maxTime;
        this.timerSlider.value = maxTime;
    }

    public void UpdateTimerSlider(float time)
    {
        this.timerSlider.value = time;
    }
}
