#region About
// Author(s)    : Oliver Cox
// Last Changed : 23/04/2021
// Description  : Basis for applying stat effects to characters
#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EffectBase : MonoBehaviour
{
    protected CharacterBase applicant;

    public abstract void EffectBehaviour();             // Core Implimentation of Effect
    
    public abstract void OnApply();                     // Immediate Action when Effect is applied
    
    public abstract void OnRemove();                    // Final Action when Effect is about to be removed

    public IEnumerator EffectTimer(float t = 5f)        // Call for a temporary Application.
    {
        OnApply();                                          // Pre-Effect Code
        EffectBehaviour();                                  // Effect Behaviour
        yield return new WaitForSeconds(t);                 // Wait until wears off
        OnRemove();                                         // Post-Effect Code
    }

}
