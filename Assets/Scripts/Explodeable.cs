﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explodeable : MonoBehaviour {



    //Tells the GameManager to increase the multiplier by .1
    double multiplierUp()
    {

        return .1;
    }
    
    //tells the GameManneger to increase the multiple by mul
    double multiplierUp(double mul)
    {

        return mul;
    }

    //Tells the GameManager to give the player a set number of points equal to 100
    int givePoints()
    {

        return 100;
    }

    //Tells the GameManager to give the player a given number of points equal to pnt
    int givePoints(int pnt)
    {
        

        return pnt;
    }

    //Removes the explodable from the map
    void disapear()
    {

    }

    //Plays a animation of the explosion
    void explode()
    {

    }

    //Tells the GameManager that this Object has Exploded
	void tellGM()
    {

    }
}
