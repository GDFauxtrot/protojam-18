using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour
{

    GameManager gameManager;

    Rigidbody2D rigidbody2D;

    public float speed;

    public GameObject[] locationPins;

    int currentPin = 0;

    public float pinRadius = 5;

    Vector2 acceleration = Vector2.zero;
    
    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        gameManager = GameManager.instance;
    }

    void Update()
    {
        if (locationPins.Length > 0)
        {
            //Change the target
            AimTowardsPlayer();

            //Change direction when within range of location pin (say radius 3 or so)
            if (Mathf.Abs(locationPins[currentPin % locationPins.Length].transform.position.x - this.transform.position.x) <= pinRadius &&
                Mathf.Abs(locationPins[currentPin % locationPins.Length].transform.position.y - this.transform.position.y) <= pinRadius)
            {
                currentPin++;
            }

            //Update velocity of car
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x + acceleration.x, rigidbody2D.velocity.y + acceleration.y);

            if (rigidbody2D.velocity.magnitude > speed)
            {
                rigidbody2D.velocity = rigidbody2D.velocity.normalized * speed;
            }

            //Rotation
            this.transform.LookAt(locationPins[currentPin % locationPins.Length].transform.position, Vector3.up);
            this.transform.Rotate(0, 90, 90);
        }

    }

    void AimTowardsPlayer()
    {
        float horizontalDifference = locationPins[currentPin % locationPins.Length].transform.position.x - this.transform.position.x;
        float verticalDifference = locationPins[currentPin % locationPins.Length].transform.position.y - this.transform.position.y;

        acceleration = new Vector2(horizontalDifference, verticalDifference).normalized;
    }
}
