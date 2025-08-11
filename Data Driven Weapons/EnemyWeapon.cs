#region About
// Author(s)    : Oliver Cox
// Last Changed : 23/04/2021
// Description  : Enables base class to be funcitonal with NPC's
#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : WeaponBase
{
    /// <summary>
    /// Override Method to work with NPCs
    /// </summary>
    /// <param name="attackRay"></param>
    protected override void RaycastBehaviour(RaycastHit attackRay)
    {
        if (attackRay.collider.CompareTag("Player"))                                    // Check for player...
        {
            attackRay.collider.gameObject.GetComponent<Player>().Damage(data._Damage);      // ...Damage them if found
        }
    }

    /// <summary>
    /// Abstraction that allows use elsewhere
    /// </summary>
    public void ReloadWeapon()
    {
        Reload();
    }

    public void Attack(bool shooting = false)
    {

        if (readyToShoot && shooting && !reloading && data._CurrentAmmo > 0)    // If able to shoot...
        {
            shotsToFire = data._BulletsPerShot;                                     // ...Assign number of shots
            Shoot();                                                                // ...Shoot!
        }
    }


}
