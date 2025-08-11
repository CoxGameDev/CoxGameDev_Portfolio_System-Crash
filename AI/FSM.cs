#region About
// Author(s)    : Oliver Cox
// Last Changed : 23/04/2021
// Description  : Machine that handles the behaviour of each state with each NPC
#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyBase))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]

public class FSM : MonoBehaviour
{
    #region Fields

    #region References
    private Animator animator;                              // Attached Animator 
    private EnemyBase unit;                                 // Attached Enemybase subclass
    private NavMeshAgent nma;                               // Attached NavMesh Agent
    #endregion

    #region State Management
    [SerializeField] StateTypes startState;                 // State to start on
    public StatesBase _currentState { get; private set; }   // Currently active state

    StatesBase[] allStates = new StatesBase[]               // List of all valid states
    {
        new Attack(),
        new Chase(),
        new Flee(),
        new Guard(),
        new MoveTo()
    };

    // Dict that connects states to an easily accessable enum type for abstract sourcing later
    public Dictionary<StateTypes, StatesBase> _StateRefDictionary { get; private set; } = new Dictionary<StateTypes, StatesBase>();
    #endregion

    #endregion

    #region Runtime

    private void Start()
    {
        // Assign values at runtime
        _currentState = null;
        animator = GetComponent<Animator>();
        unit = GetComponent<EnemyBase>();
        nma = GetComponent<NavMeshAgent>();

        // Link enum to states

        /* Ideal Implimentaiton - Research issue if time permits
        
        foreach (StatesBase state in allStates)
        {
            state._unit = GetComponent<EnemyBase>();
            state._navMeshAgent = GetComponent<NavMeshAgent>();
            state._fsm = this;
            _StateRefDictionary.Add(state.stateType, state);
        }

        */

        // Working Implimentation - Requires StateTypes and allStates[] to be in the same order
        for (int i = 0; i < allStates.Length; i++)
        {
            allStates[i]._unit = unit;
            allStates[i]._navMeshAgent = nma;
            allStates[i]._fsm = this;
            _StateRefDictionary.Add((StateTypes)i, allStates[i]);

        }
        EnterState(_StateRefDictionary[startState]);
    }

    private void Update()
    {
        if (_currentState != null)
        {
            _currentState.UpdateState();
        }
    }

    #endregion

    #region State Management

    /// <summary>
    /// Enter state using an object type
    /// </summary>
    /// <param name="nextState"></param>
    public void EnterState(StatesBase nextState)
    {
        if (nextState == null) { return; }
        if (_currentState != null) { _currentState.ExitState(); }

        _currentState = nextState;
        _currentState.EnterState();
    }

    /// <summary>
    /// Enter state using enum
    /// </summary>
    /// <param name="nextState"></param>
    public void EnterState(StateTypes nextState)
    {
        if (_StateRefDictionary.ContainsKey(nextState))
        {
            StatesBase state = _StateRefDictionary[nextState];

            EnterState(state);
        }
    }

    #endregion

}
