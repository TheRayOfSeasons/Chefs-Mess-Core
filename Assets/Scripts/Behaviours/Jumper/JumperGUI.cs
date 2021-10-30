using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumperGUI : MonoBehaviour
{
    [SerializeField] private Image timerSlider;

    [Header("Fill Sprites")]
    [SerializeField] private Sprite green;
    [SerializeField] private Sprite yellow;
    [SerializeField] private Sprite red;

    [Header("Tutorial")]
    [SerializeField] private GameObject tutorialUI;
    public GameObject TutorialUI
    {
        get { return this.tutorialUI; }
    }

    private Sprite GetTimerSprite(float normalizedTime)
    {
        if(normalizedTime >= 0.67f && normalizedTime <= 1f)
        {
            return this.green;
        }
        else if(normalizedTime >= 0.34f && normalizedTime < 0.67f)
        {
            return this.yellow;
        }
        return this.red;
    }

    public void UpdateTimerSlider(float time, float maxTime)
    {
        float normalizedTime = time / maxTime;
        this.timerSlider.fillAmount = normalizedTime;
        Sprite timerSprite = this.GetTimerSprite(normalizedTime);
        this.timerSlider.sprite = timerSprite;
    }
}
