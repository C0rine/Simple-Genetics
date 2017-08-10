using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private World world;

	// Use this for initialization
	void Start () {

        world = new World();
        world.InitializePopulation(4);

        for(int i=0; i < world.GetPopulationSize(); i++)
        {
            world.GetPopulation()[i].Print();
        }

        world.CreateNextGen(world.GetPopulation());

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
