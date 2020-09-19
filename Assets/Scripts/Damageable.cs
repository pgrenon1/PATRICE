using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public int startingHealth;

    public GameObject visualsParent;
    public GameObject deathFX;

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

            if (_currentHealth <= 0)
            {
                Die();
            }
        }
    }

    public bool IsDead { get; private set; }

    public delegate void OnDamageDealt(float damageValue);
    public event OnDamageDealt DamageDealt;

    public delegate void OnDeath();
    public event OnDeath Death;

    private bool _isPlayer = false;

    private void Start()
    {
        _isPlayer = this.IsPlayer();

        CurrentHealth = startingHealth;
    }

    public void ApplyDamage(float damageValue)
    {
        if (GameManager.Instance.IsGodMode && this.IsPlayer())
            return;

        float damageDealt = Mathf.Min(damageValue, CurrentHealth);

        CurrentHealth -= damageValue;

        if (DamageDealt != null)
            DamageDealt(damageValue);
    }

    public void Die()
    {
        HideVisuals();

        Utility.SpawnVFX(deathFX, transform.position, transform.rotation);

        IsDead = true;

        if (Death != null)
            Death();
    }

    private void HideVisuals()
    {
        visualsParent.SetActive(false);
    }
}
