using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

public class Genome {

    private bool[] genes;
    private int nrOfGenes = 0;
    private string stringRepr;

    public Genome(int nrOfDesiredGenes)
    {
        SetNrOfGenes(nrOfDesiredGenes);
        genes = new bool[nrOfGenes];
    }

    public void SetNrOfGenes(int nr)
    {
        nrOfGenes = nr;
    }

    public bool[] GetGenes()
    {
        return this.genes;
    }

    public void SetGenes(bool[] genesToSet)
    {
        this.genes = genesToSet;
    }

    public void RandomInit()
    {
        for(int i=0; i<nrOfGenes; i++)
        {
            bool randombool = (Random.value > 0.5f);
            this.ChangeGene(i, randombool);
        }
        
    }

    public void Clone()
    {
        //to do?
    }

    public void ChangeGene(int indexOfGene, bool Value)
    {
        this.genes[indexOfGene] = Value;
    }

    // the function below is hardcoded for the example with 6 genes!!
    public int GetSumSixGenes()
    {
        int sum = 0;
        for(int i = 0; i < nrOfGenes; i++)
        {
            if (i == 0 || i == 3)
            {
                sum += (this.genes[i] == true) ? 4 : 0;
            }
            else if(i == 1 || i == 4)
            {
                sum += (this.genes[i] == true) ? 2 : 0;
            }
            else if(i==2 || i == 5)
            {
                sum += (this.genes[i] == true) ? 1 : 0;
            }

        }

        return sum;
    }

    public void toString()
    {
        string stringholder = "";
        int half = nrOfGenes / 2;
        int max = nrOfGenes + 1;

        for (int i = 0; i < max; i++)
        {
            if (i < half)
            {
                stringholder += (genes[i] == true) ? "1" : "0";
            }
            else if (i > half)
            {
                stringholder += (genes[i-1] == true) ? "1" : "0";
            }
            else
            {
                stringholder += " ";
            }
        }

        stringRepr = stringholder;

    }

    public void Print()
    {
        this.toString();
        int sum = this.GetSumSixGenes();
        Debug.Log(stringRepr + " " + sum);
    }

}
