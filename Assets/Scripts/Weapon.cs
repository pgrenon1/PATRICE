using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int damage = 1;
    public int magazineSize = 30;
    public float rechargeRate = 0.5f;

    [Space]
    public Dimension dimension;
    public Projectile projectilePrefab;
    public Transform projectileOrigin;
    public float projectileSpeed = 1f;
    public float minTimeBetweenShots = 0.5f;
    public LayerMask layerMask;

    public float Magazine { get; private set; }

    private float _shotTimer;
    private bool _shootIsReady = true;

    private void Start()
    {
        Magazine = magazineSize;
    }

    private void Update()
    {
        UpdateMagazine();

        UpdateTimer();
    }

    private void UpdateMagazine()
    {
        if (IsOnOriginDimension() && Magazine <= magazineSize)
        {
            Magazine += Time.deltaTime * rechargeRate;

            if (Magazine > magazineSize)
                Magazine = magazineSize;
        }
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

    public void TryShoot(bool isPlayerProjectile)
    {
        if (!CanShoot())
            return;

        Projectile projectileInstance = Instantiate(projectilePrefab, projectileOrigin.position, projectileOrigin.rotation);

        projectileInstance.Init(projectileOrigin.forward, projectileSpeed, damage, layerMask, isPlayerProjectile, dimension);

        _shootIsReady = false;
        _shotTimer = minTimeBetweenShots;
        Magazine -= 1f;
    }

    private bool CanShoot()
    {
        return _shootIsReady && Magazine >= 1f;
    }

    public bool IsOnOriginDimension()
    {
        return GameManager.Instance.ActiveDimension == dimension;
    }
}
