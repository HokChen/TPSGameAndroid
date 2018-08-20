using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : FSMState {

    public PlayerWalkState()
    {
        stateID = StateID.Walk;
    }

    public override void DoBeforeEntering()
    {
        base.DoBeforeEntering();
    }
}
