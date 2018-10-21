using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartGame : MonoBehaviour {
    GameManager game;
    public Text gameOverText;
	// Use this for initialization
	void Start () {
        game = GameManager.instance;
	}

    // Update is called once per frame
    void Update () {
      //  game.startTime = 60;
        if (game.timeLeft == 0)
        {
            gameOverText.text = "Time's Up";
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            //Reloads current scene
            game.timeLeft = game.startTime;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    void Restart()
    {

    }
}
