using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject visualsParent;
    public GameObject hitEffectPrefab;
    public float hitEffectScale = 3f;

    public bool IsPlayerProjectile { get; set; }

    private int _damage;
    private Vector3 _lastPosition;
    private bool _isActive;
    private Vector3 _direction;
    private float _speed;
    private LayerMask _layerMask;
    private Collider _collider;
    private Dimension _dimension;

    private void Start()
    {
        _collider = GetComponentInChildren<Collider>();

        Despawn();
    }

    //private void Update()
    //{
    //    RaycastHit hitInfo;
    //    if (_isActive && Physics.Linecast(_lastPosition, transform.position, out hitInfo, _layerMask))
    //    {
    //        Hit(hitInfo.point, hitInfo.collider);
    //    }

    //    _lastPosition = transform.position;
    //}

    private void LateUpdate()
    {
        if (_isActive)
            transform.position = transform.position + _direction.normalized * _speed * Time.deltaTime;
    }

    public void Init(Vector3 direction, float speed, int damage, LayerMask layerMask, bool isPlayerProjectile, Dimension dimension)
    {
        _direction = direction;
        _speed = speed;
        _damage = damage;
        _layerMask = layerMask;
        IsPlayerProjectile = isPlayerProjectile;
        _dimension = dimension;

        if (IsPlayerProjectile && !IsOnOriginDimension())
        {
            transform.localScale *= 5f;
        }

        _isActive = true;
    }

    private void Despawn()
    {
        Destroy(gameObject, 4f);
    }

    private void Hit(Vector3 hitPoint, Collider collider)
    {
        Damageable damageable = collider.GetComponentInParent<Damageable>();

        if (damageable != null)
        {
            bool onDimension = false;
            Enemy enemy = damageable.GetComponent<Enemy>();
            if (enemy != null)
            {
                onDimension = enemy.OriginDimension == _dimension;
            }

            damageable.ApplyDamage(new Damage(_damage, onDimension, IsPlayerProjectile));
        }

        Utility.SpawnVFX(hitEffectPrefab, hitPoint, Quaternion.identity, hitEffectScale);

        if (visualsParent)
            visualsParent.SetActive(false);

        _isActive = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        _collider.enabled = false;

        Vector3 hitPoint = other.ClosestPoint(transform.position);

        Hit(hitPoint, other);
    }

    public bool IsOnOriginDimension()
    {
        return GameManager.Instance.ActiveDimension == _dimension;
    }
}