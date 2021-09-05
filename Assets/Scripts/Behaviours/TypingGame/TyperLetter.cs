using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(MonoTagAssigner))]
public class TyperLetter : MonoBehaviour
{
    public char letter = ' ';
    [SerializeField] private Text textComponent;
    [SerializeField] private GameObject SpriteComponent;
    private Constants.MonoTag monoTag = Constants.MonoTag.TYPER_LETTER;

    void Start()
    {
        this.GetComponent<MonoTagAssigner>().monoTag = this.monoTag;
        this.DefineLetter(this.letter);
    }

    public void DefineLetter(char character)
    {
        this.letter = character;
        this.textComponent.text = this.letter.ToString().ToUpper();
        this.textComponent.color = TyperMeta.untraversedColor;
    }

    public void MarkAsTraversed()
    {
        this.textComponent.color = TyperMeta.traversedColor;
    }
}
