using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TyperGUI : MonoBehaviour
{
    [SerializeField] private Image timerSlider;

    [Header("Fill Sprites")]
    [SerializeField] private Sprite green;
    [SerializeField] private Sprite yellow;
    [SerializeField] private Sprite red;

    [Header("Chef Animator")]
    public ChefAnimationHandler chefAnimationHandler;

    [Header("Vegtable Animators")]
    [SerializeField] private VegtableAnimationHandler brocolliAnimationHandler;
    [SerializeField] private VegtableAnimationHandler carrotAnimationHandler;
    [SerializeField] private VegtableAnimationHandler tomatoAnimationHandler;

    private Dictionary<VegtableType, VegtableAnimationHandler> vegtableSpriteRouter;

    [Header("Knife Animators")]
    public KnifeAnimationHandler knifeAnimationHandler;

    [Header("Counter")]
    [SerializeField] private Text counterComponent;
    [SerializeField] private Image vegtableIconComponent;
    [SerializeField] private Sprite brocolliIcon;
    [SerializeField] private Sprite carrotIcon;
    [SerializeField] private Sprite tomatoIcon;

    private Dictionary<VegtableType, Sprite> vegtableIconRouter;

    [Header("Tutorial")]
    [SerializeField] private GameObject tutorialUI;
    public GameObject TutorialUI
    {
        get { return this.tutorialUI; }
    }

    [Header("Game")]
    [SerializeField] private GameObject letterObjectPrefab;
    [SerializeField] private GameObject celebratoryTextComponent;
    [SerializeField] private Vector3 wordCenter = new Vector3();
    [SerializeField] private float letterSpacing = 1f;
    private Dictionary<int, TyperLetter> letterComponents = new Dictionary<int, TyperLetter>();
    private List<GameObject> letterObjects = new List<GameObject>();

    void Awake()
    {
        this.vegtableSpriteRouter = new Dictionary<VegtableType, VegtableAnimationHandler>() {
            {VegtableType.BROCOLLI, this.brocolliAnimationHandler},
            {VegtableType.CARROT, this.carrotAnimationHandler},
            {VegtableType.TOMATO, this.tomatoAnimationHandler}
        };
        this.vegtableIconRouter = new Dictionary<VegtableType, Sprite>() {
            {VegtableType.BROCOLLI, this.brocolliIcon},
            {VegtableType.CARROT, this.carrotIcon},
            {VegtableType.TOMATO, this.tomatoIcon}
        };
    }

    public void UpdateIcon(VegtableType vegtableType)
    {
        this.vegtableIconComponent.sprite = this.vegtableIconRouter[vegtableType];
    }

    public void UpdateCounter(int count)
    {
        this.counterComponent.text = count.ToString();
    }

    public void SetupVegtable(VegtableType target)
    {
        foreach(KeyValuePair<VegtableType, VegtableAnimationHandler> vegtable in this.vegtableSpriteRouter)
        {
            vegtable.Value.Toggle(vegtable.Key == target);
        }
    }

    public VegtableAnimationHandler GetCurrentVegtableAnimationHandler(VegtableType vegtableType)
    {
        return this.vegtableSpriteRouter[vegtableType];
    }

    void Start()
    {
        if(!this.letterObjectPrefab.GetComponent<TyperLetter>())
            throw new MissingComponentException("letterObjectPrefab must have TyperLetter");

        this.celebratoryTextComponent.SetActive(false);
    }

    private List<Vector3> GetLetterPositions(int count)
    {
        List<Vector3> positions = new List<Vector3>();
        float span = (count - 1) * letterSpacing;
        float halfSpan = span * 0.5f;
        for(int i = 0; i < count; i++)
        {
            float xPos = (i * letterSpacing) - halfSpan;
            positions.Add(new Vector3(
                xPos + this.wordCenter.x,
                this.wordCenter.y,
                this.wordCenter.z
            ));
        }
        return positions;
    }

    public void RenderWord(string word)
    {
        this.DisposeCurrentWord();
        List<Vector3> positions = this.GetLetterPositions(word.Length);
        for(int i = 0; i < word.Length; i++)
        {
            char letter = word[i];
            string letterString = letter.ToString();
            GameObject letterObject = Instantiate(this.letterObjectPrefab);
            letterObject.transform.SetParent(this.transform);
            RectTransform letterTransform = letterObject.GetComponent<RectTransform>();
            letterTransform.anchoredPosition3D = positions[i];
            letterObject.name = $"TyperLetter {letter} - {i}";
            letterTransform.localScale = new Vector3(1, 1, 1);
            TyperLetter letterComponent = letterObject.GetComponent<TyperLetter>();
            letterComponent.DefineLetter(letter);
            this.letterComponents.Add(i, letterComponent);
            this.letterObjects.Add(letterObject);
        }
    }

    public void DisposeCurrentWord()
    {
        foreach(GameObject letterObject in this.letterObjects)
        {
            Destroy(letterObject);
        }
        this.letterComponents.Clear();
        this.letterObjects.Clear();
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

    public void ToggleCelebratoryText(bool toggle)
    {
        this.celebratoryTextComponent.SetActive(toggle);
    }

    public void Traverse(char letter, int index)
    {
        this.letterComponents[index].MarkAsTraversed();
    }
}
