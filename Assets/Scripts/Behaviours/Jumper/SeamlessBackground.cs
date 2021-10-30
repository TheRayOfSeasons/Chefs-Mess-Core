using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StressManagement;

public enum BackgroundLayer
{
    SKY,
    MOUNTAIN,
    TREE,
    GROUND
}

public class BackgroundSet
{
    public Sprite skyLayer;
    public Sprite mountainLayer;
    public Sprite treeLayer;
    public Sprite groundLayer;

    public BackgroundSet(Sprite skyLayer, Sprite mountainLayer, Sprite treeLayer, Sprite groundLayer)
    {
        this.skyLayer = skyLayer;
        this.mountainLayer = mountainLayer;
        this.treeLayer = treeLayer;
        this.groundLayer = groundLayer;
    }
}

public class SeamlessBackground : MonoBehaviour
{
    [Header("Low Stress")]
    public Sprite lowStressSkyLayer;
    public Sprite lowStressMountainLayer;
    public Sprite lowStressTreeLayer;
    public Sprite lowStressGroundLayer;

    [Header("Mid Stress")]
    public Sprite midStressSkyLayer;
    public Sprite midStressMountainLayer;
    public Sprite midStressTreeLayer;
    public Sprite midStressGroundLayer;

    [Header("High Stress")]
    public Sprite highStressSkyLayer;
    public Sprite highStressMountainLayer;
    public Sprite highStressTreeLayer;
    public Sprite highStressGroundLayer;

    [Header("Configs")]
    public SpriteRenderer skyLayerRenderer;
    public SpriteRenderer mountainLayerRenderer;
    public SpriteRenderer treeLayerRenderer;
    public SpriteRenderer groundLayerRenderer;
    public bool animate = true;

    private Dictionary<StressLevel, BackgroundSet> backgroundMap;

    private StressLevel GetCurrentStress()
    {
        return GameManager.Instance.stress.GetStressLevel();
    }

    public void ToggleAnimate(bool toggle)
    {
        if(toggle)
        {
            this.skyLayerRenderer.gameObject.GetComponent<Animator>().speed = 1;
            this.mountainLayerRenderer.gameObject.GetComponent<Animator>().speed = 1;
            this.treeLayerRenderer.gameObject.GetComponent<Animator>().speed = 1;
            this.groundLayerRenderer.gameObject.GetComponent<Animator>().speed = 1;
        }
        else
        {
            this.skyLayerRenderer.gameObject.GetComponent<Animator>().speed = 0;
            this.mountainLayerRenderer.gameObject.GetComponent<Animator>().speed = 0;
            this.treeLayerRenderer.gameObject.GetComponent<Animator>().speed = 0;
            this.groundLayerRenderer.gameObject.GetComponent<Animator>().speed = 0;
        }
    }

    public void SetupBackground()
    {
        StressLevel stressLevel = this.GetCurrentStress();
        BackgroundSet backgroundSet = this.backgroundMap[stressLevel];
        this.skyLayerRenderer.sprite = backgroundSet.skyLayer;
        this.mountainLayerRenderer.sprite = backgroundSet.mountainLayer;
        this.treeLayerRenderer.sprite = backgroundSet.treeLayer;
        this.groundLayerRenderer.sprite = backgroundSet.groundLayer;
    }

    void Awake()
    {
        this.backgroundMap = new Dictionary<StressLevel, BackgroundSet>() {
            {
                StressLevel.LOW,
                new BackgroundSet(
                    this.lowStressSkyLayer,
                    this.lowStressMountainLayer,
                    this.lowStressTreeLayer,
                    this.lowStressGroundLayer
                )
            },
            {
                StressLevel.MID,
                new BackgroundSet(
                    this.midStressSkyLayer,
                    this.midStressMountainLayer,
                    this.midStressTreeLayer,
                    this.midStressGroundLayer
                )
            },
            {
                StressLevel.HIGH,
                new BackgroundSet(
                    this.highStressSkyLayer,
                    this.highStressMountainLayer,
                    this.highStressTreeLayer,
                    this.highStressGroundLayer
                )
            }
        };
    }

    void Start()
    {
        this.SetupBackground();
    }
}
