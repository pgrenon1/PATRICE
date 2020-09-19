using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float baseSpeed = 10f;
    public float lookAtSpeed = 0.5f;

    [Space]
    public float timeBeforeDestroyAfterDeath = 2f;

    private Transform _target;
    public Damageable Damageable { get; private set; }

    private void Start()
    {
        _target = FindObjectOfType<Player>().transform;
        Damageable = GetComponent<Damageable>();

        Damageable.Death += Damageable_Death;
    }

    private void Damageable_Death()
    {
        Destroy(gameObject, timeBeforeDestroyAfterDeath);
    }

    private void Update()
    {
        Vector3 lookTargetDir = _target.position - transform.position;
        //lookTargetDir.y = 0.0f;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lookTargetDir), Time.time * lookAtSpeed);
    }
}
