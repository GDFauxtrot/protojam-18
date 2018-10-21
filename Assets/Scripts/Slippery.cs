using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slippery : MonoBehaviour {

    public GameObject soundPlayer;
    public AudioClip soundClip;
    GameObject player;
    bool collide;
    public int count;
    public int maxCount;
    public float speed;

	// Use this for initialization
	void Start () {
        speed = 220;
        maxCount = 120;
        collide = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (collide == true && player)
        {
            player.transform.Rotate(Vector3.forward * Time.deltaTime * speed);
            count++;
        }
        if (count == maxCount)
        {
            collide = false;
            count = 0;
        }
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        player = collider.transform.parent.gameObject;
        collide = true;

        SoundPlayer sound = Instantiate(soundPlayer, transform).GetComponent<SoundPlayer>();
        sound.PlaySound(soundClip, 1f);
    }
}
