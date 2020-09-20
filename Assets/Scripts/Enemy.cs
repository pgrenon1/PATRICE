using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float timeBeforeDestroyAfterDeath = 2f;
    public DimensionDependantVisuals dimensionDependantVisualsOn;
    public DimensionDependantVisuals dimensionDependantVisualsOff;

    public EnemyType EnemyType { get; set; }
    public Damageable Damageable { get; private set; }
    public EnemyManager EnemySpawner { get; set; }
    public Dimension OriginDimension { get; set; }
    public Boid Boid { get; private set; }

    private Transform _target;
    private Collider _collider;

    private void Start()
    {
        _target = FindObjectOfType<Player>().transform;
        Damageable = GetComponent<Damageable>();
        _collider = GetComponentInChildren<Collider>();

        Damageable.Death += Damageable_Death;

        // register boid to BoidManager
        Boid = GetComponent<Boid>();
        BoidManager.Instance.RegisterBoid(Boid, EnemyType, _target);
    }

    private void Update()
    {
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        bool isOn = IsOnOriginDimension();

        _collider.enabled = isOn && !Damageable.IsDead;

        dimensionDependantVisualsOn.fireVisuals.SetActive(GameManager.Instance.ActiveDimension == Dimension.Fire && isOn);
        dimensionDependantVisualsOn.iceVisuals.SetActive(GameManager.Instance.ActiveDimension == Dimension.Ice && isOn);

        dimensionDependantVisualsOff.fireVisuals.SetActive(GameManager.Instance.ActiveDimension == Dimension.Ice && !isOn);
        dimensionDependantVisualsOff.iceVisuals.SetActive(GameManager.Instance.ActiveDimension == Dimension.Fire && !isOn);
    }

    public bool IsOnOriginDimension()
    {
        return GameManager.Instance.ActiveDimension == OriginDimension;
    }

    private void Damageable_Death()
    {
        _collider.enabled = false;

        EnemySpawner.RemoveEnemy(this);

        Destroy(gameObject, timeBeforeDestroyAfterDeath);
    }
}
