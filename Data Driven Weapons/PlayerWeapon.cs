#region About
// Author(s)    : Oliver Cox
// Last Changed : 23/04/2021
// Description  : Application of base class to enable cutom behaviour with the player
#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : WeaponBase
{
    public Animation reloadUI;              // UI that animates on reload
    [SerializeField] AudioClip reloadSFX;   // Sound that plays on reload

    /// <summary>
    /// FOR PLAYER SHOOTING
    /// </summary>
    void GetInput()
    {
        if (PauseMenu.gameIsPaused)
        {
            return;
        }

        if (data._ContinousAttack)                                                                  // If Player can hold attack button...
        {
            shooting = Input.GetButton("Fire1");
        }
        else                                                                                        // If Player cannot hold attack button...
        {
            shooting = Input.GetButtonDown("Fire1");
        }

        if (Input.GetKeyDown(KeyCode.R) && (data._CurrentAmmo < data._MaxAmmo) && (!reloading))     // If: (player wants to reload) + (Has less than full ammo) + (not reloading)
        {
            reloadUI.Play("ReloadingAnimation");
            sfx.clip = reloadSFX;
            sfx.Play();
            Reload();                                                                                   // Reload Weapon
        }

        if (readyToShoot && shooting && !reloading && data._CurrentAmmo > 0)                         // If: (Ready) + (Shooting) + (Not Reloading) + (Available Ammo)
        {
            shotsToFire = data._BulletsPerShot;                                                         // Enqueue Amount of Shots Needed
            Shoot();                                                                                    // Fire Weapon
        }

        if (!reloading && !shooting)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                //AmmoAndSpecial.instance.SetBulletIcon(0);
                ChangeWeapon(-1);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                //AmmoAndSpecial.instance.SetBulletIcon(1);
                ChangeWeapon(1);
            }
        }

    }

    protected override void Update()
    {
        base.Update();
        GetInput();
        AmmoAndSpecial.instance.SetAmmoUI(data._CurrentAmmo, data._MaxAmmo);
    }

}
