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

        for (int i = 0; i < popSize; i++)
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

        for (int i = 0; i < this.GetPopulationSize(); i++)
        {
            total += this.population[i].GetSumSixGenes();
        }
        return total;
    }

    public Genome[] BiasedRouletteWheel()
    {
        float populationFitness = (float)this.GetFitness();
        float[] weightedValues = new float[GetPopulationSize()];
        Genome[] winningGenomes = new Genome[2];

        int counter = 0;

        // This will work, but is far from elegant
        while (counter < 2)
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
        if (genome.GetSumSixGenes() == kingScore)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // hardcoded to work for a population of 4 genomes
    public void CreateNextGen(Genome[] currGen)
    {
        Genome[] nextGen = new Genome[this.GetPopulationSize()];

        Genome[] maters = this.BiasedRouletteWheel();

        int pointOfSwap1 = Random.Range(1, nrOfGenes);
        Genome[] childpair1 = this.CrossOver(maters, pointOfSwap1);
        int pointOfSwap2 = Random.Range(1, nrOfGenes);
        Genome[] childpair2 = this.CrossOver(maters, pointOfSwap2);

        nextGen[0] = childpair1[0];
        nextGen[1] = childpair1[1];
        nextGen[2] = childpair2[0];
        nextGen[3] = childpair2[1];

        Debug.Log("New generation:");

        for(int i=0; i < nextGen.Length; i++)
        {
            nextGen[i].Print();
        }

    }

    // hardcoded for 2 parents and 2 outcomming children
    public Genome[] CrossOver(Genome[] maters, int pointOfSwap)
    {
        Genome[] children = new Genome[2];

        children[0] = DeepCopyGenome(maters[0]);
        children[1] = DeepCopyGenome(maters[1]);

        Debug.Log("Deepcopies of the maters:");
        children[0].Print();
        children[1].Print();
       
        bool[] setA = new bool[pointOfSwap];
        bool[] setB = new bool[pointOfSwap];

        Debug.Log("Performing Crossover at: " + pointOfSwap);

        for (int i=0; i < maters.Length; i++)
        {
            for (int j=0; j < pointOfSwap; j++)
            {
                if (i == 0)
                {
                    setA[j] = maters[i].GetGenes()[j];
                }
                else if(i == 1)
                {
                    setB[j] = maters[i].GetGenes()[j];
                }
            }
        }

        for (int i = 0; i < children.Length; i++)
        {
            for (int j = 0; j < pointOfSwap; j++)
            {
                if (i == 0)
                {
                    children[i].GetGenes()[j] = setB[j];
                    Debug.Log("Child " + i + " at swappoint " + j);
                    children[i].Print();
                }
                else if (i == 1)
                {
                    children[i].GetGenes()[j] = setA[j];
                }
            }
        }
        return children;
    }

    public Genome DeepCopyGenome(Genome toBeCopied)
    {
        Genome theCopy = new Genome(nrOfGenes);
        bool[] toBeCopiedGenes = toBeCopied.GetGenes();
        bool[] copiedGenes = new bool[nrOfGenes];

        for(int i = 0; i < nrOfGenes; i++)
        {
            if(toBeCopiedGenes[i] == true)
            {
                copiedGenes[i] = true;
            }
            else
            {
                copiedGenes[i] = false;
            }
        }

        theCopy.SetGenes(copiedGenes);
        return theCopy;

    }

}
