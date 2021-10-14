using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimerUtils;

public class Typer : MonoBehaviour
{
    private static Typer instance;
    public static Typer Instance
    {
        get { return instance; }
    }

    [SerializeField] private TyperGUI gui;
    public TyperGUI GUI
    {
        get { return this.gui; }
    }
    protected TyperWordSet currentWordSet;
    protected int currentCharacterIndex = 0;
    protected int currentRound = 1;

    private bool isOngoing = true;
    public bool IsOngoing { get { return this.isOngoing; } }

    private KeyCode currentKey;
    private bool isWon = false;
    private bool startCountdown = false;
    private bool roundPause = false;
    private TimedAction countdownHandler;
    private TimedAction timer;
    private TimedAction roundPauseTimer;
    [SerializeField] public float countDownBeforeStart = 3f;

    void Awake()
    {
        instance = this;
    }

    public void ToggleRoundPause(bool toggle)
    {
        this.roundPause = toggle;
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

    private void SetTimer()
    {
        this.timer = new TimedAction(
            maxTime: 1f, // set dynamically later
            action: () => {
                this.HandleLose();
            },
            triggerOnInitial: false
        );
    }

    private void SetRoundPause()
    {
        this.roundPauseTimer = new TimedAction(
            maxTime: 0.5f, // time before player can input word again after a round
            action: () => {
                this.gui.ToggleCelebratoryText(false);
                this.roundPause = false;
                this.roundPauseTimer.Reset();
                this.RenderWord();
            },
            triggerOnInitial: false
        );
    }

    protected TyperWordSet GetCurrentWordSet()
    {
        Constants.Difficulty difficulty = GameManager.Instance.GetCurrentDifficulty();
        return TyperMeta.words[difficulty];
    }

    protected string GetCurrentWord()
    {
        return this.GetCurrentWordSet().words[this.currentRound - 1];
    }

    public void Cleanup()
    {
        this.isOngoing = false;
    }

    public void Reset()
    {
        this.SetCountDown();
        this.SetTimer();
        this.SetRoundPause();
        this.isWon = false;
        this.roundPause = false;
        this.currentCharacterIndex = 0;
        this.currentRound = 1;
        this.currentWordSet = this.GetCurrentWordSet();
        this.currentKey = this.GetCurrentCharacterKeyCode();
        this.countdownHandler.Reset();
        this.timer.maxTime = this.currentWordSet.maxTimerCount;
        this.gui.UpdateTimerSlider(this.timer.maxTime, this.timer.maxTime);
        this.gui.DisposeCurrentWord();
        this.gui.ToggleCelebratoryText(false);
        this.gui.SetupVegtable(this.currentWordSet.vegtable);
        this.gui.UpdateIcon(this.currentWordSet.vegtable);
        this.gui.UpdateCounter((int)this.currentWordSet.rounds);
        this.timer.Reset();
        this.Cleanup();
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
        return this.GetCurrentWord()[this.currentCharacterIndex];
    }

    public KeyCode GetCurrentCharacterKeyCode()
    {
        char character = this.GetCurrentCharacter();
        KeyCode key = (KeyCode) System.Enum.Parse(typeof(KeyCode), character.ToString().ToUpper());
        return key;
    }

    void HandleLose()
    {
        this.Cleanup();
        GameManager.Instance.questDefinitions.FailMainObjective("typing-game", "type-in-all-words");
    }

    void HandleWin()
    {
        this.isWon = true;
        this.Cleanup();
        GameManager.Instance.questDefinitions.ClearMainObjective("typing-game", "type-in-all-words");
    }

    public void RenderWord()
    {
        this.gui.RenderWord(this.GetCurrentWord());
    }

    void HandleBeforeWordRerender()
    {
        this.roundPause = true;
    }

    void HandleRoundChange()
    {
        int roundDisplay = (int)this.currentWordSet.rounds - this.currentRound;
        this.currentRound++;
        this.gui.UpdateCounter(roundDisplay);
        this.gui.chefAnimationHandler.ThumbsUp();
        this.gui.knifeAnimationHandler.Toggle(true);
        this.gui.knifeAnimationHandler.Move();
        if(this.currentRound > this.currentWordSet.rounds)
        {
            this.HandleWin();
        }
    }

    void OnCharacterPress()
    {
        char previousCharacter = this.GetCurrentCharacter();
        int previousIndex = this.currentCharacterIndex;

        int lastIndex = this.GetCurrentWord().Length - 1;
        int oldIndex = this.currentCharacterIndex;
        int newIndex = this.currentCharacterIndex < lastIndex
            ? this.currentCharacterIndex + 1
            : 0;
        this.currentCharacterIndex = newIndex;

        this.AfterCharacterPress(previousCharacter, previousIndex);
        if(previousIndex == lastIndex)
        {
            this.HandleBeforeWordRerender();
        }
        if((previousIndex + 1) > lastIndex)
        {
            this.HandleRoundChange();
        }

        if(this.isWon)
            return;
        this.currentKey = this.GetCurrentCharacterKeyCode();
    }

    void AfterCharacterPress(char character, int characterIndex)
    {
        this.gui.Traverse(character, characterIndex);
        this.gui.chefAnimationHandler.Chop();
        this.gui.GetCurrentVegtableAnimationHandler(this.currentWordSet.vegtable).Cut(characterIndex + 1);
    }

    void Start()
    {
        this.Reset();
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
                // TODO: pass countdown to UI
                Debug.Log(currentTime);
            }
        }

        if(!this.isOngoing)
            return;

        if(this.timer != null)
        {
            this.timer.RunOnce(Time.deltaTime);
            this.gui.UpdateTimerSlider(this.timer.currentTime, this.timer.maxTime);
        }

        if(this.roundPause)
        {
        //     if(this.roundPauseTimer != null)
        //         this.roundPauseTimer.RunOnce(Time.deltaTime);
            return;
        }

        if(Input.GetKeyDown(this.currentKey))
        {
            this.OnCharacterPress();
        }
    }
}
