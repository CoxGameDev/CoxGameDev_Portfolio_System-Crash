#region About
// Author(s)    : Oliver Cox
// Last Changed : 23/04/2021
// Description  : Contains Data for Weapon System
#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "Weapons/Weapon Attributes")]

public class WeaponData : ScriptableObject
{
    #region Data

    /// Data Values ///
    public int _Damage;                     // Damage the weapon will deal per hit
    public int _CurrentAmmo;                // Current Amount of Ammo in Weapon
    public int _MaxAmmo;                    // Maximum capacity of ammo for the weapon 
    public int _BulletsPerShot;             // Projectiles used per attack

    public float _Spread;                   // Spread of Projectiles
    public float _Range;                    // Maximum Range of Projectiles 
    public float _ReloadTime;               // Time taken to reload
    public float _FireRate;                 // How quickly the weapon will fire between shots in a burst
    public float _AttackRate;               // How quickly the weapon will cooldown after an attack

    /// Flags ///
    public bool _ContinousAttack;           // If Attack Button can be Held Down to Attack Repeatedly

    /// Visual Elements ///
    public AudioClip _AttackAudio;          // SFX to Play when Attacking

    /*
    public ParticleSystem _VisualEffect;    // Particle Effect for Weapon Attacking

    public GameObject _WeaponModel;         // 3D Model for the Weapon
    public GameObject _FiringPoint;         // Location for instanciating - often at the end of the weapons barrel
    
    public GameObject _Projectile;          // Projectile to be Fired

    public int _StoredAmmo;                 // Amount of Ammo the Player has in reserve, disreguarding currently loaded ammo
    public int _MaxStoredAmmo;              // Maximum Cap for Stored Ammo
    */

    #endregion

    #region Behaviour

    public virtual void Init()
    {
        _CurrentAmmo = _MaxAmmo;
    }

    #endregion

}
