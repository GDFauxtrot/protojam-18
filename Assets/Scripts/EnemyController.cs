using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour
{

    GameManager gameManager;

    Rigidbody2D rigidbody2D;

    public float speed;

    void Awake()
    {
        gameManager = GameManager.instance;
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        rigidbody2D.AddForce(new Vector2(speed, 0));    
    }

    void Update()
    {

    }
}
