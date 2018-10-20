using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explodeable : MonoBehaviour {

    GameManager gameManager;
    Rigidbody2D rigidbody2D;

    void Awake()
    {
        gameManager = GameManager.instance;
    }

    void Update()
    {

    }
    
    //Called when the car enters the trigger
    private void OnCollisionEnter2D(Collision2D collision)
    {
        tellGM();
        gameManager.GivePoints((int)(100 * gameManager.scoreMultiplier));
        explode();
        disapear();

    }

    //Removes the explodable from the map
    public void disapear()
    {
        Destroy(gameObject);
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
