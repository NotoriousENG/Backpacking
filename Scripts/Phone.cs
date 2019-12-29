using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phone
{
    public List<Objective> objectiveList;
    public Phone()
    {
           objectiveList = new List<Objective>();
    }

    public void addObjective(Objective objective)
    {
        objectiveList.Add(objective);
    }

    public void removeObjective(Objective objective)
    {
        int index = 0;
        for (int i = 0; i < objectiveList.Count; i++)
        {
            if (objectiveList[i].name == objective.name)
                index = i;
        }

        objectiveList.RemoveAt(index);
    }

    public bool isEmpty()
    {
        if (objectiveList.Count == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public Objective[] getAllObjectives()
    {
        Objective[] objectives = new Objective[objectiveList.Count];
        for (int i = 0; i < objectiveList.Count; i++)
        {
            objectives[i] = objectiveList[i];
        }
        return objectives;
    }


}
