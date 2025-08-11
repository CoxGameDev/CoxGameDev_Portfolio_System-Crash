#region About
// Author(s)    : Oliver Cox
// Last Changed : 23/04/2021
// Description  : Base class for Weapon System. Allows shooting, reloading, etc
#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    #region Data Types
    /// <summary>
    /// Container for a Weapons Data and 3D Model
    /// </summary>
    public struct Weapon
    {
        public WeaponData data;             // Data from ScriptableObject
        public GameObject model;            // Model from Scene
        public GameObject fireLoc;          // Firing Point
        public ParticleSystem particle;     // Particle System

        public Weapon(WeaponData d, GameObject m)
        {
            this.data = Object.Instantiate(d);  /// <------------------------------------------- Check this later. Should instanciate a new object for each use.
            this.model = m;
            this.fireLoc = this.model.GetComponentInChildren<FiringPoint>().gameObject;
            this.particle = this.model.GetComponentInChildren<ParticleSystem>();
        }
    }
    #endregion

    #region Declarations

    // Flags //
    protected bool shooting;                        // is Shooting?
    protected bool reloading;                       // is Reloading?
    protected bool readyToShoot;                    // is Ready to Shoot?

    // Weapon Data //
    public WeaponData data;                         // Currently held weapon
    public GameObject _WeaponModel;                 // 3D Model for the Weapon
    public GameObject _FiringPoint;                 // Location for instanciating - often at the end of the weapons barrel
    protected int shotsToFire = 0;                  // Queue of shots to fire (for multi-shot weapons)
    protected int weaponIndex = 0;                  // Currently Held Weapon as Index in Dictionary

    // Refs  //
    RaycastHit attackRay;                           // Store raycast to interact with environment
    protected AudioSource sfx;                      // AudioSource attatched to this character
    ExplosiveBarrel barrelRef;                      // ???

    public WeaponData[] weaponsList;                // Array of available weapons - Data
    public GameObject[] modelsList;                 // Array of available weapons - 3D Models
                                                    // Dictionary for referencing both
    protected Dictionary<int, Weapon> dictWeapons = new Dictionary<int, Weapon>();

    #endregion

    protected virtual void Awake()
    {
        InitialiseWeapons();
        ChangeWeapon(0);                            // Change To Default Weapon
        readyToShoot = true;                        // Allow Firing at Startup
        sfx = GetComponent<AudioSource>();          // Get ref of this audio player
    }

    protected virtual void Update() { }

    #region Sub-Methods

    protected virtual void RaycastBehaviour(RaycastHit attackRay)
    {
        if (attackRay.collider.CompareTag("Enemy") && !attackRay.collider.CompareTag("Player"))  // If a character is hit...
        {
            attackRay.collider.gameObject.GetComponent<CharacterBase>().Damage(data._Damage);       // ...Damage them
        }
        else if (attackRay.collider.CompareTag("ExpBarrel")|| attackRay.collider.CompareTag("ExpBarrel2")|| attackRay.collider.CompareTag("ExpBarrel3"))// If a hazard is hit
        {
            // activate hazard                
            ExplosiveBarrel.name = attackRay.collider.tag;

            /*
             * Just a note, you could instead do...
             * 
             * else if(attackRay.collider.CompareTag("ExpBarrel"))
             * {
             *      attackRay.collider.gameObject.GetComponent<ExplosiveBarrel>().BeenHit();
             * }
             * 
             * ...which would avoid all your current issues
             * 
             *  - Oli
            */
        }
    }

    protected void InitialiseWeapons()
    {
        if (weaponsList.Length != modelsList.Length) // Error Handling
        {
            Debug.LogError(gameObject.name + ": Not all weapons have both data and a MODEL (List Size Incompatable)");
        }
        else                                        // Dictionary Creation
        {
            for (int i = 0; i < weaponsList.Length; i++)
            {
                Weapon temp = new Weapon(weaponsList[i], modelsList[i]);
                if (modelsList[i] == null || weaponsList[i] == null)
                {
                    Debug.LogError(gameObject.name + ": Index " + i + " is missing its data or model");
                }
                else
                {
                    temp.model.SetActive(false);    // Set all guns to false
                    dictWeapons[i] = temp;
                }
            }
        }

        for (int i = 0; i < weaponsList.Length; i++)// Initialise All Weapons
        {
            weaponsList[i].Init();
        }

    }

    protected void Shoot()
    {
        readyToShoot = false;                                                                       // Trip Flag

        float spread = Random.Range(-data._Spread, data._Spread);                                   // Calculate X-Axis Spread from WeaponData
        Vector3 dir = _FiringPoint.transform.forward + new Vector3(spread, 0, spread);              // Get Direction to Fire

        Debug.DrawRay(_FiringPoint.transform.position, dir * data._Range, Color.red, 5); // TEST: Draw Bullets

        if (Physics.Raycast(_FiringPoint.transform.position, dir, out attackRay, data._Range))       // Raycast out from weapon to world
        {
            RaycastBehaviour(attackRay);
        }

        if (dictWeapons[weaponIndex].particle != null)
        {
            dictWeapons[weaponIndex].particle.transform.forward = dir;                              // Change forward so bullet visual fires accurately
            dictWeapons[weaponIndex].particle.Play(true);                                           // Apply Visual Effect
        }

        if(sfx != null)
        {
            sfx.clip = dictWeapons[weaponIndex].data._AttackAudio;
            sfx.Play();
        }

        data._CurrentAmmo--;                                                                        // Decrement Ammo
        shotsToFire--;                                                                              // Decrement Queue

        Invoke("ResetShot", data._AttackRate);                                                      // Reset Flag to Let Player Shoot After a Time

        if (data._CurrentAmmo > 0 && shotsToFire > 0)                                               // If: (Available Ammo) + (Are Shots Left to Shoot)
        {
            Invoke("Shoot", data._FireRate);                                                            // Shoot the next bullet in the queue
        }

    }

    protected void Reload()
    {
        reloading = true;                               // Trip reloading flag
        Invoke("ReloadFinished", data._ReloadTime);     // Once ReloadTime has elapsed, Run Reload Action
    }

    protected void ChangeWeapon(int change)
    {
        change += weaponIndex;                                  // Add current index to possible change
        if (change < 0 || change > (weaponsList.Length - 1))    // If out of range...
        {
                                                                    // ...do nothing.
        }
        else                                                    // If in range...
        {
            weaponIndex = change;                                   // set index to new value
            _WeaponModel.SetActive(false);                          // deactivate old model
        }

        data = dictWeapons[weaponIndex].data;                   // Assign new data
        _WeaponModel = dictWeapons[weaponIndex].model;          // Assign new model

        _WeaponModel.SetActive(true);                           // Activate model
        _FiringPoint = dictWeapons[weaponIndex].fireLoc;        // Assign Firing Point

    }

    /// Invoking Methods ///

    protected void ReloadFinished()
    {
        data._CurrentAmmo = data._MaxAmmo;              // Reload Clip
        reloading = false;                              // Detrip reloading flag
    }

    protected void ResetShot()
    {
        readyToShoot = true;                            // Untrip flag to allow shooting
    }

    #endregion
}
