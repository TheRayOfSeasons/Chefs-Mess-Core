using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Typer : MonoBehaviour
{
    protected TyperWord currentWord;
    protected int currentCharacterIndex = 0;
    protected int currentRound = 1;

    private KeyCode currentKey;
    private bool isWon = false;

    protected TyperWord GetCurrentWord()
    {
        Constants.Difficulty difficulty = GameManager.Instance.GetCurrentDifficulty();
        return TyperMeta.words[difficulty];
    }

    public void Reset()
    {
        this.isWon = false;
        this.currentCharacterIndex = 0;
        this.currentRound = 1;
        this.currentWord = this.GetCurrentWord();
        this.currentKey = this.GetCurrentCharacterKeyCode();
    }

    public char GetCurrentCharacter()
    {
        return this.currentWord.word[this.currentCharacterIndex];
    }

    public KeyCode GetCurrentCharacterKeyCode()
    {
        char character = this.GetCurrentCharacter();
        KeyCode key = (KeyCode) System.Enum.Parse(typeof(KeyCode), character.ToString().ToUpper());
        return key;
    }

    void Start()
    {
        this.Reset();
    }

    void HandleWin()
    {
        this.isWon = true;
        Debug.Log("Typer has been won");
    }

    void OnCharacterPress()
    {
        char previousCharacter = this.GetCurrentCharacter();

        int lastIndex = this.currentWord.word.Length - 1;
        int oldIndex = this.currentCharacterIndex;
        int newIndex = this.currentCharacterIndex < lastIndex
            ? this.currentCharacterIndex + 1
            : 0;
        this.currentCharacterIndex = newIndex;
        this.currentKey = this.GetCurrentCharacterKeyCode();

        this.AfterCharacterPress(previousCharacter);
        if(newIndex == lastIndex)
        {
            this.currentRound++;
            if(this.currentRound == this.currentWord.rounds)
            {
                this.HandleWin();
            }
        }
    }

    void AfterCharacterPress(char character)
    {
        // add here your custom logic for applying extra effects
        // or animation after pressing the right key
    }

    void Update()
    {
        if(this.isWon)
            return;

        if(Input.GetKeyDown(this.currentKey))
        {
            this.OnCharacterPress();
        }
    }
}
