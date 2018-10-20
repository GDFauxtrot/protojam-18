using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explodeable : MonoBehaviour {

    GameManager gameManager;
    Rigidbody2D rigidbody2D;

    public GameObject player;

    void Awake()
    {
        
    }

    void Start()
    {
        gameManager = GameManager.instance;
    }

    void Update()
    {

    }
    
    //Called when the car enters the trigger
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.collider.name);
        if (collision.collider.name == "Good Collider")
        {
            tellGM();
            gameManager.GivePoints((int)(100 * gameManager.scoreMultiplier));
            explode();
            disapear(gameObject);
        } else if (collision.collider.name == "Bad Collider")
        {
            explode();
            disapear(collision.gameObject);
        }

    }

    //Removes the explodable from the map
    public void disapear(GameObject obj)
    {
        Destroy(obj);
    }

    //Plays a animation of the explosion
    public void explode()
    {
        
    }

    //Tells the GameManager that this Object has Exploded
	public void tellGM()
    {

    }
}
