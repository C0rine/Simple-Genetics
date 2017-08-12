using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World {

    private Genome[] population;

    public int crossOverChance = 20;
    public int mutationChance = 20;

    private int kingScore = 14;
    private int nrOfGenes = 6;

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

    public bool FoundWinner()
    {
        int counter = 0;

        for(int i = 0; i < this.population.Length; i++)
        {
            counter += (this.IsKing(this.population[i])) ? 1 : 0;
        }

        return (counter > 0) ? true : false;
    }

    // hardcoded to work for a population of 4 genomes
    public void CreateNextGen(Genome[] currGen)
    {
        Genome[] nextGen = new Genome[this.GetPopulationSize()];

        Genome[] maters = this.BiasedRouletteWheel();

        // perform mutation if percentage wins
        int random = Random.Range(0, 100);
        if (random <= mutationChance)
        {
            maters = this.Mutate(maters);
        }

        // oerform cross over if percentage wins
        int random2 = Random.Range(0, 100);
        if(random2 <= crossOverChance)
        {
            Genome[] childpair1 = this.CrossOver(maters);
            Genome[] childpair2 = this.CrossOver(maters);

            nextGen[0] = childpair1[0];
            nextGen[1] = childpair1[1];
            nextGen[2] = childpair2[0];
            nextGen[3] = childpair2[1];
        }
        else
        {
            nextGen[0] = maters[0];
            nextGen[1] = maters[1];
            nextGen[2] = maters[0];
            nextGen[3] = maters[1];
        }


        Debug.Log("New generation:");

        for(int i=0; i < nextGen.Length; i++)
        {
            nextGen[i].Print();
        }

    }

    // swaps one gene of two genomes 
    public Genome[] Mutate(Genome[] input)
    {
        int swappoint = Random.Range(0, 5);
        bool swapped = false;

        Debug.Log("Performing mutation at: " + swappoint);

        for(int i = 0; i < input.Length; i++)
        {
            for(int j = 0; j < input[i].GetGenes().Length; j++)
            {
                if (j == swappoint && i == 0)
                {
                    swapped = (input[1].GetGenes()[j] != input[0].GetGenes()[j]) ? true : false;
                    input[1].GetGenes()[j] = (input[0].GetGenes()[j] == true) ? true : false;
                }
                if(j == swappoint && i == 1 && swapped == true)
                {
                    input[0].GetGenes()[j] = (input[1].GetGenes()[j] == true) ? false : true;
                }
            }
        }
        return input;
    }

    // hardcoded for 2 parents and 2 outcomming children
    public Genome[] CrossOver(Genome[] maters)
    {
        Genome[] children = new Genome[2];
        int pointOfSwap = Random.Range(1, nrOfGenes);

        children[0] = DeepCopyGenome(maters[0]);
        children[1] = DeepCopyGenome(maters[1]);
       
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
