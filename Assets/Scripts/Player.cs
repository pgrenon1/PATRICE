using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    public DimensionDependantVisuals visualsParent;

    [Space]
    public float dimensionSwitchCooldown;

    [Space]
    public float boostDuration;
    public float boostCooldown;
    public DimensionDependantVisuals boostParticlesParents;

    public float normalSpeed = 25f;
    public float stoppingPower = 1f;
    public float accelerationSpeed = 45f;
    public Transform cameraPosition;
    public Camera mainCamera;
    public Transform spaceshipRoot;
    public float rotationSpeed = 2.0f;
    public float cameraSmooth = 4f;
    public RectTransform crosshairTexture;
    public Image fireWeaponFill;
    public Image iceWeaponFill;
    public Image levelTimerFill;
    public TextMeshProUGUI scoreText;
    public Image switchDimensionFill;

    [Space]
    public float switchWeaponDimensionStickyTime = 0.2f;
    public Weapon fireWeapon;
    public Weapon iceWeapon;

    public Damageable Damageable { get; private set;}
    public float Speed { get; private set; }

    private Dimension _weaponDimension;
    private Rigidbody _rigidBody;
    private Quaternion _lookRotation;
    private float _rotationZ = 0;
    private float _xSmooth = 0;
    private float _YSmooth = 0;
    private Vector3 _defaultRotation;
    private float _dimensionSwitchCooldownTimer;
    private bool _isBoosting = false;
    private float _boostTimer;
    private float _boostCooldownTimer;
    private bool _boostIsInCooldown;
    private bool _isDimensionSwitchOnCooldown;
    private float _switchWeaponDimensionStickyTimer;
    private Color _switchDimensionFillColor;

    private void Start()
    {
        _switchDimensionFillColor = switchDimensionFill.color;
        _rigidBody = GetComponent<Rigidbody>();
        _rigidBody.useGravity = false;
        _lookRotation = transform.rotation;
        _defaultRotation = spaceshipRoot.localEulerAngles;
        _rotationZ = _defaultRotation.z;

        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        Damageable = GetComponent<Damageable>();

        Damageable.Death += Damageable_Death;

        GameManager.Instance.DimensionSwitched += Instance_DimensionSwitched;
    }

    private void Instance_DimensionSwitched(Dimension newActiveDimension)
    {
        visualsParent.SwitchVisuals(newActiveDimension);
    }

    private void Damageable_Death(bool onDimension = false, bool isPlayerDamage = false)
    {
        Speed = 0;
        _rigidBody.isKinematic = true;

        GameManager.Instance.GameOver();
    }

    private void Update()
    {
        UpdateInputs();
    }

    private void UpdateInputs()
    {
        if (Damageable.IsDead)
            return;

        UpdateShoot();

        UpdateAcceleration();

        UpdateDimensionSwitch();

        UpdateWeaponDimensionSwitch();

        UpdateBoost();
    }

    private void UpdateWeaponDimensionSwitch()
    {
        if (Input.GetButton("Jump") && _switchWeaponDimensionStickyTimer <= 0)
        {
            SwitchWeaponDimension();

            _switchWeaponDimensionStickyTimer = switchWeaponDimensionStickyTime;
        }

        if (_switchWeaponDimensionStickyTimer > 0)
        {
            _switchWeaponDimensionStickyTimer -= Time.deltaTime;
        }
    }

    private void SwitchWeaponDimension()
    {
        if (_weaponDimension == Dimension.Ice)
        {
            _weaponDimension = Dimension.Fire;
        }
        else if (_weaponDimension == Dimension.Fire)
        {
            _weaponDimension = Dimension.Ice;
        }
    }

    private void UpdateDimensionSwitch()
    {
        UpdateDimensionSwitchCooldown();

        if (Input.GetButton("L1") && Input.GetButton("R1") && !_isDimensionSwitchOnCooldown)
        {
            GameManager.Instance.SwitchDimension();

            _isDimensionSwitchOnCooldown = true;
            _dimensionSwitchCooldownTimer = dimensionSwitchCooldown;
        }
    }

    private void UpdateDimensionSwitchCooldown()
    {
        if (_dimensionSwitchCooldownTimer > 0)
        {
            _dimensionSwitchCooldownTimer -= Time.deltaTime;

            if (_dimensionSwitchCooldownTimer <= 0)
            _isDimensionSwitchOnCooldown = false;
        }
    }

    private void UpdateShoot()
    {
        float r2Value = Input.GetAxis("R2");

        if (r2Value != -1)
        {
            if (_weaponDimension == Dimension.Fire)
                fireWeapon.TryShoot(true);
            else
                iceWeapon.TryShoot(true);
        }
    }

    private void FixedUpdate()
    {
        UpdateMovement();

        UpdateCameraFollow();

        UpdateRotation();

        UpdateCrosshair();

        UpdateLevelTimer();

        UpdateScore();

        UpdateSwitchDimensionFill();
    }

    private void UpdateSwitchDimensionFill()
    {
        float ratio = 1 - _dimensionSwitchCooldownTimer / dimensionSwitchCooldown;

        if (_dimensionSwitchCooldownTimer > 0)
        {
            Color red = Color.red;
            red.a = 0.6f;
            switchDimensionFill.color = red;
        }
        else
        {
            switchDimensionFill.color = _switchDimensionFillColor;
        }

        switchDimensionFill.fillAmount = ratio;
    }

    private void UpdateScore()
    {
        scoreText.text = GameManager.Instance.Score.ToString();
    }

    private void UpdateLevelTimer()
    {
        levelTimerFill.fillAmount = GameManager.Instance.LevelTimer / GameManager.Instance.timePerLevel;
    }

    private void UpdateCrosshair()
    {
        //Update crosshair texture
        if (crosshairTexture)
        {
            crosshairTexture.position = mainCamera.WorldToScreenPoint(transform.position + transform.forward * 100);
        }

        UpdateWeaponUI(fireWeapon, fireWeaponFill);
        UpdateWeaponUI(iceWeapon, iceWeaponFill);
    }

    private void UpdateWeaponUI(Weapon weapon, Image fillImage)
    {
        fillImage.fillAmount = weapon.Magazine / weapon.magazineSize;
    }

    private void UpdateRotation()
    {
        //Rotation
        float rotationZTmp = Input.GetAxis("RightHorizontal");
        //if (Input.GetKey(KeyCode.A))
        //{
        //    rotationZTmp = 1;
        //}
        //else if (Input.GetKey(KeyCode.D))
        //{
        //    rotationZTmp = -1;
        //}

        _xSmooth = Mathf.Lerp(_xSmooth, Input.GetAxis("Horizontal") * rotationSpeed, Time.deltaTime * cameraSmooth);
        _YSmooth = Mathf.Lerp(_YSmooth, Input.GetAxis("Vertical") * rotationSpeed, Time.deltaTime * cameraSmooth);

        Quaternion localRotation = Quaternion.Euler(-_YSmooth, _xSmooth, -rotationZTmp * rotationSpeed);
        _lookRotation = _lookRotation * localRotation;
        transform.rotation = _lookRotation;

        _rotationZ -= _xSmooth;
        _rotationZ = Mathf.Clamp(_rotationZ, -45, 45);
        spaceshipRoot.transform.localEulerAngles = new Vector3(_defaultRotation.x, _defaultRotation.y, _rotationZ);
        _rotationZ = Mathf.Lerp(_rotationZ, _defaultRotation.z, Time.deltaTime * cameraSmooth);
    }

    private void UpdateCameraFollow()
    {
        //Camera follow
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, cameraPosition.position, Time.deltaTime * cameraSmooth);
        mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, cameraPosition.rotation, Time.deltaTime * cameraSmooth);
    }

    private void UpdateMovement()
    {
        //Set moveDirection to the vertical axis (up and down keys) * speed
        Vector3 moveDirection = new Vector3(0, 0, Speed);
        //Transform the vector3 to local space
        moveDirection = transform.TransformDirection(moveDirection);
        //Set the velocity, so you can move
        _rigidBody.velocity = new Vector3(moveDirection.x, moveDirection.y, moveDirection.z);
    }

    private void UpdateBoost()
    {
        boostParticlesParents.GetActiveVisuals().SetActive(_isBoosting);

        if (_isBoosting)
        {
            Speed = Mathf.Lerp(Speed, accelerationSpeed, Time.deltaTime * 3);

            _boostTimer += Time.deltaTime;

            if (_boostTimer >= boostDuration)
            {
                _isBoosting = false;
                _boostIsInCooldown = true;
            }
        }
        else if (_boostIsInCooldown)
        {
            _boostCooldownTimer -= Time.deltaTime;

            if (_boostCooldownTimer <= 0)
            {
                _boostIsInCooldown = false;
            }
        }
    }

    private void UpdateAcceleration()
    {
        if ((Input.GetButton("JR") || Input.GetButton("L2")) && !_isBoosting && !_boostIsInCooldown)
        {
            _isBoosting = true;

            _boostTimer = 0;
            _boostCooldownTimer = boostCooldown;
        }
        else
        {
            Speed = Mathf.Lerp(Speed, normalSpeed, Time.deltaTime * 10);
        }
    }
}

[Serializable]
public class DimensionDependantVisuals
{
    public GameObject fireVisuals;
    public GameObject iceVisuals;

    public void SwitchVisuals(Dimension dimension)
    {
        if (dimension == Dimension.Fire)
        {
            fireVisuals?.SetActive(true);
            iceVisuals?.SetActive(false);
        }
        else if (dimension == Dimension.Ice)
        {
            fireVisuals?.SetActive(false);
            iceVisuals?.SetActive(true);
        }
    }

    public GameObject GetActiveVisuals()
    {
        if (GameManager.Instance.ActiveDimension == Dimension.Fire)
        {
            return fireVisuals;
        }
        else // if (GameManager.Instance.ActiveDimension == Dimension.Ice)
        {
            return iceVisuals;
        }
    }
}

public enum Dimension
{
    Fire,
    Ice
}