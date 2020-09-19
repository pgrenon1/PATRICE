using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject visualsParent;
    public GameObject hitEffectPrefab;

    private int _damage;
    private Vector3 _lastPosition;
    private bool _isActive;
    private Vector3 _direction;
    private float _speed;
    private LayerMask _layerMask;

    private void Start()
    {
        Despawn();
    }

    private void Update()
    {
        RaycastHit hitInfo;
        if (_isActive && Physics.Linecast(_lastPosition, transform.position, out hitInfo, _layerMask))
        {
            Hit(hitInfo.point, hitInfo.collider);
        }

        _lastPosition = transform.position;
    }

    private void LateUpdate()
    {
        if (_isActive)
            transform.position = transform.position + _direction.normalized * _speed * Time.deltaTime;
    }

    public void Init(Vector3 direction, float speed, int damage, LayerMask layerMask)
    {
        _direction = direction;
        _speed = speed;
        _damage = damage;
        _layerMask = layerMask;

        _isActive = true;
    }

    private void Despawn()
    {
        Destroy(gameObject, 4f);
    }

    private void Hit(Vector3 hitPoint, Collider collider)
    {
        Damageable damageable = collider.GetComponentInParent<Damageable>();

        //Debug.Log(collider.gameObject, collider.gameObject);

        if (damageable != null)
        {
            damageable.ApplyDamage(_damage);
        }

        Utility.SpawnVFX(hitEffectPrefab, hitPoint, Quaternion.identity);

        if (visualsParent)
            visualsParent.SetActive(false);

        _isActive = false;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    other.TryDamage(_damage)
    //}
}