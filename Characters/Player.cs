#region About
// Author(s)    : Oliver Cox & Gabriella H.
// Last Changed : ??/04/2021
// Description  : Application of CharacterBase that establishes player stats and enables their controlled alteration from elsewhere
#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponBase))]
[RequireComponent(typeof(PlayerMovementScript))]

public class Player : CharacterBase
{
    /// References ///
    public GameObject deathEffect;                      // GameObject to instanciate on death
    protected WeaponBase playerWeapons;                 // Attatched weapon script
    protected PlayerMovementScript playerMovement;      // Attacked movement script

    /// Fields ///
    [Header("Default Values")]
    [SerializeField] int HP = 1;                        // Inspector health value to apply in constructor
    [SerializeField] float Speed = 5f;                  // Inspector speed value to apply in constructor

    public int _PlayerScore { get; protected set; }     // Current score of the player

    #region Constructor
    public override void Init(int maxhp, float speed)
    {
        base.Init(maxhp, speed);
        playerWeapons = this.gameObject.GetComponent<WeaponBase>();
        playerMovement = this.gameObject.GetComponent<PlayerMovementScript>();
    }
    #endregion

    #region Runtime
    void Start()
    {
        // Gabi can you comment this at some point. - Oli

        Init(GameManager.instance._hp, Speed);
        ScoreUI.instance.ScoreManager(0);

        //TEST
        if (GameManager.instance._hp != 0)
        {
            _hp = GameManager.instance._hp;
            //Debug.Log("player _hp is " + _hp);
        }
        else
        {
            _hp = 10;
            GameManager.instance._hp = 10;
        }
        _PlayerScore = GameManager.instance._PlayerScore;
        //Debug.Log("player SCORE is " + +_PlayerScore);
        playerWeapons = GameManager.instance.playerWeapons;
        //Debug.Log("player Weapon data is " + playerWeapons);

        //TEST
    }

    #endregion

    #region Methods
    /// Functionality ///

    protected override void Die() 
    {
        GetComponent<PlayerMovementScript>().enabled = false;                   // Disable movement
        GameOverMenu.instance.SetDead(true);
        _hp = 0;
        GameManager.instance._hp = 0;
        Instantiate(deathEffect, transform.position, Quaternion.identity);      // Instanciate death visuals and sounds
        //Test
        //SavePlayer();
        //Test
        Destroy(this.gameObject);                                               // Remove player from scene
    }

    public override void Damage(int dmg)
    {
        //Debug.Log("PLayer HP IS " + _hp);
        _hp -= dmg;                                                             // Remove dmg from player hp
        TakeDamage.instance.Damage(dmg);
        //TEST
        SavePlayer();

        if (_hp <= 0) { Die(); }                                                // if health is too low, die
    }

    public void Heal(int healValue)
    {
        if (_hp + healValue <= 10)
        {
            _hp += healValue;
            //TEST
            SavePlayer();

            TakeDamage.instance.Heal(healValue);
        }


    }

    #endregion

    #region Properties

    public void Add_PlayerScore(int value)
    {
        _PlayerScore += value;                          // Assign Value

        //TEST
        SavePlayer();

        ScoreUI.instance.ScoreManager(_PlayerScore);    // Update UI
    }

    #endregion

    //TESTTTTT
    public void SavePlayer()
    {
        GameManager.instance._hp = _hp;
        GameManager.instance._PlayerScore = _PlayerScore;
        //GameManager.instance.playerWeapons.data._CurrentAmmo = playerWeapons.data._CurrentAmmo;
        GameManager.instance.playerWeapons = playerWeapons;
    }
    //TESTTT
}
