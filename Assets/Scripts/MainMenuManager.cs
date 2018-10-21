using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

    public GameObject creditsParent;

    GameManager gm;

    void Awake() {
        gm = GameManager.instance;

        creditsParent.SetActive(false);
    }

    public void Pressed_Start() {
        // Load up controls + flavor text before this?
        SceneManager.LoadScene("GameScene");
    }

    public void Pressed_Credits() {
        creditsParent.SetActive(true);
    }

    public void Pressed_Quit() {
        Application.Quit();
    }

    public void Pressed_CreditsBack() {
        creditsParent.SetActive(false);
    }
}
