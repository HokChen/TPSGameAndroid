using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : FSMState {

    public PlayerRunState()
    {
        stateID = StateID.Run;
    }

    public override void DoBeforeEntering()
    {
        
    }
}
