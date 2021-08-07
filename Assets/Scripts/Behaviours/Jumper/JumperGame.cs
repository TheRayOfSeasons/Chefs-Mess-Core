﻿using System.Collections;
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

    [SerializeField] public float speedIncrementInterval = 10f;
    [SerializeField] public float countDownBeforeStart = 3f;
    [SerializeField] public float secondsToWin = 60f;

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

    void Awake()
    {
        instance = this;
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
    }

    public void HandleLose()
    {
        this.isOngoing = false;
        Time.timeScale = 0f;
        Debug.Log("Lose");
    }

    public void HandleWin()
    {
        this.isOngoing = false;
        Time.timeScale = 0f;
        Debug.Log("Win");
    }

    void Start()
    {
        this.SetSpeedIncrementHandler();
        this.SetCountDown();
        this.SetWinTimer();
        this.Reset();

        // remove later
        this.TriggerGameStart();
    }

    public void Reset()
    {
        this.currentSpeed = this.GetSpeedIncrement();
        this.speedIncrementHandler.Reset();
        this.countdownHandler.Reset();
        this.winTimerHandler.Reset();
        this.isOngoing = false;
        Time.timeScale = 1.0f;
    }

    void Update()
    {
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