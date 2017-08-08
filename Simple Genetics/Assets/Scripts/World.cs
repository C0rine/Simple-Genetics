using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World {

    private Genome[] population;

    private int kingScore = 14;
    private int nrOfGenes = 6;

	public World()
    {
        Debug.Log("Hello World!");
    }

    public void InitializePopulation(int popSize)
    {
        population = new Genome[popSize];

        for(int i=0; i < popSize; i++)
        {
            population[i] = new Genome(nrOfGenes);
            population[i].RandomInit();
        }
    }

    public Genome[] GetPopulation()
    {
        return population;
    }

    public int GetPopulationSize()
    {
        // should always be an even number
        return this.population.Length;
    }

    public int GetFitnessRating(Genome genome)
    {
        return genome.GetSumSixGenes();
    }

    public bool IsKing(Genome genome)
    {
        if(genome.GetSumSixGenes() == kingScore)
        {
            return true;
        }
        else
        {
            return false;
        }
    }



}
