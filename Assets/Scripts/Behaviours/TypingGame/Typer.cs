using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimerUtils;

public class Typer : MonoBehaviour
{
    [SerializeField] private TyperGUI gui;
    protected TyperWord currentWord;
    protected int currentCharacterIndex = 0;
    protected int currentRound = 1;

    private bool isOngoing = true;
    public bool IsOngoing { get { return this.isOngoing; } }

    private KeyCode currentKey;
    private bool isWon = false;
    private TimedAction countdownHandler;
    [SerializeField] public float countDownBeforeStart = 3f;
    private bool startCountdown = false;

    protected TyperWord GetCurrentWord()
    {
        Constants.Difficulty difficulty = GameManager.Instance.GetCurrentDifficulty();
        return TyperMeta.words[difficulty];
    }

    public void Cleanup()
    {
        this.isOngoing = false;
    }

    public void Reset()
    {
        this.isWon = false;
        this.currentCharacterIndex = 0;
        this.currentRound = 1;
        this.currentWord = this.GetCurrentWord();
        this.currentKey = this.GetCurrentCharacterKeyCode();
        this.countdownHandler.Reset();
        this.Cleanup();
    }

    private void SetCountDown()
    {
        this.countdownHandler = new TimedAction(
            maxTime: this.countDownBeforeStart,
            action: () => {
                this.startCountdown = false;
                this.StartGame();
            },
            triggerOnInitial: false
        );
    }

    public void TriggerGameStart()
    {
        this.startCountdown = true;
    }

    public void StartGame()
    {
        this.isOngoing = true;
        this.RenderWord();
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
        this.SetCountDown();
        this.Reset();

        // temporary
        this.TriggerGameStart();
    }

    void HandleWin()
    {
        this.isWon = true;
        Debug.Log("Typer has been won");
    }

    void RenderWord()
    {
        this.gui.RenderWord(this.currentWord.word);
    }

    void OnCharacterPress()
    {
        char previousCharacter = this.GetCurrentCharacter();
        int previousIndex = this.currentCharacterIndex;

        int lastIndex = this.currentWord.word.Length - 1;
        int oldIndex = this.currentCharacterIndex;
        int newIndex = this.currentCharacterIndex < lastIndex
            ? this.currentCharacterIndex + 1
            : 0;
        this.currentCharacterIndex = newIndex;
        this.currentKey = this.GetCurrentCharacterKeyCode();

        this.AfterCharacterPress(previousCharacter, previousIndex);
        if(previousIndex == lastIndex)
        {
            this.RenderWord();
        }
        if((previousIndex + 1) > lastIndex)
        {
            this.currentRound++;
            if(this.currentRound > this.currentWord.rounds)
            {
                this.HandleWin();
            }
        }
    }

    void AfterCharacterPress(char character, int characterIndex)
    {
        this.gui.Traverse(character, characterIndex);
    }

    void Update()
    {
        if(this.isWon)
            return;

        if(this.startCountdown)
        {
            if(this.countdownHandler != null)
            {
                this.countdownHandler.RunOnce(Time.deltaTime);
                float currentTime = Mathf.Ceil(this.countdownHandler.currentTime);
                Debug.Log(currentTime);
            }
        }

        if(!this.isOngoing)
            return;

        if(Input.GetKeyDown(this.currentKey))
        {
            this.OnCharacterPress();
        }
    }
}
