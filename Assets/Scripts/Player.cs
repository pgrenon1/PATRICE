using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class Player : MonoBehaviour
{
    public float normalSpeed = 25f;
    public float stoppingPower = 1f;
    public float accelerationSpeed = 45f;
    public Transform cameraPosition;
    public Camera mainCamera;
    public Transform spaceshipRoot;
    public float rotationSpeed = 2.0f;
    public float cameraSmooth = 4f;
    public RectTransform crosshairTexture;
    public Weapon weapon;

    public Damageable Damageable { get; private set;}
    public float Speed { get; private set; }

    private Vector3 _lastPosition;
    private Rigidbody _rigidBody;
    private Quaternion _lookRotation;
    private float _rotationZ = 0;
    private float _xSmooth = 0;
    private float _YSmooth = 0;
    private Vector3 _defaultRotation;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _rigidBody.useGravity = false;
        _lookRotation = transform.rotation;
        _defaultRotation = spaceshipRoot.localEulerAngles;
        _rotationZ = _defaultRotation.z;

        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        Damageable = GetComponent<Damageable>();

        Damageable.Death += Damageable_Death;
    }

    private void Damageable_Death()
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
    }

    private void UpdateShoot()
    {
        float r2Value = Input.GetAxis("R2");

        if (r2Value != -1)
        {
            weapon.TryShoot();
        }
    }

    private void FixedUpdate()
    {
        UpdateMovement();

        UpdateCameraFollow();

        UpdateRotation();

        UpdateCrosshair();
    }

    private void UpdateCrosshair()
    {
        //Update crosshair texture
        if (crosshairTexture)
        {
            crosshairTexture.position = mainCamera.WorldToScreenPoint(transform.position + transform.forward * 100);
        }
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

        Quaternion localRotation = Quaternion.Euler(-_YSmooth, _xSmooth, rotationZTmp * rotationSpeed);
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

        _lastPosition = transform.position;
    }

    private void UpdateAcceleration()
    {
        if (Input.GetAxis("L2") > 0f) // square
        {
            Speed = Mathf.Lerp(Speed, accelerationSpeed, Time.deltaTime * 3);
        }
        //else if (Input.GetButton("Fire2")) // x
        //{
        //    Speed = Mathf.Lerp(Speed, normalSpeed, Time.deltaTime * 10);
        //}
        else
        {
            Speed = Mathf.Lerp(Speed, normalSpeed, Time.deltaTime * 10);
        }


        //else if ()
        //{
        //    Speed = Mathf.Lerp(Speed, 0, Time.deltaTime * stoppingPower);
        //}
    }

    public Vector3 GetVelocity()
    {
        return transform.position - _lastPosition;
    }
}