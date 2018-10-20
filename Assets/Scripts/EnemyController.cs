using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour
{

    GameManager gameManager;

    Rigidbody2D rigidbody2D;

    GameObject player;

    Rigidbody2D playerBody;

    public float speed;

    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        gameManager = GameManager.instance;

        player = gameManager.player;

        playerBody = player.GetComponent<Rigidbody2D>();

        rigidbody2D.AddForce(new Vector2(speed, 0));    
    }

    void Update()
    {
        //Every two seconds change the direction of the enemy
        if (Time.fixedTime % 2 == 0)
        {
            AimTowardsPlayer();
        }
    }

    void AimTowardsPlayer()
    {
        float horizontalDifference = rigidbody2D.position.x - playerBody.position.x;
        float verticalDifference = rigidbody2D.position.x - playerBody.position.x;

        rigidbody2D.AddForce(new Vector2(horizontalDifference, verticalDifference));
    }
}
