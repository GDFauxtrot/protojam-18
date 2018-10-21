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
            score = 0;
        }


        scoreText.text = "Score: " + score.ToString();

        if (gameOverCoroutine != null) {
            StopCoroutine(gameOverCoroutine);
        }
        gameOverCoroutine = StartCoroutine(GameOverCoroutine(timeDelay));
    }

    public void Pressed_GameRestart() {
        SceneManager.LoadScene("MapBuilding");
    }

    public void Pressed_MainMenu() {
        SceneManager.LoadScene("MainMenuScene");
    }

    IEnumerator GameOverCoroutine(float secs) {
        yield return new WaitForSeconds(secs);
        gameOverParent.SetActive(true);
        animator.Play("GameOverAnim");

        gameOverCoroutine = null;
    }
}
