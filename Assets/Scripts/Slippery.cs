using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slippery : MonoBehaviour {

    GameObject player;
    bool collide;
    public int count;
    public int maxCount;
    public int speed;

	// Use this for initialization
	void Start () {
        speed = 220;
        maxCount = 120;
	}
	
	// Update is called once per frame
	void Update () {
        if (collide == true)
        {
            player.transform.Rotate(0, 0, speed * Time.deltaTime);
            print("TRUE");
            count++;
        }
        if (count == maxCount)
        {
            collide = false;
        }
            
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        print("WORKING");
        player = collision.gameObject;
        //player.transform.Rotate(0, 0, 90);
        collide = true;
        //gameObject.SetActiveRecursively(false);
    }
}
