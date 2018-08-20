using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Transition
{
    NullTransition = 0,
    Fire,
    StopFire
}

public enum StateID
{
    NullStateID = 0,
    Idle,
    Run,
    Walk
}

public abstract class FSMState
{
    protected StateID stateID;
    public StateID STATEID { get { return stateID; } }

    protected Dictionary<Transition, StateID> map = new Dictionary<Transition, StateID>();

    public FSMSystem fsm;

    public void AddTransition(Transition transition, StateID stateID)
    {
        if (transition == Transition.NullTransition || stateID == StateID.NullStateID)
        {
            Debug.Log("Transition or StateID is null.");
            return;
        }

        if (map.ContainsKey(transition))
        {
            Debug.LogError("State:+" + stateID + "is already ha transition:" + transition);
            return;
        }

        map.Add(transition, stateID);
    }

    public void DeleteTransition(Transition transition)
    {
        if (map.ContainsKey(transition) == false)
        {
            Debug.LogWarning("The transition:" + transition + "you want to delete is not exist in map.");
            return;
        }
        map.Remove(transition);
    }

    public StateID GetOutPutState(Transition transition)
    {
        if (map.ContainsKey(transition))
        {
            return map[transition];
        }
        return StateID.NullStateID;
    }

    public virtual void DoBeforeEntering() { }

    public virtual void DoBeforeLeaving() { }

    public virtual void DoUpdate() { }
}