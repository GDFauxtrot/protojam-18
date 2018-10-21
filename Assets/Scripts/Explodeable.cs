using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explodeable : MonoBehaviour {

    GameManager gameManager;
    Rigidbody2D rigidbody2D;

    PlayerController player;

    public GameObject explosion;

    void Start() {
        gameManager = GameManager.instance;
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    //Called when the car enters the trigger
    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "PlayerCollider_Side")
        {
            gameManager.score += (int) (100 * gameManager.scoreMultiplier);
            Explode(gameObject);
        } else if (collider.tag == "PlayerCollider_Hurt")
        {
            Explode(gameObject);
        }
    }

    //Plays a animation of the explosion
    public void Explode(GameObject obj)
    {
        Instantiate(explosion, obj.transform.position, obj.transform.rotation);
        GameManager.instance.FreezeFrame();
        player.AddScreenShake();
        Destroy(obj);
    }
}
