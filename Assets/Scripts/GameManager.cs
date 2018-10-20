using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            if (instance != this) {
                DestroyImmediate(gameObject);
            }
        }
        timeLeft = 5.0f;
    }


    public int score;

    public float scoreMultiplier, timeLeft;

    public GameObject player;

    public Camera mainCamera;

    public Text scoreText, timeText;


    // Use this for initialization
    void Start () {
    }

    // Update is called once per frame
    void Update () {
        scoreText.text = "Score: " + score.ToString();
        timeText.text = "Timer: " + Timer().ToString("00.00");
        
    }

    //Tells the GameManager to increase the multiplier by .1
    public float MultiplierUp()
    {
        scoreMultiplier += .1f;
        return .1f;
    }
    public float Timer()
    {
        if (timeLeft > 00.00f)
        {
            timeLeft -= Time.deltaTime;
        }
        else
        {
            timeLeft = 0.0f;
        }
        return timeLeft;
    }
    //tells the GameManneger to increase the multiple by mul
    public float MultiplierUp(float mul)
    {
        scoreMultiplier += mul;
        return mul;
    }

    public int GivePoints()
    {
        score += 100;
        return 100;
    }

    public float AddTime()
    {
        timeLeft += 5.0f;
        return 5.0f;
    }
    //Tells the GameManager to give the player a given number of points equal to pnt
    public int GivePoints(int pnt)
    {
        score += pnt;

        return pnt;
    }
}
