﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    Color normalColor = new Color(0.054f, 0.636f, 0.877f, 1.000f);
    Color redColor = new Color(0.877f, 0.096f, 0.054f, 1.000f);

    public static GameManager instance;

    public bool playMusicAtStartup;

    public AudioClip gameLoopIntro, gameLoop;
    
    public float musicVolume;

    AudioSource music;

    public bool musicPlayedGameLoopIntro;

    public int score;

    public float meterRate = .75f;
    public float meterPercent = 0;

    public bool timerIsRunning;
    public float startTime;
    public float scoreMultiplier, timeLeft;

    public GameObject player;

    public Camera mainCamera;

    public Text scoreText, timeText;

    public SpiralUI spiralGradient;

    Coroutine frameFreezeCoroutine;

    public GameObject[] playerStartLocations;

    void Awake() {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else {
            instance = this;
            // DontDestroyOnLoad(this.gameObject);
        }
        // if (instance == null) {
        //     instance = this;
        //     DontDestroyOnLoad(this.gameObject);
        // } else {
        //     if (instance != this) {
        //         DestroyImmediate(this.gameObject);
        //     }
        // }
        if (player == null)
            player = GameObject.Find("Player");

        music = gameObject.AddComponent<AudioSource>();

        // Load up game loop intro
        if (playMusicAtStartup && SceneManager.GetActiveScene().name == "GameScene")
            Music_PlayIntro();

        score = 0;
        timeLeft = startTime;
    }

    void Start() {
        //Set player's location
        player.transform.position = playerStartLocations[Random.Range(0, playerStartLocations.Length)].transform.position;
    }

    public void Music_PlayIntro() {
        Debug.Log("intro");
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
        else {
            GameObject st = GameObject.FindWithTag("score");
            if (st)
            {
                score = 0;
                scoreText = st.GetComponent<Text>();
                scoreText.text = "Score: " + score.ToString();
            }
        }
            

        if (!music.isPlaying && music.clip == gameLoopIntro && !musicPlayedGameLoopIntro) {
            Music_PlayLoop();
            musicPlayedGameLoopIntro = true;
        }
        music.volume = musicVolume;

        if (timeText) {
            if (timerIsRunning) {
                timeLeft = Mathf.Clamp(timeLeft - Time.deltaTime, 0f, startTime);
                timeText.text = "Timer: " + timeLeft.ToString("00.00");

                if (timeLeft == 0f) {
                    timerIsRunning = false;
                    GameObject.Find("Game UI Layer").GetComponent<GameUIManager>().GameOver(score, 1.5f, true);
                    player.GetComponent<PlayerController>().canControl = false;
                }

                //Change timer's color to red when time is low
                if (timeLeft < 10f)
                {
                    timeText.color = redColor;
                } else
                {
                    timeText.color = normalColor;
                }
            }

        }
        else {
            GameObject tt = GameObject.FindWithTag("time");
            if (tt)
            {
                timeText = tt.GetComponent<Text>();
                timeLeft = startTime;
                timerIsRunning = true;
            }
        }

        //Do not let the meter bypass 100%
        meterPercent = Mathf.Clamp01(meterPercent);

        if (meterPercent == 1.0f) {
            scoreMultiplier = 2;
        }
    }

    // Freeze the game for one frame. Makes impacts more satisfying.
    public void FreezeFrame() {
        if (frameFreezeCoroutine != null) {
            StopCoroutine(frameFreezeCoroutine);
        }
        frameFreezeCoroutine = StartCoroutine(FreezeFrameCoroutine());
    }

    IEnumerator FreezeFrameCoroutine() {
        Time.timeScale = 0.0f;
        yield return new WaitForSecondsRealtime(Time.deltaTime * 2);
        Time.timeScale = 1.0f;

        frameFreezeCoroutine = null;
    }

    public void HitPlayer(PlayerController player, Explodeable source) {
        timerIsRunning = false;
        GameObject.Find("FullscreenFlash").GetComponent<FullscreenFlasher>().FlashWhiteBlack();
        source.Explode(player.gameObject, false);
        SoundPlayer sound1 = Instantiate(source.soundPlayer, player.transform).GetComponent<SoundPlayer>();
        sound1.PlaySound(player.deathSound1, 1f);
        SoundPlayer sound2 = Instantiate(source.soundPlayer, player.transform).GetComponent<SoundPlayer>();
        sound1.PlaySound(player.deathSound2, 1f);
        Destroy(player.gameObject);
        GameObject.Find("Game UI Layer").GetComponent<GameUIManager>().GameOver(score, 1.5f, false);
        


    }

    public void StopMusic() {
        music.Stop();
    }
}