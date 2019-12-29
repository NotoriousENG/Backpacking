using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective
{
    public string name;
    public string description;

    public Objective()
    {
        this.name = "";
        this.description = "";
    }
    public Objective(string name, string description)
    {
        this.name = name;
        this.description = description;
    }
}
