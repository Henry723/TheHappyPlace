using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Substates modify the current Superstate.
// Substates are not finite, and their listeners / transitions take priority over Superstates.
// Could potentially include combos, recoveries, or buffs & debuffs. 
public abstract class Substate : ScriptableObject
{
    // Finite parameters
    public Color sceneGizmoColor = Color.grey;

    // Generalized parameters
    [Header("Config paramaters")]
    [SerializeField] protected float timeToLive = 0f;
    public float TimeToLive {
        get { return timeToLive; }
        set { timeToLive = value; }
    }
    [SerializeField] protected Substate restingState = null;
    public Substate TransitionState { get { return restingState; } }
    [SerializeField] protected bool canBeRefreshed = false;
    public bool CanBeRefreshed { get { return canBeRefreshed; } }
    [SerializeField] protected bool interruptable = false;
    public bool Interruptable { get { return interruptable; } }
    [SerializeField] protected bool finite = false; // Whether the state is finite or pushdown
    public bool IsFinite { get { return finite; } }

    // Virtual Function declarations
    public virtual void OnStateEnter(StateFrame frame) { }
    public virtual void Listen(StateFrame frame) { }
    public virtual void OnStateExit(StateFrame frame) { }

    //  A substate evaluates equality simply if the other is of the same type.
    //      Ex. a GX_Recovery type will be returned from GetType(), as each Substate implements it's own type.
    //      The base.Equals function cannot be reliably used, as the parameters of each individual state
    //      such as time-to-live may have been modified when slotted into the StateFrame.
    public override bool Equals(object other) {
        Substate otherState = other as Substate;
        return (otherState.GetType() == this.GetType());
    }

    // REVIEW: GetHashCode is used when the Object of type is stored in a hash table.
    //          When Object.Equals is overridden, a warning is logged if Object.GetHashCode is not also overridden.
    //          If Substate ever needs to be stored in a hash table, re-evaluate this override.
    public override int GetHashCode() {
        return base.GetHashCode();
    }
}
