#region About
// Author(s)    : Oliver Cox
// Last Changed : 23/04/2021
// Description  : Basis for all characters that will interact with one another during gameplay
#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterBase : MonoBehaviour
{
    public int _hp;                                             // current health
    public int _MAXhp;                                          // maximum health
    public float _moveSpeed;                                    // movement speed
    public EffectBase _currentEffect { get; protected set; }    // Currently applied status effect

    /// <summary>
    /// Constructor (unoffical)
    /// </summary>
    /// <param name="maxhp"></param>
    /// <param name="speed"></param>
    public virtual void Init(int maxhp, float speed)
    {
        this._MAXhp = maxhp;
        this._hp = maxhp;
        this._currentEffect = null;
        this._moveSpeed = speed;
    }

    /// <summary>
    /// Enables Reseting of character stats, allowing recycling
    /// </summary>
    public virtual void Reset()
    {
        _hp = _MAXhp;
        _currentEffect = null;
    }

    /// <summary>
    /// Applies status effect
    /// </summary>
    /// <param name="effect"></param>
    public virtual void ApplyEffect(EffectBase effect)
    {
        _currentEffect = effect;
        _currentEffect.OnApply();
        _currentEffect.EffectBehaviour();
    }

    /// <summary>
    /// Removes current status effect
    /// </summary>
    public virtual void RemoveEffect()
    {
        _currentEffect.OnRemove();
        _currentEffect = null;
    }

    /// <summary>
    /// Damages the character health
    /// </summary>
    /// <param name="dmg"></param>
    public abstract void Damage(int dmg);

    /// <summary>
    /// Kills the character
    /// </summary>
    protected abstract void Die();

}
