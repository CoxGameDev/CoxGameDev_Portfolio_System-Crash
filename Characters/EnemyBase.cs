#region About
// Author(s)    : Oliver Cox
// Last Changed : 23/04/2021
// Description  : Generalised base for all enemies that will be opponents for the player
#endregion
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyWeapon))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Collider))]

public abstract class EnemyBase : CharacterBase
{
    /// Fields ///
    public static GameObject playerRef { private set; get; } = null;            // Static reference to the player across all objects of this type
    public EnemyWeapon _Weapon { get; protected set; }                          // Weapon attatched to this enemy character
    public int scoreValue { get; protected set; }                               // Assigned score value of this character instance
    public Animator _Animator { get; protected set; }                           // Attatched Animator

    protected FSM _fsm;                                                         // Attatched Finite State Machine
    protected NavMeshAgent _NavAgent;                                           // Attatched NavMesh Agent
    private GameObject _goal;                                                   // Goal for this character to move to

    private GameObject spawnOrigin;                                             // Point this character spawned from
    [SerializeField] GameObject deathEffect;                                    // GameObject to instanciate on death for visuals

    /// Default Values ///
    [Header("Default Values - Base")]
    [SerializeField] int ScoreValue = 0;                                        // Inspector assigned Score value
    [SerializeField] int HP = 1;                                                // Inspector assigned Health
    [SerializeField] float Speed = 5f;                                          // Inspector assigned Speed

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="maxhp"></param>
    /// <param name="speed"></param>
    public override void Init(int maxhp, float speed)
    {
        base.Init(maxhp, speed);

        _Animator = this.GetComponent<Animator>();
        _fsm = this.GetComponent<FSM>();
        _NavAgent = this.gameObject.GetComponent<NavMeshAgent>();
        _Weapon = this.gameObject.GetComponent<EnemyWeapon>();
        scoreValue = ScoreValue;

    }

    /// <summary>
    /// Evaluates the next state to transition to
    /// </summary>
    public virtual void EvaluateState()
    {
        // Null check for if the player is dead/unassigned
        if(EnemyBase.playerRef == null)
        {
            Debug.LogWarning("Player Reference cannot be found for EnemyBase or Sub-Classes");
            _goal = spawnOrigin;
            _fsm.EnterState(StateTypes.MoveTo);
        }

        return;
    }

    /// <summary>
    /// Setter for _goal
    /// </summary>
    /// <param name="input"></param>
    protected virtual void SetGoal(GameObject input)
    {
        if(input != null)
        {
            _goal = input;
        }
        else
        {
            Debug.LogError(gameObject.name + ": Given Gameobject is Null");
        }
    }

    /// <summary>
    /// Getter for _goal
    /// </summary>
    /// <returns></returns>
    public virtual GameObject GetGoal()
    {
        if(_goal == null)
        {
            if(EnemyBase.playerRef != null)
            {
                Debug.LogWarning(this.gameObject.name + ": _goal was null, returned player object.");
                return EnemyBase.playerRef;
            }
            else
            {
                // If this happens, both the goal and player references are null - most likely problematic
                Debug.LogWarning(this.gameObject.name + ": _goal was null, returned Null.");
            }
            
        }

        return _goal;

    }

    /// <summary>
    /// Setter for spawnOrigin
    /// </summary>
    /// <param name="input"></param>
    public void SetSpawn(GameObject input)
    {
        if(input != null)
        {
            spawnOrigin = input;
        }
        else
        {
            Debug.LogWarning(gameObject.name + ": SetSpawn() was null.");
        }
    }

    /// <summary>
    /// Getter for spawnOrigin
    /// </summary>
    /// <returns></returns>
    public GameObject GetSpawn()
    {
        if (spawnOrigin == null)
        {
            Debug.LogWarning(gameObject.name + ": GetSpawn() was null.");
        }

        return spawnOrigin;
    }

    /// <summary>
    /// Attacking Behaviour
    /// </summary>
    /// <returns></returns>
    public abstract bool Attack();

    /// <summary>
    /// Taking damage behaviour
    /// </summary>
    /// <param name="dmg"></param>
    public override void Damage(int dmg)
    {
        _hp -= dmg;

        if (_hp <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Behaviour for dying
    /// </summary>
    protected override void Die()
    {
        if (deathEffect != null)
        {
            Instantiate(deathEffect, this.transform.position, Quaternion.identity);                     // Instanciate deathEffect
        }

        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().Add_PlayerScore(ScoreValue);  // Add score to player
        Destroy(this.gameObject);
    }


    protected virtual void Start()
    {
        Init(HP, Speed);                                            // Initalise this character properly
        _NavAgent.speed = this._moveSpeed;                          // Assign speed

        if (playerRef == null)
        {
            playerRef = GameObject.FindGameObjectWithTag("Player"); // Find player if not already available
        }

    }

}
