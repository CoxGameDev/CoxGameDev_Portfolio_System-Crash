#region About
// Author(s)    : Oliver Cox
// Last Changed : 23/04/2021
// Description  : MoveTo State - Moves to a given location then exits state
#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTo : StatesBase
{
    protected override void OnEnable()
    {
        base.OnEnable();
        stateType = StateTypes.MoveTo;                                      // Assign stateType so it can be used later
    }

    public override bool EnterState()
    {
        base.EnterState();

        _navMeshAgent.destination = _unit.GetGoal().transform.position;     // Set NavMesh Target

        return true;
    }

    public override void UpdateState()
    {
        if(_navMeshAgent.remainingDistance < 1f)                            // If at goal...
        {
            _unit.EvaluateState();                                              // ...Evaluate next state
        }
    }

}
