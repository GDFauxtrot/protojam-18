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
    private void OnTriggerEnter(Collider other)
    {
        tellGM();
        givePoints((int)(100 * gameManager.scoreMultiplier));
        explode();
        disapear();

    }

    //Tells the GameManager to increase the multiplier by .1
    public float multiplierUp()
    {
        gameManager.scoreMultiplier += .1f;
        return .1f;
    }
    
    //tells the GameManneger to increase the multiple by mul
    public float multiplierUp(float mul)
    {
        gameManager.scoreMultiplier += mul;
        return mul;
    }

    public int givePoints()
    {
        gameManager.score += 100;

        return 100;
    }


    //Tells the GameManager to give the player a given number of points equal to pnt
    public int givePoints(int pnt)
    {
        gameManager.score += pnt;

        return pnt;
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
