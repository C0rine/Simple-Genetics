using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World {

    private Genome[] population;

    private int kingScore = 14;
    private int nrOfGenes = 6;
    private int nrOfWinningGenesPerRound = 2;

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
        return this.population;
    }

    public int GetPopulationSize()
    {
        // should always be an even number
        return this.population.Length;
    }

    public int GetFitness()
    {
        int total = 0;

        for(int i=0; i < this.GetPopulationSize(); i++)
        {
            total += this.population[i].GetSumSixGenes();
        }
        return total;
    }

    public Genome[] BiasedRouletteWheel()
    {
        float populationFitness = (float) this.GetFitness();
        float[] weightedValues = new float[GetPopulationSize()];
        Genome[] winningGenomes = new Genome[2];

        int counter = 0;

        // This will work, but is far from elegant
        while(counter < 2)
        {
            for (int i = 0; i < this.GetPopulationSize(); i++)
            {
                weightedValues[i] = ((float)this.population[i].GetSumSixGenes() / populationFitness);
                if (this.WeightedLotteryWin(weightedValues[i], populationFitness) && counter == 0)
                {
                    winningGenomes[0] = this.population[i];
                    counter += 1;
                }
                else if (this.WeightedLotteryWin(weightedValues[i], populationFitness) && counter == 1)
                {
                    winningGenomes[1] = this.population[i];
                    counter += 1;
                }
                else if (counter >= 2)
                {
                    break;
                }
            }
        }

        Debug.Log("Winner one:");
        winningGenomes[0].Print();
        Debug.Log("Winner two:");
        winningGenomes[1].Print();

        return winningGenomes;
    }

    private bool WeightedLotteryWin(float chance, float totalfitness)
    {
        float random = Random.Range(0.0f, 1.0f);

        if (chance >= random)
        {
            return true;
        }
        else
        {
            return false;
        }
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
