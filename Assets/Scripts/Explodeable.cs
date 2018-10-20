using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explodeable : MonoBehaviour {

    GameManager gameManager;
    Rigidbody2D rigidbody2D;

    GameObject player;

    public GameObject explosion;

    void Awake()
    {
        
    }

    void Start()
    {
        gameManager = GameManager.instance;

        player = GameObject.Find("Player");
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
            explode(gameObject);
            disapear(gameObject);
        } else if (collision.collider.name == "Bad Collider")
        {
            explode(collision.gameObject);
            disapear(collision.gameObject);
        }

    }

    //Removes the explodable from the map
    public void disapear(GameObject obj)
    {
        Destroy(obj);
    }

    //Plays a animation of the explosion
    public void explode(GameObject obj)
    {
        Instantiate(explosion, obj.transform.position, obj.transform.rotation);
    }

    //Tells the GameManager that this Object has Exploded
	public void tellGM()
    {

    }
}
