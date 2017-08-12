using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private World world;

	// Use this for initialization
	void Start () {

        world = new World();
        world.InitializePopulation(4);

        int generationCounter = 0;
        int maxGenTries = 1000;

        while (!world.FoundWinner())
        {
            world.CreateNextGen(world.GetPopulation());
            generationCounter += 1;

            if (generationCounter > maxGenTries)
            {
                break;
            }
        }

        if (generationCounter <= maxGenTries)
        {
            Debug.Log("The winner is found at generation " + generationCounter + "!");
        }
        else
        {
            Debug.Log("Ran out of time, no winner created...");
        }
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
