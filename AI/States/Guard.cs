#region About
// Author(s)    : Oliver Cox
// Last Changed : 23/04/2021
// Description  : Guard State - guards current location, exits when player enters weapon range
#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : StatesBase
{
    float speed = 2;                    // Turning speed while guarding and looking at the player
    float singleStep;                   // Max step to take in RotateTowards()
    Vector3 targetDir = new Vector3();  // Target Direction
    Vector3 newDir = new Vector3();     // Next tweened direction to move to

    protected override void OnEnable()
    {
        base.OnEnable();
        stateType = StateTypes.Guard;   // Assign stateType for later use
    }

    public override void UpdateState()
    {
        // if player is in range...
        if (EnemyBase.playerRef == null || (Vector3.Distance(_unit.transform.position, EnemyBase.playerRef.transform.position) < _unit._Weapon.data._Range))
        {
            _unit.EvaluateState();
        }
        // If player is out of range, turn to face them each frame
        else
        {
            targetDir = EnemyBase.playerRef.transform.position - _unit.transform.position;          // Direction relative to this

            singleStep = speed * Time.deltaTime;                                                    // Calculate a single step on that rotation

            newDir = Vector3.RotateTowards(_unit.transform.forward, targetDir, singleStep, 0.0f);   // Assign new direction 
            _unit.transform.rotation = Quaternion.LookRotation(newDir);                             // Use new direction to turn towards player
        }
    }

}
