using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage
{
    public float Value { get; set; }
    public bool OnDimension { get; set; }
    public bool IsPlayerDamage { get; set; }

    public Damage(float value)
    {
        Value = value;
    }

    public Damage(float value, bool onDdimension, bool isPlayerDamage)
    {
        Value = value;
        OnDimension = onDdimension;
        IsPlayerDamage = isPlayerDamage;
    }
}

public class Damageable : MonoBehaviour
{
    public int startingHealth;

    public GameObject visualsParent;
    public GameObject deathFX;
    public float deathFXScale = 3f;

    private float _currentHealth;
    public float CurrentHealth
    {
        get
        {
            return _currentHealth;
        }
        set
        {
            _currentHealth = value;
        }
    }

    public bool IsDead { get; private set; }

    public delegate void OnDamageDealt(Damage damage);
    public event OnDamageDealt DamageDealt;

    public delegate void OnDeath(Damage damage);
    public event OnDeath Death;

    private bool _isPlayer = false;

    private void Start()
    {
        _isPlayer = this.IsPlayer();

        CurrentHealth = startingHealth;
    }

    public void ApplyDamage(Damage damage)
    {
        if (GameManager.Instance.IsGodMode && _isPlayer)
            return;

        float damageDealt = Mathf.Min(damage.Value, CurrentHealth);

        CurrentHealth -= damageDealt;

        if (_currentHealth <= 0)
        {
            Die(damage);
        }

        if (DamageDealt != null)
            DamageDealt(damage);
    }

    public void Die(Damage damage)
    {
        HideVisuals();

        Utility.SpawnVFX(deathFX, transform.position, transform.rotation, deathFXScale);

        IsDead = true;

        if (Death != null)
            Death(damage);
    }

    private void HideVisuals()
    {
        visualsParent.SetActive(false);
    }
}
