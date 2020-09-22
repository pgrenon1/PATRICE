using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.gameObject.GetComponentInParent<Enemy>();

        if (enemy && enemy.IsImmuneToBorder)
            return;

        other.TryDamage(Mathf.Infinity);
    }
}
