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

    [SerializeField] private GameObject letterObjectPrefab;
    [SerializeField] private GameObject celebratoryTextComponent;
    [SerializeField] private Vector3 wordCenter = new Vector3();
    [SerializeField] private float letterSpacing = 1f;
    private Dictionary<int, TyperLetter> letterComponents = new Dictionary<int, TyperLetter>();
    private List<GameObject> letterObjects = new List<GameObject>();

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

        // add here your custom logic for applying extra effects
        // or animation after pressing the right key
    }
}
