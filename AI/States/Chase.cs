#region About
// Author(s)    : Oliver Cox
// Last Changed : 23/04/2021
// Description  : Chase State - Chases the player, effectively moving towards its new location each update
#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : StatesBase
{
    protected override void OnEnable()
    {
        base.OnEnable();
        stateType = StateTypes.Chase;   // Assign for later use
    }

    public override bool EnterState()
    {
        base.EnterState();

        // Ensure NMA can move
        _navMeshAgent.isStopped = false;
        
        // Nullcheck
        if(EnemyBase.playerRef != null)
        {
            _navMeshAgent.destination = EnemyBase.playerRef.transform.position;
        }

        return true;
    }

    public override void UpdateState()
    {
        // If player in range...
        if ((_navMeshAgent.remainingDistance <= _unit._Weapon.data._Range) || EnemyBase.playerRef == null)
        {
            _unit.EvaluateState();
        }
        // If player out of range...
        else
        {
            _navMeshAgent.destination = EnemyBase.playerRef.transform.position;
        }
    }

    public override bool ExitState()
    {
        // Remain still
        _navMeshAgent.destination = _unit.transform.position;
        return base.ExitState();
    }
}
