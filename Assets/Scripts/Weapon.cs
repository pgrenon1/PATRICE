using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int damage = 1;
    public Projectile projectilePrefab;
    public Transform projectileOrigin;
    public float projectileSpeed = 1f;
    public float minTimeBetweenShots = 0.5f;
    public LayerMask layerMask;

    private float _shotTimer;
    private bool _shootIsReady = true;

    private void Update()
    {
        UpdateTimer();
    }

    private void UpdateTimer()
    {
        if (_shootIsReady)
            return;

        _shotTimer -= Time.deltaTime;
    
        if (_shotTimer <= 0)
        {
            _shootIsReady = true;
        }
    }

    public void TryShoot()
    {
        if (!_shootIsReady)
            return;

        Projectile projectileInstance = Instantiate(projectilePrefab, projectileOrigin.position, projectileOrigin.rotation);

        projectileInstance.Init(projectileOrigin.forward, projectileSpeed, damage, layerMask);

        _shootIsReady = false;
        _shotTimer = minTimeBetweenShots;
    }
}
