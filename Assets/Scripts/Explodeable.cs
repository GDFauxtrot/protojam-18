using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explodeable : MonoBehaviour {

    GameManager gameManager;
    Rigidbody2D rigidbody2D;

    GameObject player;

    public GameObject explosion;

    void Start() {
        gameManager = GameManager.instance;
        player = GameObject.Find("Player");
    }

    //Called when the car enters the trigger
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.collider.name);
        if (collision.collider.name == "Good Collider")
        {
            // Tell Game Manager item has been exploded
            gameManager.score += (int) (100 * gameManager.scoreMultiplier);
            Explode(gameObject);
            Disappear(gameObject);
        } else if (collision.collider.name == "Bad Collider")
        {
            Explode(collision.gameObject);
            Disappear(collision.gameObject);
        }

    }

    //Removes the explodable from the map
    public void Disappear(GameObject obj)
    {
        Destroy(obj);
    }

    //Plays a animation of the explosion
    public void Explode(GameObject obj)
    {
        Instantiate(explosion, obj.transform.position, obj.transform.rotation);
    }
}
