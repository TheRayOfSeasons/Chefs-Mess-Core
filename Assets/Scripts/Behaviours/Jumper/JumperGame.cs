using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimerUtils;

public class JumperGame : MonoBehaviour
{
    private static JumperGame instance;
    public static JumperGame Instance
    {
        get { return instance; }
    }

    public JumperGUI gui;
    public SeamlessBackground background;

    [SerializeField] public float speedIncrementInterval = 10f;
    [SerializeField] public float countDownBeforeStart = 3f;
    [SerializeField] public float secondsToWin = 60f;
    [SerializeField] public Runner runner;

    private bool isOngoing = true;
    public bool IsOngoing { get { return this.isOngoing; } }

    private Dictionary<Constants.Difficulty, float> speedMap = new Dictionary<Constants.Difficulty, float>() {
        {Constants.Difficulty.EASY, 0.2f},
        {Constants.Difficulty.MEDIUM, 0.2f},
        {Constants.Difficulty.HARD, 0.25f},
    };
    private float currentSpeed = 0.2f; // just a base
    private TimedAction speedIncrementHandler;
    private TimedAction countdownHandler;
    private bool startCountdown = false;
    private TimedAction winTimerHandler;
    private string spawnerParentName = "Spawners";

    void Awake()
    {
        instance = this;
    }

    private void SetCountDown()
    {
        this.countdownHandler = new TimedAction(
            maxTime: this.countDownBeforeStart,
            action: () => {
                UIManager.Instance.countdownSignal.Toggle(false);
                this.startCountdown = false;
                this.StartGame();
            },
            triggerOnInitial: false
        );
    }

    private void SetSpeedIncrementHandler()
    {
        this.speedIncrementHandler = new TimedAction(
            maxTime: this.speedIncrementInterval,
            action: () => {
                this.currentSpeed += this.GetSpeedIncrement();
                Debug.Log(this.currentSpeed);
            }
        );
    }

    private void SetWinTimer()
    {
        this.winTimerHandler = new TimedAction(
            maxTime: this.secondsToWin,
            action: () => {
                this.HandleWin();
            }
        );
    }

    public float GetCurrentSpeed()
    {
        return this.currentSpeed;
    }

    public void TriggerGameStart()
    {
        UIManager.Instance.countdownSignal.Toggle(true);
        this.background.ToggleAnimate(true);
        this.startCountdown = true;
    }

    public void StartGame()
    {
        this.isOngoing = true;
    }

    private float GetSpeedIncrement()
    {
        Constants.Difficulty currentDifficulty = GameManager.Instance.GetCurrentDifficulty();
        return this.speedMap[currentDifficulty];
    }

    public void ClearObstacles()
    {
        Transform spawnerParent = this.transform.Find(this.spawnerParentName);
        foreach(Transform child in spawnerParent)
        {
            foreach(Transform grandChild in child)
            {
                Destroy(grandChild.gameObject);
            }
        }
    }

    public void HandleLose()
    {
        this.background.ToggleAnimate(false);
        this.isOngoing = false;
        Time.timeScale = 0f;
        GameManager.Instance.questDefinitions.FailMainObjective("jumper", "arrive-at-finish-line");
    }

    public void HandleWin()
    {
        this.background.ToggleAnimate(false);
        this.isOngoing = false;
        Time.timeScale = 0f;
        GameManager.Instance.questDefinitions.ClearMainObjective("jumper", "arrive-at-finish-line");
    }

    void Start()
    {
        this.SetSpeedIncrementHandler();
        this.SetCountDown();
        this.SetWinTimer();
        this.Reset();
    }

    public void Cleanup()
    {
        this.isOngoing = false;
        Time.timeScale = 1.0f;
    }

    public void Reset()
    {
        this.currentSpeed = this.GetSpeedIncrement();
        this.speedIncrementHandler.Reset();
        this.countdownHandler.Reset();
        this.winTimerHandler.Reset();
        this.runner.Reset();
        this.background.SetupBackground();
        this.ClearObstacles();
        this.Cleanup();
    }

    void Update()
    {
        if(this.startCountdown)
        {
            if(this.countdownHandler != null)
            {
                this.countdownHandler.RunOnce(Time.deltaTime);
                float currentTime = Mathf.Ceil(this.countdownHandler.currentTime);
                UIManager.Instance.countdownSignal.UpdateValue((int)currentTime);
            }
        }

        if(!this.isOngoing)
            return;

        if(this.speedIncrementHandler != null)
        {
            this.speedIncrementHandler.Run(Time.deltaTime);
        }

        if(this.winTimerHandler != null)
        {
            this.winTimerHandler.RunOnce(Time.deltaTime);
        }

        Time.timeScale = this.currentSpeed + 1f;
    }
}
