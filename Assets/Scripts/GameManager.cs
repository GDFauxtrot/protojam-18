using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public bool playMusicAtStartup;

    public AudioClip gameLoopIntro, gameLoop;
    
    public float musicVolume;

    AudioSource music;

    bool musicPlayedGameLoopIntro;

    public int score;

    public bool timerIsRunning;
    public float startTime;
    public float scoreMultiplier, timeLeft;

    public GameObject player;

    public Camera mainCamera;

    public Text scoreText, timeText;

    public SpiralUI spiralGradient;

    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else {
            if (instance != this) {
                DestroyImmediate(gameObject);
            }
        }
        music = gameObject.AddComponent<AudioSource>();

        // Load up game loop intro
        if (playMusicAtStartup)
            Music_PlayIntro();

        timeLeft = startTime;
    }

    public void Music_PlayIntro() {
        music.Stop();
        music.clip = gameLoopIntro;
        music.loop = false;
        music.Play();
        musicPlayedGameLoopIntro = false;
    }

    public void Music_PlayLoop() {
        music.Stop();
        music.clip = gameLoop;
        music.loop = true;
        music.Play();
    }

    void Update () {
        if (scoreText)
            scoreText.text = "Score: " + score.ToString();

        if (!music.isPlaying && music.clip == gameLoopIntro && !musicPlayedGameLoopIntro) {
            Music_PlayLoop();
            musicPlayedGameLoopIntro = true;
        }
        music.volume = musicVolume;

        if (timeText) {
            if (timerIsRunning) {
                timeLeft = Mathf.Clamp(timeLeft - Time.deltaTime, 0f, startTime);
                timeText.text = "Timer: " + timeLeft.ToString("00.00");
            }
        }
    }
}