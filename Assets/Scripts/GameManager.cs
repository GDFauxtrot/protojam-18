﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public int score;

    public int scoreMultiplier;

    public GameObject player;

    public Camera mainCamera;

	// Use this for initialization
	void Start () {
        score = 0;
        scoreMultiplier = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
