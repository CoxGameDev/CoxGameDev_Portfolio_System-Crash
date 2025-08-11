#region About
// Author(s)    : Oliver Cox
// Last Changed : 23/04/2021
// Description  : Base for all states in the FSM - controls the Enter, Update & Exit of the state
#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#region Enums: ExecutionState + StateTypes

public enum ExecutionState                                              // Current state of execution
{ 
    NULL, 
    EXECUTING, 
    EXECUTED, 
    ERRORED 
};

public enum StateTypes                                                  // Names of states that are available
{
    Attack,
    Chase,
    Flee,
    Guard,
    MoveTo
};

#endregion

public abstract class StatesBase
{
    #region Fields
    public EnemyBase _unit;                                             // Attatched AI Unit - holds FSM and NMA components.
    public FSM _fsm;                                                    // Attatched Finite State Machine - controls AI unit.
    public NavMeshAgent _navMeshAgent;                                  // Attatched NMA - To send movement commands.

    public ExecutionState executionState { get; protected set; }        // Current state of execution - allows effective error handling
    public StateTypes stateType { get; protected set; }                 // Collection of all states in an easily accessable data type

    #endregion

    #region Runtime

    protected virtual void OnEnable()
    {
        executionState = ExecutionState.NULL;
    }

    #endregion

    #region State Functionality

    /// <summary>
    /// Sets pre-conditions for state
    /// </summary>
    /// <returns></returns>
    public virtual bool EnterState()
    {
        executionState = ExecutionState.EXECUTING;                      // State is attempting to execute behaviour, set flag accoringly.

        bool output = true;
        if (_unit == null) { output = false; }                          // Test if there is an AI unit component attached
        if (_navMeshAgent == null) { output = false; }                  // Test if there is a NMA component attached

        return output;
    }

    /// <summary>
    /// Actions to be performed while in state
    /// </summary>
    public abstract void UpdateState();

    /// <summary>
    /// Exits state cleanly
    /// </summary>
    /// <returns></returns>
    public virtual bool ExitState()
    {
        bool output = true;                                             // Exists such that tests can be easily added
        executionState = ExecutionState.EXECUTED;                       // State has executed behaviour, set flag accoringly.
        return output;
    }

    #endregion

}
