using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HubGUI : MonoBehaviour
{
    [SerializeField] private Slider stressSlider;

    public void SetupStressSlider(float maxStress)
    {
        this.stressSlider.maxValue = maxStress;
        this.stressSlider.value = maxStress;
    }

    public void UpdateStress(float stress)
    {
        this.stressSlider.value = stress;
    }
}
