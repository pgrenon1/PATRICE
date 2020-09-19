using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyType enemyType;
    public float timeBeforeDestroyAfterDeath = 2f;

    public Damageable Damageable { get; private set; }
    public EnemyManager EnemySpawner { get; set; }

    private Transform _target;
    private Collider _collider;
    private Boid _boid;

    private void Start()
    {
        _target = FindObjectOfType<Player>().transform;
        Damageable = GetComponent<Damageable>();
        _collider = GetComponentInChildren<Collider>();

        Damageable.Death += Damageable_Death;

        // register boid to BoidManager
        _boid = GetComponent<Boid>();
        BoidManager.Instance.RegisterBoid(_boid, enemyType);
    }

    private void Damageable_Death()
    {
        _collider.enabled = false;

        EnemySpawner.RemoveEnemy(this);

        Destroy(gameObject, timeBeforeDestroyAfterDeath);
    }
}
