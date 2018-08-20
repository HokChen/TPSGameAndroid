using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMSystem {
    private Dictionary<StateID, FSMState> fsmStates;

    private FSMState currentState;
    public FSMState CurrentState
    {
        get
        {
            return currentState;
        }
    }

    public FSMSystem()
    {
        fsmStates = new Dictionary<StateID, FSMState>();
    }

    public void AddState(FSMState fSMState)
    {
        if (fSMState==null)
        {
            Debug.LogError("The state you want to add is null");
            return;
        }

        if (fsmStates.ContainsKey(fSMState.STATEID))
        {
            Debug.LogError("The state" + fSMState.STATEID + "you want to add has already been added");
            return;
        }

        fSMState.fsm = this;
        fsmStates.Add(fSMState.STATEID, fSMState);
    }

    public void DeleteState(FSMState fSMState)
    {
        if (fSMState==null)
        {
            Debug.LogError("The state you want to delete is null");
            return;
        }

        if (fsmStates.ContainsKey(fSMState.STATEID)==false)
        {
            Debug.LogError("The state you want to delete is not exist");
            return;
        }

        fsmStates.Remove(fSMState.STATEID);
    }

    public void PerformTransition(Transition transition)
    {
        if (transition==Transition.NullTransition)
        {
            Debug.LogError("NullTransition is not allowed for a real transition");
            return;
        }
        StateID id = currentState.GetOutPutState(transition);
        if (id==StateID.NullStateID)
        {
            Debug.Log("Transition is not to be happened,没有符合条件的转换");
            return;
        }

        FSMState state;
        fsmStates.TryGetValue(id, out state);
        currentState.DoBeforeLeaving();
        currentState = state;
        currentState.DoBeforeEntering();
    }

    public void Start(StateID stateID)
    {
        FSMState state;
        if (fsmStates.TryGetValue(stateID, out state))
        {
            state.DoBeforeEntering();
            currentState = state;
        }
        else
        {
            Debug.LogError("The state:" + stateID + "is not exist in fsm");
            return;
        }
    }
}
