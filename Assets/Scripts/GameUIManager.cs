using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour {

    public Text scoreText;
    public Text timeUpText;
    public GameObject gameOverParent;

    Coroutine gameOverCoroutine;
    Animator animator;

    void Awake() {
        animator = GetComponent<Animator>();
    }
    public void GameOver(int score, float timeDelay, bool timeUp) {
        if (timeUp)
        {
            timeUpText.gameObject.SetActive(true);
        }
        scoreText.text = "Score: " + score.ToString();
        if (gameOverCoroutine != null) {
            StopCoroutine(gameOverCoroutine);
        }
        gameOverCoroutine = StartCoroutine(GameOverCoroutine(timeDelay));
    }

    public void Pressed_GameRestart() {
        GameManager.instance.playMusicAtStartup = true;
        GameManager.instance.musicPlayedGameLoopIntro = false;
        GameManager.instance.StopMusic();
        
        SceneManager.LoadScene("GameScene");
    }

    public void Pressed_MainMenu() {
        GameManager.instance.playMusicAtStartup = false;
        GameManager.instance.musicPlayedGameLoopIntro = false;
        GameManager.instance.StopMusic();
        SceneManager.LoadScene("MainMenuScene");
    }

    IEnumerator GameOverCoroutine(float secs) {
        yield return new WaitForSeconds(secs);
        gameOverParent.SetActive(true);
        animator.Play("GameOverAnim");

        gameOverCoroutine = null;
    }
}
