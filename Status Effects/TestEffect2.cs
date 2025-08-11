#region About
// Author(s)    : Oliver Cox
// Last Changed : 23/04/2021
// Description  : Testing that TestEffect works - Applies Effect using Triggers
#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// puddle example

public class TestEffect2 : MonoBehaviour
{

    TestEffect test = new TestEffect();             // Sample effect

    /// <summary>
    /// Use trigger to apply effects
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        test.OnApply();
        test.EffectBehaviour();
    }

    /// <summary>
    /// Remove effect on exit
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        test.OnRemove();
    }

}
