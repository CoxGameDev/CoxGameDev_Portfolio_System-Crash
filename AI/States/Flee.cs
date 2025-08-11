#region About
// Author(s)    : Oliver Cox
// Last Changed : 23/04/2021
// Description  : Flee State - Moves away from the player
#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flee : StatesBase
{
    [SerializeField] float r = 3;       // Radius to flee within

    float d = 0;                        // Distance
    Vector3 dir = new Vector3();        // Direction
    Vector3 newPos = new Vector3();     // New Position to flee to

    bool waiting = false;               // Flag: If Waiting
    public float waitTime = 7.5f;       // Time to wait
    float endTime;                      // Time to execute next behaviour

    protected override void OnEnable()
    {
        base.OnEnable();
        stateType = StateTypes.Flee;    // Assign for later use
    }

    public override bool EnterState()
    {
        endTime = Time.time + waitTime; // Set goal time as a constant point, waitTime seconds from now.
        return base.EnterState();
    }

    public override void UpdateState()
    {
        if(EnemyBase.playerRef == null)
        {
            FleeGeneral();              // Flee to spawnOrigin
        }
        else
        {
            FleePlayer();               // Constantly move away from player if in range
        }
        
    }

    /// <summary>
    /// Flees player and then waits until safe
    /// </summary>
    private void FleePlayer()
    {
        d = Vector3.Distance(_unit.transform.position, EnemyBase.playerRef.transform.position); // Distance between player and this

        if (d < r)                                                                              // If player is within fleeing distance...
        {
            dir = _unit.transform.position - EnemyBase.playerRef.transform.position;                // Calculate inverse direction to player
            newPos = _unit.transform.position + dir;                                                // Calculate new position using inverse direction

            _navMeshAgent.SetDestination(newPos);                                                   // Move to inverse dir position

            if (waiting)                                                                            // if currently waiting...
            {
                waiting = false;                                                                        // revert wait flag
                endTime = Time.time + waitTime;                                                         // Assign new end time
            }

        }

        else if (!waiting)                                                                      // If not waiting...
        {
            waiting = true;                                                                         // Set to wait
            endTime = Time.time + waitTime;                                                         // establish a new endTime
        }
        else if (waiting)                                                                       // If waiting...
        {
            if (Time.time >= endTime)                                                               // If enough time has elapsed...
            {
                _unit.EvaluateState();                                                                  // Move to new state
            }
        }
    }

    /// <summary>
    /// Flees back "home"
    /// </summary>
    private void FleeGeneral()
    {
        _navMeshAgent.SetDestination(_unit.GetSpawn().transform.position);  // Set movement destination

        if(_navMeshAgent.remainingDistance < 0.75f)                         // if within range...
        {
            if (!waiting)                                                       // if not currently waiting...
            {
                waiting = true;                                                     // set wait flag
                endTime = Time.time + waitTime;                                     // establish end time
            }
            else if (waiting)                                                   // if waiting...
            {
                if (Time.time >= endTime)                                           // if time elapsed...
                {
                    _unit.EvaluateState();                                              // Exit to another state
                }
            }
        }
    }

}
