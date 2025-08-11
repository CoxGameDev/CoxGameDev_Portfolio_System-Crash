#region About
// Author(s)    : Oliver Cox
// Last Changed : 23/04/2021
// Description  : Testing that base class works - Defines Slowing Effect
#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEffect : EffectBase
{
    /// <summary>
    /// Testing Implimentation - Slowdown
    /// </summary>
    public static TestEffect instance;

    // Vars
    float[] speeds = new float[2];              // Speeds to switch between
    [SerializeField] float slowedSpeed = 1f;    // Slowed speed

    /// <summary>
    /// Actual effect of the behaviour
    /// </summary>
    public override void EffectBehaviour()
    {
        applicant._moveSpeed = speeds[0];
    }
    
    /// <summary>
    /// Pre-application code
    /// </summary>
    public override void OnApply()
    {
        this.applicant = this.GetComponent<CharacterBase>();

        speeds[0] = slowedSpeed;
        speeds[1] = applicant._moveSpeed;
    }

    /// <summary>
    /// Post-Effect code
    /// </summary>
    public override void OnRemove()
    {
        applicant._moveSpeed = speeds[1];

        this.applicant = null;
        Destroy(this);
    }
   
}
