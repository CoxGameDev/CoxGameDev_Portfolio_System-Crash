#region About
// Author(s)    : Oliver Cox
// Last Changed : 23/04/2021
// Description  : Attack State - Causes the NPC to aim and attack the player
#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : StatesBase
{
    bool aim = false;                   // Finished aiming
    bool attacked = false;              // Finished Attacking
    bool waiting = false;               // Currently Waiting

    float speed = 2;                    // Aim Speed
    float singleStep;                   // Single tween step when turning
    Vector3 targetDir = new Vector3();  // Direction to turn to
    Vector3 newDir = new Vector3();     // Direction for tweening

    protected override void OnEnable()
    {
        base.OnEnable();
        stateType = StateTypes.Attack;  // Assign for use later
    }

    public override bool EnterState()
    {
        // Stop movement
        _navMeshAgent.isStopped = true; 
        // Reset flags
        aim = false;                    
        attacked = false;
        waiting = false;

        // Null check
        if (EnemyBase.playerRef != null)
        {
            // Calculate relative direction
            targetDir = EnemyBase.playerRef.transform.position - _unit.transform.position;
            return base.EnterState();
        }
        else
        {
            // Error Handling
            Debug.LogWarning(_unit.gameObject.name + ": Attack's EnterState() Cannot Reference Player");
            _unit.EvaluateState();
            return false;
        }
    }

    public override void UpdateState()
    {
        // Nullchecks
        if (EnemyBase.playerRef == null)
        {
            _unit.EvaluateState();
        }
        if(_unit == null) { return; }

        // If range is roughly melee
        if(_unit._Weapon.data._Range <= 1.5)
        {
            // If out of range
            if (Vector3.Distance(_unit.transform.position, EnemyBase.playerRef.transform.position) > _unit._Weapon.data._Range)
            {
                _unit.EvaluateState();
            }
        }

        // If this enemy hasn't aimed at the player yet...
        if (!aim)
        {
            // Establish a direction and turn to it
            targetDir = EnemyBase.playerRef.transform.position - _unit.transform.position;

            singleStep = speed * Time.deltaTime;

            newDir = Vector3.RotateTowards(_unit.transform.forward, targetDir, singleStep, 0.0f);
            _unit.transform.rotation = Quaternion.LookRotation(newDir);

            // If enemy is facing player...
            if (Vector3.Angle(_unit.transform.forward, targetDir) < 1)
            {
                // If animator is playing the correct animation... (Helps delay action until animation is finished switching)
                if (_unit._Animator.GetCurrentAnimatorStateInfo(0).IsName("Shooting_Rifle"))
                {
                    // Aiming is done!
                    aim = true;
                }
            }

        }

        // If this enemy has aimed but not attacked...
        else if (!attacked)
        {
            // If coroutine is not running...
            if (!waiting)
            {
                // Set coroutine to run (attack)
                waiting = true;
                _unit.StartCoroutine(ReloadWait());
            }
        }

        // If this enemy has aimed and attacked...
        else if (attacked && aim)
        {
            _unit.EvaluateState();
        }

        // If, somehow, something else has happened...
        else
        {
            Debug.LogError(_unit.name + ": Error while attacking");
        }
    }

    public override bool ExitState()
    {
        // Clean up visuals when exiting attack
        _navMeshAgent.isStopped = false;
        _unit.StopCoroutine(ReloadWait());
        return base.ExitState();
    }

    IEnumerator ReloadWait()
    {
        _unit.Attack();                                                     // Attack with weapon
        _unit._Weapon.ReloadWeapon();                                       // Start reloading weapon
        yield return new WaitForSeconds(_unit._Weapon.data._ReloadTime);    // Wait for reload, halting all other actions
        attacked = true;                                                    // Trip attack flag
    }

}
