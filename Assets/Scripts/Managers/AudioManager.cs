using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BGM
{
    HUB,
    STUDY,
    KITCHEN,
    BEDROOM
}

public enum TaskAudio
{
    ACCEPTED,
    SUCCESS,
    FAILED
}

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance
    {
        get { return instance; }
    }

    [Header("BGM")]
    public AudioSource hubBGM;
    public AudioSource studyBGM;
    public AudioSource kitchenBGM;
    public AudioSource bedroomBGM;

    [Header("Task")]
    public AudioSource taskAccepted;
    public AudioSource taskSuccess;
    public AudioSource taskFailed;

    private Dictionary<TaskAudio, AudioSource> taskAudioMap;

    [Header("Game Over")]
    public AudioSource victory;

    private BGM currentBGM = BGM.HUB;
    private Dictionary<BGM, AudioSource> bgmMap;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        this.bgmMap = new Dictionary<BGM, AudioSource>() {
            {BGM.HUB, this.hubBGM},
            {BGM.STUDY, this.studyBGM},
            {BGM.KITCHEN, this.kitchenBGM},
            {BGM.BEDROOM, this.bedroomBGM}
        };
        this.ChangeBackgroundMusic(BGM.HUB);

        this.taskAudioMap = new Dictionary<TaskAudio, AudioSource>() {
            {TaskAudio.ACCEPTED, this.taskAccepted},
            {TaskAudio.SUCCESS, this.taskSuccess},
            {TaskAudio.FAILED, this.taskFailed}
        };
    }

    public void ChangeBackgroundMusic(BGM bgm)
    {
        this.bgmMap[this.currentBGM].Stop();
        this.bgmMap[bgm].Play();
        this.currentBGM = bgm;
    }

    public void PlayTaskAudio(TaskAudio taskAudio)
    {
        this.taskAudioMap[taskAudio].Play();
    }

    public void PlayVictoryMusic()
    {
        this.bgmMap[this.currentBGM].Stop();
        this.victory.Play();
    }
}
