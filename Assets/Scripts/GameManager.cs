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
    }

    public int score;

    public int scoreMultiplier;

    public GameObject player;

    public Camera mainCamera;

    public Text scoreText;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        scoreText.text = "Score: " + score.ToString();
	}
}
