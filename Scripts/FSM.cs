using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM
{
    private State activeState;

    public void update()
    {
        if (activeState != null)
        {
            if (activeState.justEntered)
            {
                activeState.enter();
            }
            activeState.update();
        }
    }

    public void setState(State state)
    {
        activeState = state;
    }
}