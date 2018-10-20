using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {

    GameManager gameManager;

    Rigidbody2D rigidbody2D;

    void Awake() {
        gameManager = GameManager.instance;
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update () {
        
    }
}
