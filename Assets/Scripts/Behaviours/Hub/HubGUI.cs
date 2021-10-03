using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HubGUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Image dayDisplay;
    [SerializeField] private Image stressSliderImage;
    [SerializeField] private Image moodImage;

    [Header("Mood Sprites")]
    [SerializeField] private Sprite happyMood;
    [SerializeField] private Sprite mehMood;
    [SerializeField] private Sprite mmhhMood;
    [SerializeField] private Sprite peevedMood;

    [Header("Day Display")]
    [SerializeField] private Sprite day1;
    [SerializeField] private Sprite day2;
    [SerializeField] private Sprite day3;

    public void UpdateDay(int day)
    {
        Sprite daySprite = this.GetDayImage(day);
        this.dayDisplay.sprite = daySprite;
    }

    private Sprite GetDayImage(int day)
    {
        switch(day)
        {
            case 1:
                return this.day1;
            case 2:
                return this.day2;
        }
        return this.day3;
    }

    public void UpdateStress(float stress, float maxStress)
    {
        float normalizedStress = stress / maxStress;
        this.stressSliderImage.fillAmount = normalizedStress;
        Sprite mood = this.GetMood(normalizedStress);
        moodImage.sprite = mood;
    }

    private Sprite GetMood(float normalizedStress)
    {
        if(normalizedStress <= 0.25f)
            return this.happyMood;
        else if(normalizedStress <= 0.5f)
            return this.mehMood;
        else if(normalizedStress <= 0.75f)
            return this.mmhhMood;
        return this.peevedMood;
    }
}
